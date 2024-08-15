namespace DaemonMC.Network.RakNet
{
    public class NACKPacket
    {
        public List<NACKdata> NACKs { get; set; }
    }

    public class NACKdata
    {
        public bool singleSequence { get; set; }
        public uint sequenceNumber { get; set; }
        public uint firstSequenceNumber { get; set; }
        public uint lastSequenceNumber { get; set; }
    }

    public class NACK
    {
        public static byte id = 160;
        public static void Decode(byte[] buffer)
        {
            var NACKs = new List<NACKdata>();
            var count = DataTypes.ReadShort(buffer);
            for (int i = 0; i < count; ++i)
            {
                var NACK = new NACKdata();
                NACK.singleSequence = DataTypes.ReadBool(buffer);
                if (NACK.singleSequence == true)
                {
                    NACK.sequenceNumber = DataTypes.ReadUInt24LE(buffer);
                }
                else
                {
                    NACK.firstSequenceNumber = DataTypes.ReadUInt24LE(buffer);
                    NACK.lastSequenceNumber = DataTypes.ReadUInt24LE(buffer);
                }
                NACKs.Add(NACK);
            }
            var packet = new NACKPacket
            {
                NACKs = NACKs
            };

            RakPacketProcessor.NACK(packet);
        }

        public static void Encode(ACKPacket fields)
        {

        }
    }
}
