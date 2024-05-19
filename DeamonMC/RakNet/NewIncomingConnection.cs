namespace DeamonMC.RakNet
{
    public class NewIncomingConnectionPacket
    {
        public IPAddressInfo serverAddress { get; set; }

        public IPAddressInfo[] internalAddress { get; set; }
        public long incommingTime { get; set; }
        public long serverTime { get; set; }
    }

    public class NewIncomingConnection
    {
        public static byte id = 19;
        public static void Decode(byte[] buffer)
        {
            var packet = new NewIncomingConnectionPacket
            {
                serverAddress = DataTypes.ReadAddress(buffer),
                internalAddress = DataTypes.ReadInternalAddress(buffer, 20),
                incommingTime = DataTypes.ReadLong(buffer),
                serverTime = DataTypes.ReadLong(buffer)
            };

            RakPacketProcessor.NewIncomingConnection(packet);
        }

        public static void Encode(NewIncomingConnectionPacket fields)
        {

        }
    }
}
