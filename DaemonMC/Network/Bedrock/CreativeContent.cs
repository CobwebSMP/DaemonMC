namespace DaemonMC.Network.Bedrock
{
    public class CreativeContentPacket
    {

    }

    public class CreativeContent
    {
        public static byte id = 145;
        public static void Decode(byte[] buffer)
        {

        }

        public static void Encode(CreativeContentPacket fields)
        {
            DataTypes.WriteVarInt(id);
            DataTypes.WriteVarInt(0);
            PacketEncoder.handlePacket("bedrock");
        }
    }
}
