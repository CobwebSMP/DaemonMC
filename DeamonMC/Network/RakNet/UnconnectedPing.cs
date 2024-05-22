namespace DeamonMC.Network.RakNet
{
    public class UnconnectedPingPacket
    {
        public long Time { get; set; }
        public string Magic { get; set; }
        public long ClientId { get; set; }
    }

    public class UnconnectedPing
    {
        public static byte id = 1;
        public static void Decode(byte[] buffer)
        {
            var packet = new UnconnectedPingPacket
            {
                Time = DataTypes.ReadLongLE(buffer),
                Magic = DataTypes.ReadMagic(buffer),
                ClientId = DataTypes.ReadLong(buffer)
            };

            RakPacketProcessor.UnconnectedPing(packet);
        }

        public static void Encode(UnconnectedPingPacket fields)
        {

        }
    }
}
