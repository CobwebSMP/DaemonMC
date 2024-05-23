using System.Net.Sockets;
using System.Net;
using DeamonMC.Network;

namespace DeamonMC
{
    public class Client
    {
        private static readonly object lockObject = new object();
        private static bool running = true;
        private static Socket sock;
        private static IPEndPoint iep;

        public static void ClientF()
        {
            sock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            sock.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, 1);
            iep = new IPEndPoint(IPAddress.Parse("255.255.255.255"), 19132);

            Thread sendThread = new Thread(SendData);
            Thread receiveThread = new Thread(ReceiveData);

            sendThread.Start();
            receiveThread.Start();
        }

        private static void SendData()
        {
            while (running)
            {
                DataTypes.WriteByte(0x01);
                DataTypes.WriteLongLE(123456);
                DataTypes.WriteMagic("00ffff00fefefefefdfdfdfd12345678");
                DataTypes.WriteLongLE(123456);

                Console.WriteLine("[Server] --> [Broadcast] pkID: 1");
                byte[] trimmedBuffer = new byte[PacketEncoder.writeOffset];
                Array.Copy(PacketEncoder.byteStream, trimmedBuffer, PacketEncoder.writeOffset);
                sock.SendTo(trimmedBuffer, iep);
                PacketEncoder.writeOffset = 0;
                PacketEncoder.byteStream = new byte[1024];
                Thread.Sleep(1000);
            }
        }

        private static void ReceiveData()
        {
            byte[] receiveBuffer = new byte[1024];

            try
            {
                while (running)
                {
                    int bytesRead = sock.Receive(receiveBuffer);
                    if (bytesRead > 0)
                    {
                        lock (lockObject)
                        {
                            var pkid = DataTypes.ReadByte(receiveBuffer);
                            Console.WriteLine($"[Server] <-- [Broadcast] pkID: {pkid}");
                            if (pkid == 28)
                            {
                                var time = DataTypes.ReadLongLE(receiveBuffer);
                                var serverid = DataTypes.ReadLongLE(receiveBuffer);
                                var magic = DataTypes.ReadMagic(receiveBuffer);
                                var motd = DataTypes.ReadString(receiveBuffer);
                                Console.WriteLine($"--time: {time} serverid: {serverid} magic: {magic} motd: {motd}");
                            }
                            PacketDecoder.readOffset = 0;
                        }
                    }
                }
            }
            catch (SocketException ex)
            {
                Console.WriteLine($"Socket Exception: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
            }
        }

        public static void StopClient()
        {
            running = false;
            sock.Close();
        }
    }
}
