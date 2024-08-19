namespace DaemonMC.Network.Bedrock
{
    public class InteractPacket
    {
        public byte action { get; set; }
        public long actorRuntimeId { get; set; }
    }

    public class Interact
    {
        public const int id = 33;
        public static void Decode(byte[] buffer)
        {
            var packet = new InteractPacket
            {
                action = DataTypes.ReadByte(buffer),
                actorRuntimeId = DataTypes.ReadVarLong(buffer)
            };

            BedrockPacketProcessor.Interact(packet);
        }

        public static void Encode(InteractPacket fields)
        {

        }
    }
}
