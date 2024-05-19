using System.Net.Sockets;
using System.Net;
using DeamonMC.RakNet;

namespace DeamonMC
{
    public class Server
    {
        public static int readOffset = 0;
        public static int writeOffset = 0;
        public static byte[] byteStream = new byte[1024];
        public static List<byte[]> packetBuffers = new List<byte[]>();

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
                readOffset = 0;

                clientEp = (IPEndPoint)ep;
                var clientIp = clientEp.Address.ToString();
                var clientPort = clientEp.Port;

                var pkid = DataTypes.ReadByte(buffer);
                Console.WriteLine($"[Server] <-- [{clientIp}:{clientPort}] pkID: {pkid}");


                if (pkid == UnconnectedPing.id)
                {
                    UnconnectedPing.Decode(buffer);
                }
                else if (pkid == OpenConnectionRequest1.id)
                {
                    OpenConnectionRequest1.Decode(buffer, recv);
                }
                else if (pkid == OpenConnectionRequest2.id)
                {
                    OpenConnectionRequest2.Decode(buffer);
                }
                else if (pkid == ACK.id)
                {
                    ACK.Decode(buffer);
                }
                else if (pkid == NACK.id)
                {
                    NACK.Decode(buffer);
                }
                else
                {
                    if (pkid >= 128 && pkid <= 141) // Frame Set Packet
                    {
                        Reliability.ReliabilityHandler(buffer, recv);
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
                    Console.WriteLine($"[Read Warn] Read too many bytes. Tried to read more {readOffset - recv} bytes");
                    Console.ResetColor();
                }
                readOffset = 0;
                packetHandler();

            }
        }

        public static void packetHandler()
        {
            foreach (byte[] buffer in packetBuffers)
            {
                readOffset = 0;
                var pkid = DataTypes.ReadByte(buffer);
                Console.WriteLine($"[Server] <-- [previous client's 132] pkID: {pkid}");
                if (pkid == 9)
                {
                    ConnectionRequest.Decode(buffer);
                }
                else
                {
                    Console.WriteLine($"[Server] Unknown packet: {pkid}");
                }
            }
            packetBuffers.Clear();
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

        public static void handlePacket()
        {
            byte[] trimmedBuffer = new byte[writeOffset];
            Array.Copy(byteStream, trimmedBuffer, writeOffset);
            writeOffset = 0;
            byteStream = new byte[1024];
            Reliability.ReliabilityHandler(trimmedBuffer);
        }
    }
}
