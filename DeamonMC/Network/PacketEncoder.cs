using DeamonMC.Network.RakNet;
using DeamonMC.Utils.Text;

namespace DeamonMC.Network
{
    public class PacketEncoder
    {
        public static int writeOffset = 0;
        public static byte[] byteStream = new byte[1024];

        public static void handlePacket(string type = "")
        {
            byte[] trimmedBuffer = new byte[writeOffset];
            Array.Copy(byteStream, trimmedBuffer, writeOffset);
            if (type == "") { Log.debug($"[Server] --> [{Server.clientEp.Address,-16}:{Server.clientEp.Port}] {(Info.RakNet)trimmedBuffer[0]}"); };
            if (type == "bedrock") { PacketDecoder.readOffset = 2; Log.debug($"[Server] --> [{Server.clientEp.Address,-16}:{Server.clientEp.Port}] {(Info.Bedrock)DataTypes.ReadVarInt(trimmedBuffer)}"); };
            if (RakSessionManager.getSession(Server.clientEp).initCompression)
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

        public static void SendPacket(int pkid)
        {
            var clientIp = Server.clientEp.Address.ToString();
            var clientPort = Server.clientEp.Port;
            if (pkid <= 127 || pkid >= 141) { Log.debug($"[Server] --> [{clientIp,-16}:{clientPort}] {(Info.RakNet)pkid}"); };
            byte[] trimmedBuffer = new byte[writeOffset];
            Array.Copy(byteStream, trimmedBuffer, writeOffset);
            Server.Send(trimmedBuffer, Server.clientEp);
        }
    }
}
