namespace DaemonMC.Network.Bedrock
{
    public class ResourcePacksInfoPacket
    {
        public bool force { get; set; }
        public bool isAddon { get; set; }
        public bool hasScripts { get; set; }
        public bool forceServerPacks { get; set; }
        //behaviour packs todo
        //resource packs todo
    }

    public class ResourcePacksInfo
    {
        public static byte id = 6;
        public static void Decode(byte[] buffer)
        {

        }

        public static void Encode(ResourcePacksInfoPacket fields)
        {
            DataTypes.WriteVarInt(id);
            DataTypes.WriteBool(fields.force);
            DataTypes.WriteBool(fields.isAddon);
            DataTypes.WriteBool(fields.hasScripts);
            DataTypes.WriteBool(fields.forceServerPacks);
            DataTypes.WriteShort(0);
            DataTypes.WriteShort(0);
            DataTypes.WriteVarInt(0);
            PacketEncoder.handlePacket("bedrock");
        }
    }
}
