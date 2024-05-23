using DeamonMC.Network.RakNet;
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
                compressionAlgorithm = 0,
                clientThrottleEnabled = false,
                clientThrottleScalar = 0,
                clientThrottleThreshold = 0
            };
            NetworkSettings.Encode(pk);
            //RakSessionManager.Compression(Server.clientEp, true);
        }
    }
}
