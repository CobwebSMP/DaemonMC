using System;

namespace DeamonMC.Network.RakNet
{
    public class OpenConnectionReply1Packet
    {
        public string Magic { get; set; }
        public long GUID { get; set; }
        public bool Security { get; set; }
        public int Cookie { get; set; }
        public int Mtu { get; set; }
    }

    public class OpenConnectionReply1
    {
        public static byte id = 6;
        public static void Decode(byte[] buffer)
        {

        }

        public static void Encode(OpenConnectionReply1Packet fields)
        {
            DataTypes.WriteByte(id);
            DataTypes.WriteMagic(fields.Magic);
            DataTypes.WriteLongLE(fields.GUID);
            DataTypes.WriteByte(0);
            DataTypes.WriteShort((ushort)fields.Mtu);
            PacketEncoder.SendPacket(id);
        }
    }
}
