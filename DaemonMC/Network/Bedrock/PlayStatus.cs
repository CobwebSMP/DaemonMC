namespace DaemonMC.Network.Bedrock
{
    public class PlayStatusPacket
    {
        public int status { get; set; }
    }

    public class PlayStatus
    {
        public static byte id = 2;
        public static void Decode(byte[] buffer)
        {

        }

        public static void Encode(PlayStatusPacket fields)
        {
            DataTypes.WriteVarInt(id);
            DataTypes.WriteInt(fields.status);
            PacketEncoder.handlePacket("bedrock");
        }
    }
}
