namespace DaemonMC.Network.RakNet
{
    public class ConnectedPongPacket
    {
        public long pingTime { get; set; }
        public long pongTime { get; set; }
    }

    public class ConnectedPong
    {
        public static byte id = 3;
        public static void Decode(byte[] buffer)
        {
            var packet = new ConnectedPongPacket
            {
                pingTime = DataTypes.ReadLongLE(buffer),
                pongTime = DataTypes.ReadLongLE(buffer),
            };
            RakClientPacketProcessor.ConnectedPong(packet);
        }

        public static void Encode(ConnectedPongPacket fields)
        {
            DataTypes.WriteByte(id);
            DataTypes.WriteLongLE(fields.pingTime);
            DataTypes.WriteLongLE(fields.pongTime);
            PacketEncoder.handlePacket();
        }
    }
}
