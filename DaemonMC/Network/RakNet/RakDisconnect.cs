namespace DaemonMC.Network.RakNet
{
    public class RakDisconnectPacket
    {

    }

    public class RakDisconnect
    {
        public static byte id = 21;
        public static void Decode(byte[] buffer)
        {
            var packet = new RakDisconnectPacket
            {
            };
            RakPacketProcessor.Disconnect(packet);
        }

        public static void Encode(RakDisconnectPacket fields)
        {
            DataTypes.WriteByte(id);
            PacketEncoder.handlePacket("raknet");
        }
    }
}
