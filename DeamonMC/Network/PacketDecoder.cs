using DeamonMC.Network.Bedrock;
using DeamonMC.Network.RakNet;
using DeamonMC.Utils.Text;

namespace DeamonMC.Network
{
    public class PacketDecoder
    {
        public static List<byte[]> packetBuffers = new List<byte[]>();
        public static int readOffset = 0;

        public static void RakDecoder(byte[] buffer, int recv)
        {
            readOffset = 0;

            var clientIp = Server.clientEp.Address.ToString();
            var clientPort = Server.clientEp.Port;

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

        public static void packetHandler()
        {
            foreach (byte[] buffer in packetBuffers)
            {
                readOffset = 0;
                var pkid = DataTypes.ReadByte(buffer);
                if (pkid != 254) { Log.debug($"[Server] <-- [{Server.clientEp.Address,-16}:{Server.clientEp.Port}] {(Info.RakNet)pkid}"); }
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
                else if (pkid == RakDisconnect.id)
                {
                    RakDisconnect.Decode(buffer);
                }
                else
                {
                    if (pkid == 254)
                    {
                        BedrockPacketDecoder.BedrockDecoder(buffer);
                    }
                    else
                    {
                        Log.error($"[Server] Unknown RakNet packet2: {pkid}");
                    }
                }
            }
            packetBuffers.Clear();
        }
    }
}
