namespace DaemonMC.Network.Bedrock
{
    public class ResourcePackStackPacket
    {
        public bool forceTexturePack { get; set; }
    }

    public class ResourcePackStack
    {
        public static int id = 7;
        public static void Decode(byte[] buffer)
        {

        }

        public static void Encode(ResourcePackStackPacket fields)
        {
            DataTypes.WriteVarInt(id);
            DataTypes.WriteBool(fields.forceTexturePack);
            DataTypes.WriteVarInt(0); //add-on list
            DataTypes.WriteVarInt(0); //texture pack list
            DataTypes.WriteString(Info.version);
            DataTypes.WriteInt(0); //experiments
            DataTypes.WriteBool(false); //experiments was on
            DataTypes.WriteBool(false); //editor packs
            PacketEncoder.handlePacket();
        }
    }
}
