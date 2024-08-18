using DaemonMC.Utils;
namespace DaemonMC.Network.Bedrock
{
    public class ChunkRadiusUpdatedPacket
    {
        public int radius { get; set; }
    }

    public class ChunkRadiusUpdated
    {
        public static byte id = 70;
        public static void Decode(byte[] buffer)
        {

        }

        public static void Encode(ChunkRadiusUpdatedPacket fields)
        {
            DataTypes.WriteVarInt(id);
            DataTypes.WriteVarInt(fields.radius);
            PacketEncoder.handlePacket("bedrock");
        }
    }
}
