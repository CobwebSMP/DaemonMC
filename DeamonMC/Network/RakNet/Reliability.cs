using System.Net.Sockets;
using DeamonMC.Utils.Text;

namespace DeamonMC.Network.RakNet
{
    public class FragmentedPacket
    {
        public int TotalSize;
        public int ReceivedSize;
        public byte[][] Fragments;

        public FragmentedPacket(int totalSize, int fragmentCount)
        {
            TotalSize = totalSize;
            ReceivedSize = 0;
            Fragments = new byte[fragmentCount][];
        }
    }

    public class Reliability
    {
        public static uint reliableIndex = 0;
        public static Dictionary<short, FragmentedPacket> fragmentedPackets = new Dictionary<short, FragmentedPacket>();

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

            while (PacketDecoder.readOffset < recv)
            {
                var flags = DataTypes.ReadByte(buffer);
                var pLength = DataTypes.ReadShortBE(buffer);

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

                if (isFragmented)
                {
                    compSize = DataTypes.ReadIntBE(buffer);
                    compId = DataTypes.ReadShortBE(buffer);
                    compIndex = DataTypes.ReadIntBE(buffer);
                }

                int lengthInBytes = (pLength + 7) / 8;
                byte[] body = new byte[lengthInBytes];
                Array.Copy(buffer, PacketDecoder.readOffset, body, 0, lengthInBytes);
                PacketDecoder.readOffset += lengthInBytes;

                if (isFragmented)
                {
                    if (!fragmentedPackets.ContainsKey(compId))
                    {
                        fragmentedPackets[compId] = new FragmentedPacket(compSize, lengthInBytes);
                    }

                    var fragment = fragmentedPackets[compId];
                    fragment.Fragments[compIndex] = body;
                    fragment.ReceivedSize += body.Length;

                    if (compSize == compIndex+1)
                    {
                        byte[] fullPacket = ReassemblePacket(fragment);
                        ProcessReassembledPacket(fullPacket);
                        fragmentedPackets.Remove(compId);
                    }
                }
                else
                {
                    PacketDecoder.packetBuffers.Add(body);
                }
                //Console.WriteLine($"[Frame Set Packet] seq: {sequence} f: {flags} pL: {pLength} rtype: {reliabilityType} frag: {isFragmented} relIndx: {reliableIndex} seqIndxL: {sequenceIndex} ordIndx: {orderIndex} ordCh: {orderChannel} compSize: {compSize} compIndx: {compIndex} compId: {compId}");

                var ack = new ACKdata { sequenceNumber = sequence };

                var acks = new List<ACKdata>();

                acks.Add(ack);

                var pk = new ACKPacket
                {
                    ACKs = acks,
                };
                ACK.Encode(pk);
            }
        }

        private static byte[] ReassemblePacket(FragmentedPacket fragment)
        {
            byte[] fullPacket = new byte[600000]; //todo better to know size of the packet
            int offset = 0;

            foreach (var part in fragment.Fragments)
            {
                if (part != null)
                {
                    Array.Copy(part, 0, fullPacket, offset, part.Length);
                    offset += part.Length;
                }
            }

            return fullPacket;
        }

        private static void ProcessReassembledPacket(byte[] packet)
        {
            PacketDecoder.packetBuffers.Add(packet);
        }

        public static void ReliabilityHandler(
    byte[] body,
    byte reliabilityType = 2,
    bool isFragmented = false,
    uint sequenceIndex = 0,
    uint orderIndex = 0,
    byte orderChannel = 0,
    int compSize = 0,
    ushort compId = 0,
    int compIndex = 0)
        {
            byte flags = (byte)((reliabilityType << 5) & 0b01110000);
            if (isFragmented)
            {
                flags |= 0b00010000;
            }
            else
            {
                flags |= 0x00;
            }

            DataTypes.WriteByte(128);
            DataTypes.WriteUInt24LE(PacketEncoder.sequenceNumber);
            DataTypes.WriteByte(flags);
            DataTypes.WriteShortBE((ushort)(body.Count() * 8));

            if (reliabilityType == 0) // Unreliable
            {
                // nothing
            }
            else if (reliabilityType == 1) // Unreliable Sequenced
            {
                DataTypes.WriteUInt24LE(reliableIndex);
                DataTypes.WriteUInt24LE(sequenceIndex);
            }
            else if (reliabilityType == 2) // Reliable
            {
                DataTypes.WriteUInt24LE(reliableIndex);
                reliableIndex++;
            }
            else if (reliabilityType == 3) // Ordered
            {
                DataTypes.WriteUInt24LE(reliableIndex);
                DataTypes.WriteUInt24LE(orderIndex);
                DataTypes.WriteByte(orderChannel);
            }
            else if (reliabilityType == 4) // Reliable Ordered
            {
                DataTypes.WriteUInt24LE(reliableIndex);
                DataTypes.WriteUInt24LE(orderIndex);
                DataTypes.WriteByte(orderChannel);
            }
            else if (reliabilityType == 5) // Reliable Sequenced
            {
                // nothing
            }
            else if (reliabilityType == 6) // Unreliable, ACK
            {
                DataTypes.WriteUInt24LE(reliableIndex);
            }
            else if (reliabilityType == 7) // Reliable, ACK
            {
                DataTypes.WriteUInt24LE(reliableIndex);
                DataTypes.WriteUInt24LE(orderIndex);
                DataTypes.WriteByte(orderChannel);
            }

            Array.Copy(body, 0, PacketEncoder.byteStream, PacketEncoder.writeOffset, body.Length);
            PacketEncoder.writeOffset += body.Length;

            if (isFragmented)
            {
                DataTypes.WriteInt(compSize);
                DataTypes.WriteShort(compId);
                DataTypes.WriteInt(compIndex);
            }
            PacketEncoder.SendPacket(128);
        }
    }
}
