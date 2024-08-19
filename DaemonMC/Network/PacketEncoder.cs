using DaemonMC.Network.RakNet;
using DaemonMC.Utils.Text;

namespace DaemonMC.Network
{
    public class PacketEncoder
    {
        public static int writeOffset = 0;
        public static uint sequenceNumber = 0;
        public static byte[] byteStream = new byte[1024];

        public static Dictionary<uint, byte[]> sentPackets = new Dictionary<uint, byte[]>();

        public static void handlePacket(string type = "bedrock")
        {
            byte[] trimmedBuffer = new byte[writeOffset];
            Array.Copy(byteStream, trimmedBuffer, writeOffset);
            if (type == "bedrock")
            {
                PacketDecoder.readOffset = 0; Log.debug($"[Server] --> [{Server.clientEp.Address,-16}:{Server.clientEp.Port}] {(Info.Bedrock)DataTypes.ReadVarInt(trimmedBuffer)}");
                byte[] bedrockId = new byte[] { 254 };

                if (RakSessionManager.getSession(Server.clientEp) != null)
                {
                    if (RakSessionManager.getSession(Server.clientEp).initCompression)
                    {
                        bedrockId = new byte[] { 254, 255 };
                    }
                }

                byte[] lengthVarInt = ToDataTypes.GetVarint(writeOffset);

                byte[] header = new byte[bedrockId.Length + lengthVarInt.Length];
                Array.Copy(bedrockId, 0, header, 0, bedrockId.Length);
                Array.Copy(lengthVarInt, 0, header, bedrockId.Length, lengthVarInt.Length);

                byte[] newtrimmedBuffer = new byte[trimmedBuffer.Length + header.Length];
                Array.Copy(header, 0, newtrimmedBuffer, 0, header.Length);
                Array.Copy(trimmedBuffer, 0, newtrimmedBuffer, header.Length, trimmedBuffer.Length);

                writeOffset = 0;
                byteStream = new byte[1024];

                Reliability.ReliabilityHandler(newtrimmedBuffer);
                return;
            }

            Log.debug($"[Server] --> [{Server.clientEp.Address,-16}:{Server.clientEp.Port}] {(Info.RakNet)trimmedBuffer[0]}");

            writeOffset = 0;
            byteStream = new byte[1024];

            if (trimmedBuffer[0] == 3)
            {
                Reliability.ReliabilityHandler(trimmedBuffer, 0, false);
            }
            else
            {
                Reliability.ReliabilityHandler(trimmedBuffer);
            }
        }

        public static void SendPacket(int pkid)
        {
            var clientIp = Server.clientEp.Address.ToString();
            var clientPort = Server.clientEp.Port;
            if (pkid <= 127 || pkid >= 141) { Log.debug($"[Server] --> [{clientIp,-16}:{clientPort}] {(Info.RakNet)pkid}"); };
            byte[] trimmedBuffer = new byte[writeOffset];
            Array.Copy(byteStream, trimmedBuffer, writeOffset);
            Server.Send(trimmedBuffer, Server.clientEp);
            sequenceNumber++;
        }

        //FE FF 0C 05 00 00 00 04 74 65 73 74 00 00 00 00
        //FE - 254 Bedrock packet
        //FF - 255 no compression
        //05 - packet id
    }
}
