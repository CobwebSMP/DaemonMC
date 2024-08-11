using DeamonMC.Network.RakNet;
using DeamonMC.Utils.Text;

namespace DeamonMC.Network.Bedrock
{
    public class BedrockPacketDecoder
    {
        public static void BedrockDecoder(byte[] buffer)
        {
           DataTypes.ReadVarInt(buffer); //packet size
           if (RakSessionManager.getSession(Server.clientEp).initCompression)
           {
               /* var compression = DataTypes.ReadByte(buffer); //compression type
                Log.info(compression.ToString());
                try
                {
                    var snappy = new SnappyDecompressor();
                    byte[] decompressedData = snappy.Decompress(buffer, 1, buffer.Length-1);
                    buffer = decompressedData;
                }
                catch (Exception ex)
                {
                    Log.error("An error occurred during decompression: " + ex.Message);
                }*/
            }
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
                default:
                    Log.error($"[Server] Unknown Bedrock packet: {pkid}");
                    break;
            }
        }
    }
}
