using System.Diagnostics;
using DeamonMC.Utils.Text;

namespace DeamonMC.Network.Bedrock
{
    public class DisconnectPacket
    {

    }

    public class Disconnect
    {
        public static byte id = 5;
        public static void Decode(byte[] buffer)
        {

        }

        public static void Encode(DisconnectPacket fields)
        {
            DataTypes.WriteVarInt(id);
            DataTypes.WriteVarInt(0);
            DataTypes.WriteBool(false);
            DataTypes.WriteString("test");
            PacketEncoder.handlePacket("bedrock");
        }
    }
}
