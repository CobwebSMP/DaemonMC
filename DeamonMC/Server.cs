using System.Net.Sockets;
using System.Net;
using System.Reflection.PortableExecutable;

namespace DeamonMC
{
    public class Server
    {
        public static int readOffset = 0;
        public static int writeOffset = 0;
        public static byte[] byteStream = new byte[1024];

        public static Socket sock { get; set; }
        public static IPEndPoint clientEp { get; set; }
        public static void ServerF()
        {
            sock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            IPEndPoint iep = new IPEndPoint(IPAddress.Any, 19132);
            sock.Bind(iep);
            Console.WriteLine("Server listening on port 19132");

            while (true)
            {
                EndPoint ep = (EndPoint)iep;

                byte[] buffer = new byte[8192];
                int recv = sock.ReceiveFrom(buffer, ref ep);

                clientEp = (IPEndPoint)ep;
                var clientIp = clientEp.Address.ToString();
                var clientPort = clientEp.Port;

                var pkid = DataTypes.ReadByte(buffer);
                Console.WriteLine($"[Server] <-- [{clientIp}:{clientPort}] pkID: {pkid}");


                if (pkid == 1)
                {
                    var time = DataTypes.ReadLongLE(buffer);
                    var magic = DataTypes.ReadMagic(buffer);
                    var clientId = DataTypes.ReadLong(buffer);

                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine($"[Unconnected Ping] --time: {time}  magic: {magic} clientId: {clientId}");
                    Console.ResetColor();



                    var pkid2 = (byte)28;
                    DataTypes.WriteByte(pkid2);
                    DataTypes.WriteLongLE(time);
                    DataTypes.WriteLongLE(123456);
                    DataTypes.WriteMagic("00ffff00fefefefefdfdfdfd12345678");
                    DataTypes.WriteString($"MCPE;DeamonMC;100;{DeamonMC.version};0;{DeamonMC.maxOnline};12345678912345678912;World;Survival;1;19132;19133;");
                    SendPacket(pkid2);
                }
                else if (pkid == 5)
                {
                    var magic = DataTypes.ReadMagic(buffer);
                    var protocol = DataTypes.ReadByte(buffer);
                    var mtu = DataTypes.ReadMTU(buffer, recv);

                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine($"[Open Connection Request 1] --magic: {magic}  protocol version: {protocol} mtu: {mtu}");
                    Console.ResetColor();



                    var pkid2 = (byte)6;
                    DataTypes.WriteByte(pkid2);
                    DataTypes.WriteMagic("00ffff00fefefefefdfdfdfd12345678");
                    DataTypes.WriteLongLE(123456);
                    DataTypes.WriteByte(0);
                    DataTypes.WriteShort((ushort) mtu);
                    SendPacket(pkid2);

                }
                else if (pkid == 7)
                {
                    var magic = DataTypes.ReadMagic(buffer);
                    var adress = DataTypes.ReadAddress(buffer);
                    var mtu = DataTypes.ReadShort(buffer);
                    var clientId = DataTypes.ReadLong(buffer);


                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine($"[Open Connection Request 2] --magic: {magic} adress {new IPAddress(adress.IPAddress).ToString()} port: {adress.Port} mtu: {mtu} clientId: {clientId}");
                    Console.ResetColor();



                    var pkid2 = (byte)8;
                    DataTypes.WriteByte(pkid2);
                    DataTypes.WriteMagic("00ffff00fefefefefdfdfdfd12345678");
                    DataTypes.WriteAddress();
                    DataTypes.WriteShort((ushort) mtu);
                    DataTypes.WriteByte(0);
                    SendPacket(pkid2);
                }
                else
                {
                    if (pkid >= 128 && pkid <= 141) // Frame Set Packet
                    {
                        readOffset = 0;
                        uint sequence = DataTypes.ReadUInt24LE(buffer);
                        uint reliableIndex = 0;
                        uint sequenceIndex = 0;
                        uint orderIndex = 0;
                        byte orderChannel = 0;

                        while (readOffset < recv)
                        {
                            var flags = DataTypes.ReadByte(buffer);
                            var pLength = DataTypes.ReadShort(buffer);

                            byte reliabilityType = (byte)(flags & 0b00000111);
                            bool isFragmented = ((flags >> 3) & 0b00000001) == 1;

                            if (reliabilityType == 0)
                            {

                            }
                            else if (reliabilityType == 1)
                            {
                                reliableIndex = DataTypes.ReadUInt24LE(buffer);
                                sequenceIndex = DataTypes.ReadUInt24LE(buffer);
                            }
                            else if (reliabilityType == 2)
                            {

                            }
                            else if (reliabilityType == 3)
                            {
                                reliableIndex = DataTypes.ReadUInt24LE(buffer);
                                orderIndex = DataTypes.ReadUInt24LE(buffer);
                                orderChannel = DataTypes.ReadByte(buffer);
                            }
                            else if (reliabilityType == 4)
                            {

                            }
                            else if (reliabilityType == 5)
                            {

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
                            Array.Copy(buffer, readOffset, body, 0, lengthInBytes);
                            readOffset += lengthInBytes;


                            Console.WriteLine($"[Frame Set Packet] seq: {sequence} f: {flags} pL: {pLength} rtype: {reliabilityType} frag: {isFragmented} relIndx: {reliableIndex} seqIndxL: {sequenceIndex} ordIndx: {orderIndex} ordCh: {orderChannel}");
                            DataTypes.HexDump(body, body.Length);
                        }

                    }
                    else
                    {
                        Console.WriteLine($"[Server] Unknown packet: {pkid}");
                        DataTypes.HexDump(buffer, recv);
                    }
                }

                if (recv > readOffset)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"[Read Warn] Still left {recv - readOffset} bytes");
                    Console.ResetColor();
                }
                else if (recv < readOffset)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"[Read Warn] Read too many bytes. Tryed to read more {readOffset - recv} bytes");
                    Console.ResetColor();
                }
                readOffset = 0;

            }
        }

        public static void SendPacket(int pkid)
        {
            var clientIp = clientEp.Address.ToString();
            var clientPort = clientEp.Port;
            Console.WriteLine($"[Server] --> [{clientIp}:{clientPort}] pkID: {pkid}");
            byte[] trimmedBuffer = new byte[writeOffset];
            Array.Copy(byteStream, trimmedBuffer, writeOffset);
            sock.SendTo(trimmedBuffer, clientEp);
            writeOffset = 0;
            byteStream = new byte[1024];
        }
    }
}
