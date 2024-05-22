namespace DeamonMC.Network.RakNet
{
    public class Reliability
    {
        public static void ReliabilityHandler(byte[] buffer, int recv)
        {
            uint sequence = DataTypes.ReadUInt24LE(buffer);
            uint reliableIndex = 0;
            uint sequenceIndex = 0;
            uint orderIndex = 0;
            byte orderChannel = 0;

            int compSize = 0;
            short compId = 0;
            int compIndex = 0;

            while (Server.readOffset < recv)
            {
                var flags = DataTypes.ReadByte(buffer);
                var pLength = DataTypes.ReadShort(buffer);

                byte reliabilityType = (byte)((flags & 0b011100000) >> 5);
                bool isFragmented = (flags & 0b00010000) > 0;

                if (reliabilityType == 0)
                {
                    //nothing
                }
                else if (reliabilityType == 1)
                {
                    reliableIndex = DataTypes.ReadUInt24LE(buffer);
                    sequenceIndex = DataTypes.ReadUInt24LE(buffer);
                }
                else if (reliabilityType == 2)
                {
                    reliableIndex = DataTypes.ReadUInt24LE(buffer);
                }
                else if (reliabilityType == 3)
                {
                    reliableIndex = DataTypes.ReadUInt24LE(buffer);
                    sequenceIndex = DataTypes.ReadUInt24LE(buffer);
                    orderChannel = DataTypes.ReadByte(buffer);
                }
                else if (reliabilityType == 4)
                {
                    reliableIndex = DataTypes.ReadUInt24LE(buffer);
                    orderIndex = DataTypes.ReadUInt24LE(buffer);
                    orderChannel = DataTypes.ReadByte(buffer);
                }
                else if (reliabilityType == 5)
                {
                    //nothing
                }
                else if (reliabilityType == 6)
                {
                    reliableIndex = DataTypes.ReadUInt24LE(buffer);
                }
                else if (reliabilityType == 7)
                {
                    reliableIndex = DataTypes.ReadUInt24LE(buffer);
                    orderIndex = DataTypes.ReadUInt24LE(buffer);
                    orderChannel = DataTypes.ReadByte(buffer);
                }

                int lengthInBytes = (pLength + 7) / 8;
                byte[] body = new byte[lengthInBytes];
                Array.Copy(buffer, Server.readOffset, body, 0, lengthInBytes);
                Server.readOffset += lengthInBytes;

                if (isFragmented)
                {
                    compSize = DataTypes.ReadInt(buffer);
                    compId = DataTypes.ReadShort(buffer);
                    compIndex = DataTypes.ReadInt(buffer);
                    Console.WriteLine("FRGMENTED!");
                }
                Server.packetBuffers.Add(body);
                //Console.WriteLine($"[Frame Set Packet] seq: {sequence} f: {flags} pL: {pLength} rtype: {reliabilityType} frag: {isFragmented} relIndx: {reliableIndex} seqIndxL: {sequenceIndex} ordIndx: {orderIndex} ordCh: {orderChannel} compSize: {compSize} compIndx: {compIndex} compId: {compId}");
            }
        }

        public static void ReliabilityHandler(
    byte[] body,
    byte reliabilityType = 2,
    bool isFragmented = false,
    uint reliableIndex = 0,
    uint sequenceIndex = 0,
    uint orderIndex = 0,
    byte orderChannel = 0,
    int compSize = 0,
    ushort compId = 0,
    int compIndex = 0)
        {
            int offset = 0;

            // Calculate flags based on reliabilityType and isFragmented
            byte flags = (byte)((reliabilityType << 5) & 0b01110000);
            if (isFragmented)
            {
                flags |= 0b00010000;
            }

            DataTypes.WriteByte(128);
            DataTypes.WriteUInt24LE(1);
            // Write flags and pLength
            DataTypes.WriteByte(flags);
            DataTypes.WriteShort((ushort)body.Count());

            // Write reliability-specific data
            if (reliabilityType == 0)
            {
                // nothing
            }
            else if (reliabilityType == 1)
            {
                DataTypes.WriteUInt24LE(reliableIndex);
                DataTypes.WriteUInt24LE(sequenceIndex);
            }
            else if (reliabilityType == 2)
            {
                DataTypes.WriteUInt24LE(reliableIndex);
            }
            else if (reliabilityType == 3)
            {
                DataTypes.WriteUInt24LE(reliableIndex);
                DataTypes.WriteUInt24LE(orderIndex);
                DataTypes.WriteByte(orderChannel);
            }
            else if (reliabilityType == 4)
            {
                DataTypes.WriteUInt24LE(reliableIndex);
                DataTypes.WriteUInt24LE(orderIndex);
                DataTypes.WriteByte(orderChannel);
            }
            else if (reliabilityType == 5)
            {
                // nothing
            }
            else if (reliabilityType == 6)
            {
                DataTypes.WriteUInt24LE(reliableIndex);
            }
            else if (reliabilityType == 7)
            {
                DataTypes.WriteUInt24LE(reliableIndex);
                DataTypes.WriteUInt24LE(orderIndex);
                DataTypes.WriteByte(orderChannel);
            }

            Array.Copy(body, 0, Server.byteStream, Server.writeOffset, body.Length);
            Server.writeOffset += body.Length;

            // Write fragmentation data if fragmented
            if (isFragmented)
            {
                DataTypes.WriteInt(compSize);
                DataTypes.WriteShort(compId);
                DataTypes.WriteInt(compIndex);
            }
            Server.SendPacket(128);
        }
    }
}
