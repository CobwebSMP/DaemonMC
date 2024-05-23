using DeamonMC.Utils.Text;

namespace DeamonMC.Network.Bedrock
{
    public class BedrockPacketDecoder
    {
        public static void BedrockDecoder(byte[] buffer)
        {
            DataTypes.ReadByte(buffer);
            var pkid = DataTypes.ReadVarInt(buffer);
            Log.debug($"[Server] <-- [{Server.clientEp.Address,-16}:{Server.clientEp.Port}] {(Info.Bedrock)pkid}");

            switch (pkid)
            {
                case RequestNetworkSettings.id:
                    RequestNetworkSettings.Decode(buffer);
                    break;
                default:
                    Log.error($"[Server] Unknown Bedrock packet: {pkid}");
                    break;
            }
        }
    }
}
