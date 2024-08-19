namespace DaemonMC.Network.RakNet
{
    public class ConnectedPingPacket
    {
        public long Time { get; set; }
    }

    public class ConnectedPing
    {
        public static byte id = 0;
        public static void Decode(byte[] buffer)
        {
            var packet = new ConnectedPingPacket
            {
                Time = DataTypes.ReadLongLE(buffer),
            };

            RakPacketProcessor.ConnectedPing(packet);
        }

        public static void Encode(ConnectedPingPacket fields)
        {
            DataTypes.WriteByte(id);
            DataTypes.WriteLongLE(fields.Time);
            PacketEncoder.handlePacket("raknet");
        }
    }
}
