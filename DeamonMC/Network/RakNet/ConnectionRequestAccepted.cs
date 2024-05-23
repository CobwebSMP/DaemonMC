namespace DeamonMC.Network.RakNet
{
    public class ConnectionRequestAcceptedPacket
    {
        public long Time { get; set; }
    }

    public class ConnectionRequestAccepted
    {
        public static byte id = 16;
        public static void Decode(byte[] buffer)
        {
            DataTypes.ReadByte(buffer);
            DataTypes.ReadAddress(buffer);
            DataTypes.ReadShort(buffer);
            DataTypes.ReadInternalAddress(buffer, 20); //todo check
            var packet = new ConnectionRequestAcceptedPacket
            {
                Time = DataTypes.ReadLongLE(buffer),
            };
            DataTypes.ReadLongLE(buffer);

            RakClientPacketProcessor.ConnectionRequestAccepted(packet);
        }

        public static void Encode(ConnectionRequestAcceptedPacket fields)
        {
            DataTypes.WriteByte(id);
            DataTypes.WriteAddress(); //todo
            DataTypes.WriteShort(0);

            for (int i = 0; i < 40; ++i)
            {
                DataTypes.WriteAddress(); //todo
            }

            DataTypes.WriteLongLE(fields.Time);
            DataTypes.WriteLongLE(DateTimeOffset.UtcNow.ToUnixTimeSeconds());
            PacketEncoder.handlePacket();
        }
    }
}
