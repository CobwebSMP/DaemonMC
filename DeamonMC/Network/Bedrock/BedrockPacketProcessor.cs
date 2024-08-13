using System;
using System.Text;
using DeamonMC.Network.RakNet;
using DeamonMC.Utils;
using DeamonMC.Utils.Text;

namespace DeamonMC.Network.Bedrock
{
    public class BedrockPacketProcessor
    {
        public static void RequestNetworkSettings(RequestNetworkSettingsPacket packet)
        {
            Log.debug($"New player ({RakSessionManager.getSession(Server.clientEp).GUID}) log in with protocol version: {packet.protocolVersion}");
            var pk = new NetworkSettingsPacket
            {
                compressionThreshold = 0,
                compressionAlgorithm = 0,
                clientThrottleEnabled = false,
                clientThrottleScalar = 0,
                clientThrottleThreshold = 0
            };
            NetworkSettings.Encode(pk);
            //RakSessionManager.Compression(Server.clientEp, true);
            /*var pk2 = new DisconnectPacket
            {

            };
            Disconnect.Encode(pk2);*/
        }

        public static void Login(LoginPacket packet)
        {
            //Log.info(packet.protocolVersion.ToString());
            //Log.info(packet.request);
            byte[] jwtBuffer = Encoding.UTF8.GetBytes(packet.request);
            int CertificateChainSize = BitConverter.ToInt32(jwtBuffer, 0);
           // Log.info(CertificateChainSize.ToString());
            string CertificateChain = Encoding.UTF8.GetString(jwtBuffer, 4, CertificateChainSize);
            //Log.info(TokenSize.ToString());
            string Token = Encoding.UTF8.GetString(jwtBuffer, CertificateChainSize + 12, jwtBuffer.Length - (CertificateChainSize + 12)); //weird way but .. maybe later

            //Log.debug("Raw CertificateChain:");
            //Log.debug(CertificateChain);

            //Log.debug("Raw Token");
            JWT.processJWTchain(CertificateChain);
            JWT.processJWTtoken(Token);
            /*
             var jwt = JWT.processJWTchain(filteredJWT);
             Log.debug("Signed chain:");
            // Log.debug(jwt);
             var pk = new ServerToClientHandshakePacket
             {
                 JWT = jwt,
             };
             ServerToClientHandshake.Encode(pk);*/
        }

        public static void PacketViolationWarning(PacketViolationWarningPacket packet)
        {
            Log.error($"Client reported that server sent failed packet '{(Info.Bedrock)packet.packetId}'");
            Log.error(packet.description);
        }
    }
}
