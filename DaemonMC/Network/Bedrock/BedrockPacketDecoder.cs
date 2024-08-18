using DaemonMC.Network.RakNet;
using DaemonMC.Utils.Text;

namespace DaemonMC.Network.Bedrock
{
    public class BedrockPacketDecoder
    {
        public static void BedrockDecoder(byte[] buffer)
        {
            if (RakSessionManager.getSession(Server.clientEp) != null)
            {
                if (RakSessionManager.getSession(Server.clientEp).initCompression)
                {
                    DataTypes.ReadByte(buffer);
                }
            }
            var size = DataTypes.ReadVarInt(buffer); //packet size
            var pkid = DataTypes.ReadVarInt(buffer);
            Log.debug($"[Server] <-- [{Server.clientEp.Address,-16}:{Server.clientEp.Port}] {(Info.Bedrock)pkid}");

            switch (pkid)
            {
                case RequestNetworkSettings.id:
                    RequestNetworkSettings.Decode(buffer);
                    break;
                case Login.id:
                    Login.Decode(buffer);
                    break;
                case PacketViolationWarning.id:
                    PacketViolationWarning.Decode(buffer);
                    break;
                case ClientCacheStatus.id:
                    ClientCacheStatus.Decode(buffer);
                    break;
                case ResourcePackClientResponse.id:
                    ResourcePackClientResponse.Decode(buffer);
                    break;
                case RequestChunkRadius.id:
                    RequestChunkRadius.Decode(buffer);
                    break;
                case MovePlayer.id:
                    MovePlayer.Decode(buffer);
                    break;

                default:
                    Log.error($"[Server] Unknown Bedrock packet: {pkid}");
                    DataTypes.HexDump(buffer, buffer.Length);
                    break;
            }
        }
    }
}
