using DaemonMC.Utils;
namespace DaemonMC.Network.Bedrock
{
    public class LevelChunkPacket
    {
        public int chunkX { get; set; }
        public int chunkZ { get; set; }
        public string data { get; set; }
    }

    public class LevelChunk
    {
        public static int id = 58;
        public static void Decode(byte[] buffer)
        {

        }

        public static void Encode(LevelChunkPacket fields)
        {
            DataTypes.WriteVarInt(id);
            DataTypes.WriteSignedVarInt(fields.chunkX);
            DataTypes.WriteSignedVarInt(fields.chunkZ);
            DataTypes.WriteSignedVarInt(0);
            DataTypes.WriteVarInt(0);
            DataTypes.WriteBool(false);
            DataTypes.WriteString(fields.data);
            PacketEncoder.handlePacket();
        }
    }
}
