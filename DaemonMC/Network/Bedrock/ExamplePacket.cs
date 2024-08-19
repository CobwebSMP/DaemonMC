namespace DaemonMC.Network.Bedrock
{
    public class ExamplePacket
    {
        public int variable { get; set; }
    }

    public class Example
    {
        public static int id = 0;
        public static void Decode(byte[] buffer)
        {

        }

        public static void Encode(ExamplePacket fields)
        {
            DataTypes.WriteVarInt(id);
            PacketEncoder.handlePacket();
        }
    }
}
