namespace DaemonMC.Network.Bedrock
{
    public class ServerboundLoadingScreenPacket
    {
        public int screenType { get; set; }
        public int screenId { get; set; }
    }

    public class ServerboundLoadingScreen
    {
        public const int id = 312;
        public static void Decode(byte[] buffer)
        {
            var packet = new ServerboundLoadingScreenPacket
            {
                screenType = DataTypes.ReadVarInt(buffer),
                screenId = DataTypes.ReadInt(buffer)
            };

            BedrockPacketProcessor.ServerboundLoadingScreen(packet);
        }

        public static void Encode(ServerboundLoadingScreenPacket fields)
        {

        }
    }
}
