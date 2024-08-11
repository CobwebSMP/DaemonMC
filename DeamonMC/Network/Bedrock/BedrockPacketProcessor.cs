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
            //Log.debug($"net vers {packet.protocolVersion} ");
            Log.debug("Got JWT data");
            Log.info(packet.request);
        }

        public static void PacketViolationWarning(PacketViolationWarningPacket packet)
        {
            Log.error($"Client reported that server sent failed packet '{(Info.Bedrock)packet.packetId}'");
            Log.error(packet.description);
        }
    }
}
