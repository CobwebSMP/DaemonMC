namespace DeamonMC.RakNet
{
    public class DisconnectPacket
    {

    }

    public class Disconnect
    {
        public static byte id = 21;
        public static void Decode(byte[] buffer)
        {
            var packet = new DisconnectPacket
            {
            };
            RakPacketProcessor.Disconnect(packet);
        }

        public static void Encode(DisconnectPacket fields)
        {

        }
    }
}
