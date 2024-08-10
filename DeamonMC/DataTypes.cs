using System;
using System.Text;
using DeamonMC.Network;

namespace DeamonMC
{
    public class IPAddressInfo
    {
        public byte[] IPAddress { get; set; }
        public ushort Port { get; set; }
    }

    public static class DataTypes
    {
        public static bool ReadBool(byte[] buffer)
        {
            byte b = buffer[PacketDecoder.readOffset];
            PacketDecoder.readOffset += 1;
            return b == 1 ? true : false;
        }

        public static void WriteBool(bool value)
        {
            PacketEncoder.byteStream[PacketEncoder.writeOffset] = value == true ? (byte)1 : (byte)0;
            PacketEncoder.writeOffset += 1;
        }

        public static int ReadInt(byte[] buffer)
        {
            int a = BitConverter.ToInt32(buffer, PacketDecoder.readOffset);
            PacketDecoder.readOffset += 4;
            return a;
        }

        public static void WriteInt(int value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            Array.Copy(bytes, 0, PacketEncoder.byteStream, PacketEncoder.writeOffset, 4);
            PacketEncoder.writeOffset += 4;
        }

        public static void WriteFloat(float value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            Array.Copy(bytes, 0, PacketEncoder.byteStream, PacketEncoder.writeOffset, 4);
            PacketEncoder.writeOffset += 4;
        }

        public static int ReadIntBE(byte[] buffer)
        {
            Array.Reverse(buffer, PacketDecoder.readOffset, 4);
            int a = BitConverter.ToInt32(buffer, PacketDecoder.readOffset);
            PacketDecoder.readOffset += 4;
            return a;
        }

        public static int ReadSignedVarInt(byte[] data)
        {

            int rawVarInt = ReadVarInt(data);
            int value = (rawVarInt >> 1) ^ -(rawVarInt & 1);
            return value;
        }

        public static int ReadVarInt(byte[] data)
        {
            int value = 0;
            int size = 0;

            while (true)
            {
                byte currentByte = data[PacketDecoder.readOffset++];
                value |= (currentByte & 0x7F) << (size * 7);

                if ((currentByte & 0x80) == 0)
                {
                    break;
                }

                size++;
            }

            return value;
        }

        public static void WriteVarInt(int value)
        {
            while ((value & -128) != 0)
            {
                PacketEncoder.byteStream[PacketEncoder.writeOffset++] = (byte)((value & 127) | 128);
                value >>= 7;
            }
            PacketEncoder.byteStream[PacketEncoder.writeOffset++] = (byte)(value & 127);
        }

        public static short ReadShort(byte[] buffer)
        {
            short value = (short)((buffer[PacketDecoder.readOffset] << 8) | buffer[PacketDecoder.readOffset + 1]);
            PacketDecoder.readOffset += 2;
            return value;
        }

        public static void WriteShortBE(ushort value)
        {
            PacketEncoder.byteStream[PacketEncoder.writeOffset] = (byte)(value >> 8);
            PacketEncoder.byteStream[PacketEncoder.writeOffset + 1] = (byte)value;
            PacketEncoder.writeOffset += 2;
        }

        public static void WriteShort(ushort value)
        {
            PacketEncoder.byteStream[PacketEncoder.writeOffset] = (byte)value;
            PacketEncoder.byteStream[PacketEncoder.writeOffset + 1] = (byte)(value >> 8);
            PacketEncoder.writeOffset += 2;
        }

        public static byte ReadByte(byte[] buffer)
        {
            byte b = buffer[PacketDecoder.readOffset];
            PacketDecoder.readOffset += 1;
            return b;
        }

        public static void WriteByte(byte value)
        {
            PacketEncoder.byteStream[PacketEncoder.writeOffset] = value;
            PacketEncoder.writeOffset += 1;
        }

        public static long ReadLong(byte[] buffer)
        {
            long value = BitConverter.ToInt64(buffer, PacketDecoder.readOffset);
            PacketDecoder.readOffset += 8;
            return value;
        }

