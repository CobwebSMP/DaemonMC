namespace DeamonMC.Network.Bedrock
{
    public class LoginPacket
    {
        public int protocolVersion { get; set; }
        public string request { get; set; }
    }

    public class Login
    {
        public const byte id = 1;
        public static void Decode(byte[] buffer)
        {
            var packet = new LoginPacket
            {
                protocolVersion = DataTypes.ReadIntBE(buffer),
            };

            BedrockPacketProcessor.Login(packet);
        }

        public static void Encode(LoginPacket fields)
        {

        }
    }
}
