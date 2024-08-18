namespace DaemonMC.Network.Bedrock
{
    public class RequestChunkRadiusPacket
    {
        public int radius { get; set; }
        public byte maxRadius { get; set; }
    }

    public class RequestChunkRadius
    {
        public const byte id = 69;
        public static void Decode(byte[] buffer)
        {
            var packet = new RequestChunkRadiusPacket
            {
                radius = DataTypes.ReadVarInt(buffer),
                maxRadius = DataTypes.ReadByte(buffer)
            };
            BedrockPacketProcessor.RequestChunkRadius(packet);
        }

        public static void Encode(LoginPacket fields)
        {

        }
    }
}