        public static long ReadLongLE(byte[] buffer)
        {
            Array.Reverse(buffer, PacketDecoder.readOffset, 8);
            long value = BitConverter.ToInt64(buffer, PacketDecoder.readOffset);
            PacketDecoder.readOffset += 8;
            return value;
        }

        public static void WriteLongLE(long value)
        {
            byte[] valueBytes = BitConverter.GetBytes(value); 
            Array.Reverse(valueBytes);
            Array.Copy(valueBytes, 0, PacketEncoder.byteStream, PacketEncoder.writeOffset, 8);
            PacketEncoder.writeOffset += 8;
        }

        public static void WriteLong(long value)
        {
            byte[] valueBytes = BitConverter.GetBytes(value);
            Array.Copy(valueBytes, 0, PacketEncoder.byteStream, PacketEncoder.writeOffset, 8);
            PacketEncoder.writeOffset += 8;
        }

        public static string ReadMagic(byte[] buffer)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < 16; ++i)
            {
                sb.Append(buffer[PacketDecoder.readOffset + i].ToString("X2"));
            }
            PacketDecoder.readOffset += 16;
            return sb.ToString();
        }

        public static void WriteMagic(string magic)
        {
            for (int i = 0; i < magic.Length; i += 2)
            {
                string byteString = magic.Substring(i, 2);
                byte b = byte.Parse(byteString, System.Globalization.NumberStyles.HexNumber);
                PacketEncoder.byteStream[PacketEncoder.writeOffset++] = b;
            }
        }

        public static string ReadRakString(byte[] buffer)
        {
            ushort length = BitConverter.ToUInt16(buffer, PacketDecoder.readOffset);
            PacketDecoder.readOffset += 2;
            string str = Encoding.UTF8.GetString(buffer, PacketDecoder.readOffset, length);
            PacketDecoder.readOffset += length;

            return str;
        }

        public static void WriteRakString(string str)
        {
            ushort length = (ushort)str.Length;
            byte[] lengthBytes = BitConverter.GetBytes(length);
            Array.Reverse(lengthBytes);
            Array.Copy(lengthBytes, 0, PacketEncoder.byteStream, PacketEncoder.writeOffset, 2);
            PacketEncoder.writeOffset += 2; 

            byte[] strBytes = Encoding.UTF8.GetBytes(str);
            Array.Copy(strBytes, 0, PacketEncoder.byteStream, PacketEncoder.writeOffset, strBytes.Length);
            PacketEncoder.writeOffset += strBytes.Length;
        }

        public static string ReadString(byte[] buffer)
        {
            int length = ReadVarInt(buffer);
            string str = Encoding.UTF8.GetString(buffer, PacketDecoder.readOffset, length);
            PacketDecoder.readOffset += length;

            return str;
        }

        public static void WriteString(string str)
        {
            byte[] strBytes = Encoding.UTF8.GetBytes(str);
            WriteVarInt(strBytes.Length);
            Array.Copy(strBytes, 0, PacketEncoder.byteStream, PacketEncoder.writeOffset, strBytes.Length);
            PacketEncoder.writeOffset += strBytes.Length;
        }

        public static int ReadMTU(byte[] buffer, int lenght)
        {
            int paddingSize = lenght - PacketDecoder.readOffset;

            int estimatedMTU = PacketDecoder.readOffset + paddingSize + 28;

            PacketDecoder.readOffset = (paddingSize + PacketDecoder.readOffset);

            return estimatedMTU;
        }

        public static IPAddressInfo ReadAddress(byte[] buffer)
        {
            byte ipVersion = buffer[PacketDecoder.readOffset];
            PacketDecoder.readOffset++;

            IPAddressInfo ipAddressInfo = new IPAddressInfo();

            if (ipVersion == 4)
            {
                ipAddressInfo.IPAddress = new byte[4];
                Array.Copy(buffer, PacketDecoder.readOffset, ipAddressInfo.IPAddress, 0, 4);
                ipAddressInfo.Port = BitConverter.ToUInt16(buffer, PacketDecoder.readOffset + 4);
                PacketDecoder.readOffset += 6;
            }
            else if (ipVersion == 6)
            {
                ipAddressInfo.IPAddress = new byte[16];
                Array.Copy(buffer, PacketDecoder.readOffset + 4, ipAddressInfo.IPAddress, 0, 16);
                ipAddressInfo.Port = BitConverter.ToUInt16(buffer, PacketDecoder.readOffset + 2);
                PacketDecoder.readOffset += 32;
            }

            return ipAddressInfo;
        }

        public static void WriteAddress(string ip = "127.0.0.1")
        {
            string[] ipParts = ip.Split('.');
            byte[] ipAddress = new byte[] { byte.Parse(ipParts[0]), byte.Parse(ipParts[1]), byte.Parse(ipParts[2]), byte.Parse(ipParts[3]) };
            ushort port = 19132;

            PacketEncoder.byteStream[PacketEncoder.writeOffset] = 4;
            PacketEncoder.writeOffset++;

            Array.Copy(ipAddress, 0, PacketEncoder.byteStream, PacketEncoder.writeOffset, ipAddress.Length);
            PacketEncoder.writeOffset += ipAddress.Length;

            byte[] portBytes = BitConverter.GetBytes(port);
            Array.Reverse(portBytes);
            Array.Copy(portBytes, 0, PacketEncoder.byteStream, PacketEncoder.writeOffset, portBytes.Length);
            PacketEncoder.writeOffset += portBytes.Length;
        }

        public static IPAddressInfo[] ReadInternalAddress(byte[] buffer, int count)
        {
            IPAddressInfo[] ipAddress = new IPAddressInfo[count];
            for (int i = 0; i < count; ++i)
            {
                byte ipVersion = buffer[PacketDecoder.readOffset];
                PacketDecoder.readOffset++;

                IPAddressInfo ipAddressInfo = new IPAddressInfo();

                if (ipVersion == 4)
                {
                    ipAddressInfo.IPAddress = new byte[4];
                    Array.Copy(buffer, PacketDecoder.readOffset, ipAddressInfo.IPAddress, 0, 4);
                    ipAddressInfo.Port = BitConverter.ToUInt16(buffer, PacketDecoder.readOffset + 4);
                    PacketDecoder.readOffset += 6;
                }
                else if (ipVersion == 6)
                {
                    ipAddressInfo.IPAddress = new byte[16];
                    Array.Copy(buffer, PacketDecoder.readOffset + 4, ipAddressInfo.IPAddress, 0, 16);
                    ipAddressInfo.Port = BitConverter.ToUInt16(buffer, PacketDecoder.readOffset + 2);
                    PacketDecoder.readOffset += 32;
                }
                ipAddress[i] = ipAddressInfo;
            }
            return ipAddress;
        }

        public static uint ReadUInt24LE(byte[] buffer)
        {
            uint uint24leValue = (uint)(buffer[PacketDecoder.readOffset] | (buffer[PacketDecoder.readOffset + 1] << 8) | (buffer[PacketDecoder.readOffset + 2] << 16));
            PacketDecoder.readOffset += 3;
            return uint24leValue;
        }

        public static void WriteUInt24LE(uint value)
        {
            PacketEncoder.byteStream[PacketEncoder.writeOffset] = (byte)(value & 0xFF);
            PacketEncoder.byteStream[PacketEncoder.writeOffset + 1] = (byte)((value >> 8) & 0xFF);
            PacketEncoder.byteStream[PacketEncoder.writeOffset + 2] = (byte)((value >> 16) & 0xFF);
            PacketEncoder.writeOffset += 3;
        }

        public static void HexDump(byte[] buffer, int lenght)
        {
            for (int i = 0; i < lenght; i++)
            {
                Console.Write(buffer[i].ToString("X2") + " ");
                if ((i + 1) % 16 == 0)
                {
                    Console.WriteLine();
                }
            }
            Console.WriteLine();
        }
    }
}
