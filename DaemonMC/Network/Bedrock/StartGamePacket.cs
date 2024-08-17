namespace DaemonMC.Network.Bedrock
{
    public class StartGamePacketPacket
    {
        public string JWT { get; set; }
    }

    public class StartGamePacket
    {
        public static byte id = 1;
        public static void Decode(byte[] buffer)
        {

        }

        public static void Encode(StartGamePacketPacket fields)
        {
            DataTypes.WriteVarInt(id);
            DataTypes.WriteString(fields.JWT);
            PacketEncoder.handlePacket("bedrock");
        }
    }
}
