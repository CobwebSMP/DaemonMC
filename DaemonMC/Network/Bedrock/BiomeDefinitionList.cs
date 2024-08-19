using fNbt;
namespace DaemonMC.Network.Bedrock
{
    public class BiomeDefinitionListPacket
    {
        public NbtCompound biomeData { get; set; }
    }

    public class BiomeDefinitionList
    {
        public static int id = 122;
        public static void Decode(byte[] buffer)
        {

        }

        public static void Encode(BiomeDefinitionListPacket fields)
        {
            DataTypes.WriteVarInt(id);
            DataTypes.WriteCompoundTag(fields.biomeData);
            PacketEncoder.handlePacket();
        }
    }
}
