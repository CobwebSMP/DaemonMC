namespace DeamonMC.RakNet
{
    public class ConnectionRequestAcceptedPacket
    {
        public long Time { get; set; }
    }

    public class ConnectionRequestAccepted
    {
        public static byte id = 10;
        public static void Decode(byte[] buffer)
        {

        }

        public static void Encode(ConnectionRequestAcceptedPacket fields)
        {
            DataTypes.WriteByte(id);
            DataTypes.WriteAddress(); //todo
            DataTypes.WriteShort(0);

            DataTypes.WriteAddress(); //todo
            DataTypes.WriteAddress(); //todo
            DataTypes.WriteAddress(); //todo
            DataTypes.WriteAddress(); //todo
            DataTypes.WriteAddress(); //todo
            DataTypes.WriteAddress(); //todo
            DataTypes.WriteAddress(); //todo
            DataTypes.WriteAddress(); //todo
            DataTypes.WriteAddress(); //todo
            DataTypes.WriteAddress(); //todo

            DataTypes.WriteLongLE(fields.Time);
            DataTypes.WriteLongLE(fields.Time);

            Server.handlePacket();
        }
    }
}
