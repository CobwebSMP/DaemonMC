namespace DeamonMC.RakNet
{
    public class ConnectionRequestPacket
    {
        public long Time { get; set; }
        public long GUID { get; set; }
        public byte Security { get; set; }
    }

    public class ConnectionRequest
    {
        public static byte id = 1;
        public static void Decode(byte[] buffer)
        {
            var packet = new ConnectionRequestPacket
            {
                GUID = DataTypes.ReadLong(buffer),
                Time = DataTypes.ReadLongLE(buffer),
                Security = DataTypes.ReadByte(buffer) //todo
            };

            RakPacketProcessor.ConnectionRequest(packet);
        }

        public static void Encode(ConnectionRequestPacket fields)
        {

        }
    }
}
