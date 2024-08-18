using DaemonMC.Network.Bedrock;
using DaemonMC.Utils;
using System.Text;

namespace DaemonMC.Network.Handler
{
    public class Login
    {
        public static void execute(LoginPacket packet)
        {
            byte[] jwtBuffer = Encoding.UTF8.GetBytes(packet.request);

            var filteredJWT = new string(packet.request.Where(c => c >= 32 && c <= 126).ToArray());
            int jsonEndIndex = filteredJWT.LastIndexOf('}');
            if (jsonEndIndex >= 0)
            {
                filteredJWT = filteredJWT.Substring(0, jsonEndIndex + 1);
            }
            string Token = Encoding.UTF8.GetString(jwtBuffer, filteredJWT.Length + 8, jwtBuffer.Length - (filteredJWT.Length + 8));

            JWT.processJWTchain(filteredJWT);
            JWT.processJWTtoken(Token);

            var encrypt = false;
            if (encrypt)
            {
                var jwt = JWT.createJWT();
                var pk = new ServerToClientHandshakePacket
                {
                    JWT = jwt,
                };
                ServerToClientHandshake.Encode(pk);
            }
            else
            {
                var pk = new PlayStatusPacket
                {
                    status = 0,
                };
                PlayStatus.Encode(pk);
            }
        }
    }
}
