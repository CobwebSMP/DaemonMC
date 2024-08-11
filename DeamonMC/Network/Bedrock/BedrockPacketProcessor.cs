using DeamonMC.Network.RakNet;
using DeamonMC.Utils;
using DeamonMC.Utils.Text;

namespace DeamonMC.Network.Bedrock
{
    public class BedrockPacketProcessor
    {
        public static void RequestNetworkSettings(RequestNetworkSettingsPacket packet)
        {
            Log.info($"New player ({RakSessionManager.getSession(Server.clientEp).GUID}) log in with protocol version: {packet.protocolVersion}");
            var pk = new NetworkSettingsPacket
            {
                compressionThreshold = 0,
                compressionAlgorithm = 255,
                clientThrottleEnabled = false,
                clientThrottleScalar = 0,
                clientThrottleThreshold = 0
            };
            NetworkSettings.Encode(pk);
            RakSessionManager.Compression(Server.clientEp, true);
            /*var pk2 = new DisconnectPacket
            {

            };
            Disconnect.Encode(pk2);*/
        }

        public static void Login(LoginPacket packet)
        {
            var filteredJWT = new string(packet.request.Where(c => c >= 32 && c <= 126).ToArray());
            int jsonEndIndex = filteredJWT.LastIndexOf('}');
            if (jsonEndIndex >= 0)
            {
                filteredJWT = filteredJWT.Substring(0, jsonEndIndex + 1);
            }
            JWT.processJWTchain(filteredJWT);
        }

        public static void PacketViolationWarning(PacketViolationWarningPacket packet)
        {
            Log.error($"Client reported that server sent failed packet '{(Info.Bedrock)packet.packetId}'");
            Log.error(packet.description);
        }
    }
}
