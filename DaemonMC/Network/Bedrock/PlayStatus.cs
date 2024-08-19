namespace DaemonMC.Network.Bedrock
{
    public class PlayStatusPacket
    {
        public int status { get; set; }
    }

    public class PlayStatus
    {
        public static int id = 2;
        public static void Decode(byte[] buffer)
        {

        }

        public static void Encode(PlayStatusPacket fields)
        {
            DataTypes.WriteVarInt(id);
            DataTypes.WriteIntBE(fields.status);
            PacketEncoder.handlePacket();
        }
    }
}
