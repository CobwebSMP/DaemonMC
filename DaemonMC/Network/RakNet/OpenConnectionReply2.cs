namespace DaemonMC.Network.RakNet
{
    public class OpenConnectionReply2Packet
    {
        public string Magic { get; set; }
        public long GUID { get; set; }
        public IPAddressInfo clientAddress { get; set; }
        public int Mtu { get; set; }
        public bool Encryption { get; set; }
    }

    public class OpenConnectionReply2
    {
        public static byte id = 8;
        public static void Decode(byte[] buffer)
        {

        }

        public static void Encode(OpenConnectionReply2Packet fields)
        {
            DataTypes.WriteByte(id);
            DataTypes.WriteMagic(fields.Magic);
            DataTypes.WriteLongLE(fields.GUID);
            DataTypes.WriteAddress(); //todo
            DataTypes.WriteShortBE((ushort)fields.Mtu);
            DataTypes.WriteByte(0); //todo
            PacketEncoder.SendPacket(id);
        }
    }
}
