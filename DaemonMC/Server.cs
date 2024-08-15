using System.Net.Sockets;
using System.Net;
using DaemonMC.Utils.Text;
using DaemonMC.Network;

namespace DaemonMC
{
    public class Server
    {
        public static Socket sock { get; set; } = null!;
        public static IPEndPoint clientEp { get; set; } = null!;
        public static void ServerF()
        {
            sock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            IPEndPoint iep = new IPEndPoint(IPAddress.Any, 19132);
            sock.Bind(iep);
            if (Log.debugMode) { Log.warn("Decreased performance expected due to enabled debug mode (DaemonMC.yaml: debug)"); }
            Log.info("Server listening on port 19132");

            while (true)
            {
                EndPoint ep = iep;

                byte[] buffer = new byte[8192];
                int recv = sock.ReceiveFrom(buffer, ref ep);

                if (recv != 0)
                {
                    clientEp = (IPEndPoint)ep;
                    PacketDecoder.RakDecoder(buffer, recv);
                }
            }
        }

        public static void Send(byte[] trimmedBuffer, IPEndPoint client)
        {
            //DataTypes.HexDump(trimmedBuffer, trimmedBuffer.Length);
            sock.SendTo(trimmedBuffer, client);
            PacketEncoder.writeOffset = 0;
            PacketEncoder.byteStream = new byte[1024];
        }

    }
}
