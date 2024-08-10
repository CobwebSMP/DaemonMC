namespace DeamonMC.Network.Bedrock
{
    public class PacketViolationWarningPacket
    {
        public int type { get; set; }
        public int serverity { get; set; }
        public int packetId { get; set; }
        public string description { get; set; }
    }

    public class PacketViolationWarning
    {
        public const byte id = 156;
        public static void Decode(byte[] buffer)
        {
            var packet = new PacketViolationWarningPacket
            {
                type = DataTypes.ReadSignedVarInt(buffer),
                serverity = DataTypes.ReadSignedVarInt(buffer),
                packetId = DataTypes.ReadSignedVarInt(buffer),
                description = DataTypes.ReadString(buffer),
            };

            BedrockPacketProcessor.PacketViolationWarning(packet);
        }

        public static void Encode(PacketViolationWarningPacket fields)
        {

        }
    }
}
