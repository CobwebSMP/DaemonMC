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
                    var clientId = DataTypes.ReadLongLE(buffer);

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
                    var mtu = DataTypes.ReadMTU(buffer);

                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine($"[Open Connection Request 1] --magic: {magic}  protocol version: {protocol} mtu: {mtu}");
                    Console.ResetColor();



                    var pkid2 = (byte)6;
                    DataTypes.WriteByte(pkid2);
                    DataTypes.WriteMagic("00ffff00fefefefefdfdfdfd12345678");
                    DataTypes.WriteLongLE(123456);
                    DataTypes.WriteByte(0);
                    DataTypes.WriteShort((ushort)(mtu + 46));
                    SendPacket(pkid2);

                }
                else if (pkid == 7)
                {
                    var magic = DataTypes.ReadMagic(buffer);
                    var adress = DataTypes.ReadAddress(buffer);
                    var mtu = DataTypes.ReadMTU(buffer);
                    var clientId = DataTypes.ReadLongLE(buffer);


                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine($"[Open Connection Request 2] --magic: {magic}  port: {adress.Port} mtu: {mtu} clientId: {clientId}");
                    Console.ResetColor();
                }
                else
                {
                    Console.WriteLine($"[Server] Unknown packet: {pkid}");
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
