namespace DaemonMC.Network.RakNet
{
    public class OpenConnectionRequest1Packet
    {
        public string Magic { get; set; }
        public byte Protocol { get; set; }
        public int Mtu { get; set; }
    }

    public class OpenConnectionRequest1
    {
        public static byte id = 5;
        public static void Decode(byte[] buffer, int recv)
        {
            var packet = new OpenConnectionRequest1Packet
            {
                Magic = DataTypes.ReadMagic(buffer),
                Protocol = DataTypes.ReadByte(buffer),
                Mtu = DataTypes.ReadMTU(buffer, recv),
            };

            RakPacketProcessor.OpenConnectionRequest1(packet);
        }

        public static void Encode(OpenConnectionRequest1Packet fields)
        {

        }
    }
}
