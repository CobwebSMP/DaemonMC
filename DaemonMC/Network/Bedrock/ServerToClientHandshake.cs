using DaemonMC.Utils;
namespace DaemonMC.Network.Bedrock
{
    public class ServerToClientHandshakePacket
    {
        public string JWT { get; set; }
    }

    public class ServerToClientHandshake
    {
        public static int id = 3;
        public static void Decode(byte[] buffer)
        {

        }

        public static void Encode(ServerToClientHandshakePacket fields)
        {
            DataTypes.WriteVarInt(id);
            DataTypes.WriteString(fields.JWT);
            PacketEncoder.handlePacket();
        }
    }
}
