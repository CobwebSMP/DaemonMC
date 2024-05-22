namespace DeamonMC.Network.Bedrock
{
    public class RequestNetworkSettingsPacket
    {
        public int protocolVersion { get; set; }
    }

    public class RequestNetworkSettings
    {
        public static byte id = 193;
        public static void Decode(byte[] buffer)
        {
            var packet = new RequestNetworkSettingsPacket
            {
                protocolVersion = DataTypes.ReadIntBE(buffer),
            };

            BedrockPacketProcessor.RequestNetworkSettings(packet);
        }

        public static void Encode(RequestNetworkSettingsPacket fields)
        {

        }
    }
}
