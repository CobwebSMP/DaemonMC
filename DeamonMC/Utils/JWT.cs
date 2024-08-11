using System.IdentityModel.Tokens.Jwt;
using DeamonMC.Utils.Text;
using Newtonsoft.Json;
namespace DeamonMC.Utils
{
    public class JWTObject
    {
        public List<string> Chain { get; set; }
    }

    public class JWT
    {
        public static void processJWTchain(string jsonString)
        {
            JWTObject decodedObject = JsonConvert.DeserializeObject<JWTObject>(jsonString);
            //Console.WriteLine(JsonConvert.SerializeObject(decodedObject, Formatting.Indented));
            var handler = new JwtSecurityTokenHandler();

            foreach (var jwtToken in decodedObject.Chain)
            {
                var jsonToken = handler.ReadToken(jwtToken) as JwtSecurityToken;

                if (jsonToken != null)
                {
                    Log.debug("JWT Claims:");
                    foreach (var claim in jsonToken.Claims)
                    {
                        Log.debug($"{claim.Type}: {claim.Value}");
                    }

                    var extraDataClaim = jsonToken.Claims.FirstOrDefault(claim => claim.Type == "extraData");
                    if (extraDataClaim != null)
                    {
                       // Console.WriteLine("got extraData.");
                    }
                }
                else
                {
                    Log.error("Failed to decode JWT.");
                }
            }
        }
    }
}
