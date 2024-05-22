using System.Net;
using System.Reflection.PortableExecutable;

namespace DeamonMC.Network.RakNet
{
    public class OpenConnectionRequest2Packet
    {
        public string Magic { get; set; }
        public IPAddressInfo Address { get; set; }
        public short Mtu { get; set; }
        public long ClientId { get; set; }
    }

    public class OpenConnectionRequest2
    {
        public static byte id = 7;
        public static void Decode(byte[] buffer)
        {
            var packet = new OpenConnectionRequest2Packet
            {
                Magic = DataTypes.ReadMagic(buffer),
                Address = DataTypes.ReadAddress(buffer),
                Mtu = DataTypes.ReadShort(buffer),
                ClientId = DataTypes.ReadLong(buffer)
            };

            RakPacketProcessor.OpenConnectionRequest2(packet);
        }

        public static void Encode(OpenConnectionRequest2Packet fields)
        {

        }
    }
}
