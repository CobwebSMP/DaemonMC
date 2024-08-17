using System.Text;
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

                var pk2 = new ResourcePacksInfoPacket
                {
                    force = false,
                    isAddon = false,
                    hasScripts = false,
                    forceServerPacks = false

                };
                ResourcePacksInfo.Encode(pk2);
            }
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
                var session = RakSessionManager.getCurrentSession();

                Player player = new Player();
                player.username = session.username;

                long EntityId = Server.AddPlayer(player);
                session.EntityID = EntityId;

                var pk1 = new DisconnectPacket
                {
                    message = $"Yayy hi {RakSessionManager.sessions[Server.clientEp].username}. All works, your entity id is {EntityId}! That's all for now. Start game packets coming soon."
                };
                Disconnect.Encode(pk1);
            }
        }
    }
}
