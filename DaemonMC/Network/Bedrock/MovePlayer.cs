using System.Numerics;

namespace DaemonMC.Network.Bedrock
{
    public class MovePlayerPacket
    {
        public long actorRuntimeId { get; set; }
        public Vector3 position { get; set; }
        public Vector2 rotation { get; set; }
        public float YheadRotation { get; set; }
        public byte positionMode { get; set; }
        public bool isOnGround { get; set; }
        public long vehicleRuntimeId { get; set; }
        public long tick { get; set; }
    }

    public class MovePlayer
    {
        public const int id = 19;
        public static void Decode(byte[] buffer)
        {
            var packet = new MovePlayerPacket
            {
                actorRuntimeId = DataTypes.ReadVarLong(buffer),
                position = DataTypes.ReadVec3(buffer),
                rotation = DataTypes.ReadVec2(buffer),
                YheadRotation = DataTypes.ReadFloat(buffer),
                positionMode = DataTypes.ReadByte(buffer),
                isOnGround = DataTypes.ReadBool(buffer),
                vehicleRuntimeId = DataTypes.ReadVarLong(buffer)
            };

            BedrockPacketProcessor.MovePlayer(packet);
        }

        public static void Encode(MovePlayerPacket fields)
        {

        }
    }
}
