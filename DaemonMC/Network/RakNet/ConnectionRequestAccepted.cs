namespace DaemonMC.Network.RakNet
{
    public class ConnectionRequestAcceptedPacket
    {
        public long Time { get; set; }
    }

    public class ConnectionRequestAccepted
    {
        public static byte id = 16;
        public static void Decode(byte[] buffer)
        {

        }

        public static void Encode(ConnectionRequestAcceptedPacket fields)
        {
            DataTypes.WriteByte(id);
            DataTypes.WriteAddress();
            DataTypes.WriteShort(0);

            for (int i = 0; i < 20; ++i)
            {
                DataTypes.WriteAddress();
            }

            DataTypes.WriteLongLE(fields.Time);
            DataTypes.WriteLongLE(DateTimeOffset.UtcNow.ToUnixTimeSeconds());
            PacketEncoder.handlePacket("raknet");
        }
    }
}
