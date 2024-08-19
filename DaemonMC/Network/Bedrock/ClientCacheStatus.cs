namespace DaemonMC.Network.Bedrock
{
    public class ClientCacheStatusPacket
    {
        public bool status { get; set; }
    }

    public class ClientCacheStatus
    {
        public const int id = 129;
        public static void Decode(byte[] buffer)
        {
            var packet = new ClientCacheStatusPacket
            {
                status = DataTypes.ReadBool(buffer),
            };

            BedrockPacketProcessor.ClientCacheStatus(packet);
        }

        public static void Encode(ClientCacheStatusPacket fields)
        {

        }
    }
}
