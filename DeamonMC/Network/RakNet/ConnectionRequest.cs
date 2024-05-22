namespace DeamonMC.Network.RakNet
{
    public class ConnectionRequestPacket
    {
        public long Time { get; set; }
        public long GUID { get; set; }
        public byte Security { get; set; }
    }

    public class ConnectionRequest
    {
        public static byte id = 9;
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
            DataTypes.WriteByte(id);
            DataTypes.WriteLong(fields.GUID);
            DataTypes.WriteLongLE(fields.Time);
            DataTypes.WriteByte(fields.Security);
            Server.handlePacket();
        }
    }
}
