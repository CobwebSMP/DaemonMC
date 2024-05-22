namespace DeamonMC.Network.RakNet
{
    public class ACKPacket
    {
        public List<ACKdata> ACKs { get; set; }
    }

    public class ACKdata
    {
        public bool singleSequence { get; set; }
        public uint sequenceNumber { get; set; }
        public uint firstSequenceNumber { get; set; }
        public uint lastSequenceNumber { get; set; }
    }

    public class ACK
    {
        public static byte id = 192;
        public static void Decode(byte[] buffer)
        {
            var ACKs = new List<ACKdata>();
            var count = DataTypes.ReadShort(buffer);
            for (int i = 0; i < count; ++i)
            {
                var ACK = new ACKdata();
                ACK.singleSequence = DataTypes.ReadBool(buffer);
                if (ACK.singleSequence == true)
                {
                    ACK.sequenceNumber = DataTypes.ReadUInt24LE(buffer);
                }
                else
                {
                    ACK.firstSequenceNumber = DataTypes.ReadUInt24LE(buffer);
                    ACK.lastSequenceNumber = DataTypes.ReadUInt24LE(buffer);
                }
                ACKs.Add(ACK);
            }
            var packet = new ACKPacket
            {
                ACKs = ACKs
            };

            RakPacketProcessor.ACK(packet);
        }

        public static void Encode(ACKPacket fields)
        {

        }
    }
}
