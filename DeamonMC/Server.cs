using System.Net.Sockets;
using System.Net;
using DeamonMC.Utils.Text;
using DeamonMC.Network.RakNet;
using DeamonMC.Network.Bedrock;
using DeamonMC.Network;
using static DeamonMC.Network.Info;

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
            Log.info("Server listening on port 19132");

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
                if (pkid <= 127 || pkid >= 141) { Log.debug($"[Server] <-- [{clientIp,-16}:{clientPort}] {(Info.RakNet)pkid}"); }


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
                        Log.error($"[Server] Unknown RakNet packet: {pkid}");
                        DataTypes.HexDump(buffer, recv);
                    }
                }

                if (recv > readOffset)
                {
                    Log.warn($"[Read Warn] Still left {recv - readOffset} bytes");
                }
                else if (recv < readOffset)
                {
                    Log.warn($"[Read Warn] Read too many bytes. Tried to read more {readOffset - recv} bytes");
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
                if (pkid != 254) { Log.debug($"[Server] <-- [{clientEp.Address,-16}:{clientEp.Port}] {(Info.RakNet)pkid}"); }
                if (pkid == ConnectionRequest.id)
                {
                    ConnectionRequest.Decode(buffer);
                }
                else if (pkid == NewIncomingConnection.id)
                {
                    NewIncomingConnection.Decode(buffer);
                }
                else if (pkid == ConnectedPing.id)
                {
                    ConnectedPing.Decode(buffer);
                }
                else if (pkid == Disconnect.id)
                {
                    Disconnect.Decode(buffer);
                }
                else
                {
                    if (pkid == 254)
                    {
                        DataTypes.ReadByte(buffer);
                        var pkid2 = DataTypes.ReadVarInt(buffer);
                        Log.debug($"[Server] <-- [{clientEp.Address,-16}:{clientEp.Port}] {(Info.Bedrock)pkid2}");
                        if (pkid2 == RequestNetworkSettings.id)
                        {
                            RequestNetworkSettings.Decode(buffer);
                        }
                        else
                        {
                            Log.error($"[Server] Unknown Bedrock packet: {pkid}");
                        }
                    }
                    else
                    {
                        Log.error($"[Server] Unknown RakNet packet2: {pkid}");
                    }
                }
            }
            packetBuffers.Clear();
        }

        public static void SendPacket(int pkid)
        {
            var clientIp = clientEp.Address.ToString();
            var clientPort = clientEp.Port;
            if (pkid <= 127 || pkid >= 141) { Log.debug($"[Server] --> [{clientIp,-16}:{clientPort}] {(Info.RakNet)pkid}"); };
            byte[] trimmedBuffer = new byte[writeOffset];
            Array.Copy(byteStream, trimmedBuffer, writeOffset);
            sock.SendTo(trimmedBuffer, clientEp);
            writeOffset = 0;
            byteStream = new byte[1024];
        }

        public static void handlePacket(string type = "")
        {
            byte[] trimmedBuffer = new byte[writeOffset];
            Array.Copy(byteStream, trimmedBuffer, writeOffset);
            if (type == "") { Log.debug($"[Server] --> [{clientEp.Address,-16}:{clientEp.Port}] {(Info.RakNet)trimmedBuffer[0]}"); };
            if (type == "bedrock") { readOffset = 2; Log.debug($"[Server] --> [{clientEp.Address,-16}:{clientEp.Port}] {(Info.Bedrock)DataTypes.ReadVarInt(trimmedBuffer)}"); };
            if (RakSessionManager.getSession(clientEp).initCompression)
            {
                byte[] header = { 255, 254, (byte)writeOffset };
                byte[] newtrimmedBuffer = new byte[trimmedBuffer.Length + header.Length];
                Array.Copy(header, 0, newtrimmedBuffer, 0, header.Length);
                Array.Copy(trimmedBuffer, 0, newtrimmedBuffer, header.Length, trimmedBuffer.Length);
                writeOffset = 0;
                byteStream = new byte[1024];
                Reliability.ReliabilityHandler(newtrimmedBuffer);
                DataTypes.HexDump(newtrimmedBuffer, newtrimmedBuffer.Length);
            }
            writeOffset = 0;
            byteStream = new byte[1024];
            Reliability.ReliabilityHandler(trimmedBuffer);
        }
     }
}
