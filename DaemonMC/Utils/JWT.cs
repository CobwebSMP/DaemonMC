using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Text;
using DaemonMC.Network.RakNet;
using DaemonMC.Utils.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.X509;
using Org.BouncyCastle.Crypto.Agreement;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto;
namespace DaemonMC.Utils
{
    public class JWTObject
    {
        public List<string> Chain { get; set; }
    }

    public class JWT
    {
        public const string RootKey = "MHYwEAYHKoZIzj0CAQYFK4EEACIDYgAECRXueJeTDqNRRgJi/vlRufByu/2G0i2Ebt6YMar5QX/R0DIIyrJMcUpruK4QveTfJSTp3Shlq4Gk34cD/4GUWwkv0DVuzeuB+tXija7HBxii03NHDbPAD0AKnLr2wdAp";

        public static void processJWTchain(string jsonString)
        {
            var player = RakSessionManager.getCurrentSession();
            JWTObject decodedObject = JsonConvert.DeserializeObject<JWTObject>(jsonString);
            var handler = new JwtSecurityTokenHandler();

            foreach (var jwtToken in decodedObject.Chain)
            {
                var jsonToken = handler.ReadToken(jwtToken) as JwtSecurityToken;
                var x5u = jsonToken.Header["x5u"].ToString();

                if (jsonToken != null)
                {
                    if (x5u == RootKey)
                    {
                        Log.debug("Mojang RootKey: OK");
                    }

                    var extraDataClaim = jsonToken.Claims.FirstOrDefault(claim => claim.Type == "extraData");
                    if (extraDataClaim != null)
                    {
                        ExtraData extraData = JsonConvert.DeserializeObject<ExtraData>(extraDataClaim.Value);
                        player.username = extraData.displayName;
                        player.identity = extraData.identity;
                    }
                    else
                    {
                        //Disconnect if online mode false
                    }
                }
                else
                {
                    Log.error("Failed to decode JWT.");
                }
            }
        }

        public static void processJWTtoken(string rawToken)
        {
            var player = RakSessionManager.getCurrentSession();
            int index = rawToken.IndexOf("ey");
            string[] tokenParts = rawToken.Substring(index).Split('.');

            string headerJson = DecodeBase64Url(tokenParts[0]);
            string payloadJson = DecodeBase64Url(tokenParts[1]);

            JObject header = JObject.Parse(headerJson);
            JwtPayload payload = JsonConvert.DeserializeObject<JwtPayload>(payloadJson);

            string publicKey = header["x5u"].ToString();

            Log.debug($"Public Key (x5u): {publicKey}");
            Log.info($"{player.username} with client version {payload.GameVersion} doing login...");
        }

        public static string createJWT()
        {
            var player = RakSessionManager.getCurrentSession();

            ECPublicKeyParameters rootECKey = (ECPublicKeyParameters) PublicKeyFactory.CreateKey(Convert.FromBase64String(RootKey));

            var generator = new ECKeyPairGenerator("ECDH");
            generator.Init(new ECKeyGenerationParameters(rootECKey.PublicKeyParamSet, SecureRandom.GetInstance("SHA256PRNG")));
            var keyPair = generator.GenerateKeyPair();

            ECPublicKeyParameters publicECKey = (ECPublicKeyParameters)keyPair.Public;
            ECPrivateKeyParameters privateECKey = (ECPrivateKeyParameters)keyPair.Private;

            var base64PublicKey = Convert.ToBase64String(SubjectPublicKeyInfoFactory.CreateSubjectPublicKeyInfo(publicECKey).GetEncoded());

            var header = new Dictionary<string, object>
            {
                {"alg", "ES384"},
                {"typ", "JWT"},
                {"x5u", base64PublicKey}
            };

            var handshakeJson = new
            {
                salt = GenerateSalt(),
                signedToken = ""
            };

            var agreement = new ECDHBasicAgreement();
            agreement.Init(privateECKey);

            byte[] secretPrepend = Encoding.UTF8.GetBytes(handshakeJson.salt); 
            byte[] sharedSecret = agreement.CalculateAgreement(rootECKey).ToByteArrayUnsigned();
            byte[] saltHash;
            using (SHA256 sha256 = SHA256.Create())
            {
                saltHash = sha256.ComputeHash(secretPrepend.Concat(sharedSecret).ToArray());
            }

            Log.debug($"saltHash {Convert.ToBase64String(saltHash)}");

            string payloadJson = JsonConvert.SerializeObject(handshakeJson);

            string encodedHeader = EncodeBase64Url(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(header)));
            string encodedPayload = EncodeBase64Url(Encoding.UTF8.GetBytes(payloadJson));

            var signEC = new ECParameters
            {
                Curve = ECCurve.NamedCurves.nistP384,
                Q =
                    {
                        X = publicECKey.Q.AffineXCoord.GetEncoded(),
                        Y = publicECKey.Q.AffineYCoord.GetEncoded()
                    }
                };
            signEC.D = (privateECKey.D.ToByteArrayUnsigned());

            var signKey = ECDsa.Create(signEC);

            string dataToSign = $"{encodedHeader}.{encodedPayload}";
            byte[] signatureBytes = SignData(signKey, dataToSign);

            string encodedSignature = EncodeBase64Url(signatureBytes);
            var aesEngine = new AesEngine();
            var sicBlockCipher = new SicBlockCipher(aesEngine);

            var cipher = new BufferedBlockCipher(sicBlockCipher);

            byte[] iv = saltHash.Take(12).Concat(new byte[] { 0, 0, 0, 2 }).ToArray();

            cipher.Init(false, new ParametersWithIV(new KeyParameter(saltHash), iv));

            player.decryptor = cipher;

            Log.debug($"Encrypted connection established with {RakSessionManager.sessions[Server.clientEp].username}");

            return $"{encodedHeader}.{encodedPayload}.{encodedSignature}";
        }

        private static string DecodeBase64Url(string base64Url)
        {
            string base64 = base64Url.Replace('-', '+').Replace('_', '/');
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }

            byte[] data = Convert.FromBase64String(base64);
            return Encoding.UTF8.GetString(data);
        }

        public static string EncodeBase64Url(byte[] input)
        {
            return Convert.ToBase64String(input)
                .TrimEnd('=')
                .Replace('+', '-')
                .Replace('/', '_');
        }

        private static string GenerateSalt()
        {
            var random = new byte[32];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(random);
            }
            return Convert.ToBase64String(random);
        }

        private static byte[] SignData(ECDsa privateKey, string data)
        {
            byte[] dataBytes = Encoding.UTF8.GetBytes(data);
            return privateKey.SignData(dataBytes, HashAlgorithmName.SHA384);
        }
    }
}