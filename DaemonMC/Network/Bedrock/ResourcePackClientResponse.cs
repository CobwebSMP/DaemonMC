namespace DaemonMC.Network.Bedrock
{
    public class ResourcePackClientResponsePacket
    {
        public byte response { get; set; }
    }

    public class ResourcePackClientResponse
    {
        public const byte id = 8;
        public static void Decode(byte[] buffer)
        {
            var packet = new ResourcePackClientResponsePacket
            {
                response = DataTypes.ReadByte(buffer),
            };
            DataTypes.ReadShort(buffer);
            BedrockPacketProcessor.ResourcePackClientResponse(packet);
        }

        public static void Encode(LoginPacket fields)
        {

        }
    }
}
