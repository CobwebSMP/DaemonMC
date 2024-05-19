namespace DeamonMC.RakNet
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
            DataTypes.WriteAddress(); //todo
            DataTypes.WriteShort(0);

            for (int i = 0; i < 40; ++i)
            {
                DataTypes.WriteAddress(); //todo
            }

            DataTypes.WriteLongLE(fields.Time);
            DataTypes.WriteLongLE(DateTimeOffset.UtcNow.ToUnixTimeSeconds());
            Server.handlePacket();
        }
    }
}
