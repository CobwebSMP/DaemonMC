using System.Diagnostics;
using DaemonMC.Utils.Text;

namespace DaemonMC.Network.Bedrock
{
    public class DisconnectPacket
    {
        public string message { get; set; }
    }

    public class Disconnect
    {
        public static int id = 5;
        public static void Decode(byte[] buffer)
        {

        }

        public static void Encode(DisconnectPacket fields)
        {
            DataTypes.WriteVarInt(id);
            DataTypes.WriteVarInt(0);
            DataTypes.WriteBool(false);
            DataTypes.WriteString(fields.message);
            DataTypes.WriteString("");
            PacketEncoder.handlePacket();
        }
    }
}
