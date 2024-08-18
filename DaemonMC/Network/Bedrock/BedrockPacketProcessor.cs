using System.Drawing;
using System.Text;
using DaemonMC.Network.Handler;
using DaemonMC.Network.RakNet;
using DaemonMC.Utils;
using DaemonMC.Utils.Text;

namespace DaemonMC.Network.Bedrock
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
            RakSessionManager.Compression(Server.clientEp, true);
        }

        public static void Login(LoginPacket packet)
        {
            Handler.Login.execute(packet);
        }

        public static void PacketViolationWarning(PacketViolationWarningPacket packet)
        {
            Log.error($"Client reported that server sent failed packet '{(Info.Bedrock)packet.packetId}'");
            Log.error(packet.description);
        }

        public static void ClientCacheStatus(ClientCacheStatusPacket packet)
        {
            var player = RakSessionManager.getCurrentSession();
            Log.debug($"{player.username} ClientCacheStatus = {packet.status}");

            var pk1 = new ResourcePacksInfoPacket
            {
                force = false,
                isAddon = false,
                hasScripts = false,
                forceServerPacks = false
            };
            ResourcePacksInfo.Encode(pk1);
        }

        public static void ResourcePackClientResponse(ResourcePackClientResponsePacket packet)
        {
            Log.debug($"ResourcePackClientResponse = {packet.response}");
            if (packet.response == 3)
            {
                var pk = new ResourcePackStackPacket
                {
                    forceTexturePack = false,
                };
                ResourcePackStack.Encode(pk);
            }
            else if (packet.response == 4) //start game
            {
                preSpawn.execute();
            }
        }

        public static void RequestChunkRadius(RequestChunkRadiusPacket packet)
        {
            var player = RakSessionManager.getCurrentSession();
            Log.debug($"{player.username} requested chunks with radius {packet.radius}. Max radius = {packet.maxRadius}");
            var pk = new ChunkRadiusUpdatedPacket
            {
                radius = packet.radius,
            };
            ChunkRadiusUpdated.Encode(pk);
            return;
            for (int x = -20; x <= 20; x++)
            {
                for (int z = -20; z <= 20; z++)
                {
                    Log.debug($"({x}, {z})");
                    var pk1 = new LevelChunkPacket
                    {
                        chunkX = x,
                        chunkZ = z,
                        data = ""
                    };
                    LevelChunk.Encode(pk1);
                }
            }
        }
    }
}
