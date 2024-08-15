using DaemonMC.Network;

namespace DaemonMC
{
    public static class ToDataTypes
    {

        public static byte[] GetVarint(int value)
        {
            List<byte> bytes = new List<byte>();
            while ((value & -128) != 0)
            {
                bytes.Add((byte)((value & 127) | 128));
                value >>= 7;
            }
            bytes.Add((byte)(value & 127));
            return bytes.ToArray();
        }
    }
}
