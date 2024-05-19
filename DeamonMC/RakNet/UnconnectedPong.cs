namespace DeamonMC.RakNet
{
    public class UnconnectedPongPacket
    {
        public long Time { get; set; }
        public long GUID { get; set; }
        public string Magic { get; set; }
        public string MOTD { get; set; }
    }

    public class UnconnectedPong
    {
        public static byte id = 28;
        public static void Decode(byte[] buffer)
        {

        }

        public static void Encode(UnconnectedPongPacket fields)
        {
            DataTypes.WriteByte(id);
            DataTypes.WriteLongLE(fields.Time);
            DataTypes.WriteLongLE(fields.GUID);
            DataTypes.WriteMagic(fields.Magic);
            DataTypes.WriteString(fields.MOTD);
            Server.SendPacket(id);
        }
    }
}
