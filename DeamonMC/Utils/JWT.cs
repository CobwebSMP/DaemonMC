using System.IdentityModel.Tokens.Jwt;
using System.Reflection.PortableExecutable;
using System.Security.Cryptography;
using System.Text;
using DeamonMC.Network.RakNet;
using DeamonMC.Utils.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
namespace DeamonMC.Utils
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
            var player = RakSessionManager.sessions[Server.clientEp];
            JWTObject decodedObject = JsonConvert.DeserializeObject<JWTObject>(jsonString);
            var handler = new JwtSecurityTokenHandler();
            string identityPublicKey = null;
            foreach (var jwtToken in decodedObject.Chain)
            {
                var jsonToken = handler.ReadToken(jwtToken) as JwtSecurityToken;
                if (jsonToken != null)
                {
                    var publicKeyClaim = jsonToken.Claims.FirstOrDefault(claim => claim.Type == "identityPublicKey");
                    if (publicKeyClaim != null)
                    {
                        if (publicKeyClaim.Value == RootKey)
                        {
                            Log.debug("Got certificate");
                            identityPublicKey = publicKeyClaim.Value;
                            Log.debug("Mojang RootKey: OK");
                        }
                    }

                    var extraDataClaim = jsonToken.Claims.FirstOrDefault(claim => claim.Type == "extraData");
                    if (extraDataClaim != null)
                    {
                        ExtraData extraData = JsonConvert.DeserializeObject<ExtraData>(extraDataClaim.Value);
                        player.username = extraData.displayName;
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
            var player = RakSessionManager.sessions[Server.clientEp];
            string[] tokenParts = rawToken.Split('.');

            string headerJson = DecodeBase64Url(tokenParts[0]);
            string payloadJson = DecodeBase64Url(tokenParts[1]);

            JObject header = JObject.Parse(headerJson);
            JwtPayload payload = JsonConvert.DeserializeObject<JwtPayload>(payloadJson);

            string publicKey = header["x5u"].ToString();

            Log.debug($"Public Key (x5u): {publicKey}");
            Log.info($"{player.username} with client version {payload.GameVersion} doing login...");

            //Console.WriteLine("JWT Header:");
            // Console.WriteLine(JsonConvert.SerializeObject(headerJson, Formatting.Indented));

            //Console.WriteLine("JWT Payload:");
            //Console.WriteLine(payloadJson);
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

        private static string GenerateSalt()
        {
            var random = new byte[32];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(random);
            }
            return Convert.ToBase64String(random);
        }
    }
}