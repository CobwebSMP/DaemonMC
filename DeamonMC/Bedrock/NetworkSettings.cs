namespace DeamonMC.Bedrock
{
    public class NetworkSettingsPacket
    {
        public ushort compressionThreshold { get; set; }
        public ushort compressionAlgorithm { get; set; }
        public bool clientThrottleEnabled { get; set; }
        public byte clientThrottleThreshold { get; set; }
        public float clientThrottleScalar { get; set; }
    }

    public class NetworkSettings
    {
        public static byte id = 143;
        public static void Decode(byte[] buffer)
        {

        }

        public static void Encode(NetworkSettingsPacket fields)
        {
            DataTypes.WriteByte(254);
            DataTypes.WriteByte(10);
            DataTypes.WriteVarInt(id);
            DataTypes.WriteShort(fields.compressionThreshold);
            DataTypes.WriteShort(fields.compressionAlgorithm);
            DataTypes.WriteBool(fields.clientThrottleEnabled);
            DataTypes.WriteByte(fields.clientThrottleThreshold);
            DataTypes.WriteFloat(fields.clientThrottleScalar);
            Server.handlePacket("bedrock");
        }
    }
}
