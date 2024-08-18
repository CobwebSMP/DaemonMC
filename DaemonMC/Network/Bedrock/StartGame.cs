using System.Numerics;

namespace DaemonMC.Network.Bedrock
{
    public class StartGamePacket
    {
        public long EntityId { get; set; }
        public int GameMode { get; set; }
        public Vector3 position { get; set; }
        public Vector2 rotation { get; set; }
        public long seed { get; set; }
        public ushort biomeType { get; set; } = 0;
        public string biomeName { get; set; } = "plains";
        public int dimension { get; set; } = 0;
        public int generator { get; set; }
        public int gameType { get; set; }
        public int difficulty { get; set; }
        public int spawnBlockX { get; set; }
        public int spawnBlockY { get; set; }
        public int spawnBlockZ { get; set; }
        public int editorType { get; set; }
        public int stopTime { get; set; }
    }

    public class StartGame
    {
        public static byte id = 11;
        public static void Decode(byte[] buffer)
        {

        }

        public static void Encode(StartGamePacket fields)
        {
            DataTypes.WriteVarInt(id);
            DataTypes.WriteSignedVarLong(fields.EntityId);
            DataTypes.WriteVarLong((ulong)fields.EntityId);
            DataTypes.WriteVarInt(fields.GameMode);
            DataTypes.WriteVec3(fields.position);
            DataTypes.WriteVec2(fields.rotation);
            //Level settings
            DataTypes.WriteLongLE(fields.seed);
                    //Spawn settings
                    DataTypes.WriteShort(fields.biomeType);
                    DataTypes.WriteString(fields.biomeName);
                    DataTypes.WriteVarInt(fields.dimension);
                    //End of Spawn settings
                DataTypes.WriteSignedVarInt(fields.generator);
                DataTypes.WriteSignedVarInt(fields.gameType);
                DataTypes.WriteBool(false); //hardcore
                DataTypes.WriteSignedVarInt(fields.difficulty);
                DataTypes.WriteSignedVarInt(fields.spawnBlockX);
                DataTypes.WriteVarInt(fields.spawnBlockY);
                DataTypes.WriteSignedVarInt(fields.spawnBlockZ);
                DataTypes.WriteBool(false); //achievements
                DataTypes.WriteBool(false);
                DataTypes.WriteBool(false); //editorCreated
                DataTypes.WriteBool(false); //editorExported
                DataTypes.WriteSignedVarInt(fields.stopTime);
                DataTypes.WriteSignedVarInt(0);
                DataTypes.WriteBool(false);
                DataTypes.WriteString("");
                DataTypes.WriteFloat(0);
                DataTypes.WriteFloat(0);
                DataTypes.WriteBool(true); //platform content
                DataTypes.WriteBool(true); //multiplayer?
                DataTypes.WriteBool(true); //lan?
                DataTypes.WriteVarInt(0); //xbox broadcast settings
                DataTypes.WriteVarInt(0); //platform broadcast settings
                DataTypes.WriteBool(true); //commands?
                DataTypes.WriteBool(false); //texture packs?
                DataTypes.WriteVarInt(0); //game rules
                DataTypes.WriteInt(0); //experiments
                DataTypes.WriteBool(false);
                DataTypes.WriteBool(false); //bonus chest
                DataTypes.WriteBool(false); //map
                DataTypes.WriteByte(0); //permission level
                DataTypes.WriteInt(0); //chunk tick range
                DataTypes.WriteBool(false);
                DataTypes.WriteBool(false);
                DataTypes.WriteBool(false);
                DataTypes.WriteBool(true);
                DataTypes.WriteBool(false);
                DataTypes.WriteBool(false);
                DataTypes.WriteBool(false);
                DataTypes.WriteBool(false);
                DataTypes.WriteBool(false);
                DataTypes.WriteBool(false);
                DataTypes.WriteString(Info.version);
                DataTypes.WriteInt(0);
                DataTypes.WriteInt(0);
                DataTypes.WriteBool(false);
                    DataTypes.WriteString("");
                    DataTypes.WriteString("");
                DataTypes.WriteBool(false);
                DataTypes.WriteBool(true);
                DataTypes.WriteBool(false);
            //End of Level settings
            DataTypes.WriteString("");
            DataTypes.WriteString("");
            DataTypes.WriteString("");
            DataTypes.WriteString("");
            DataTypes.WriteString("");
            DataTypes.WriteString("");
            DataTypes.WriteBool(false); //trial //ok
                //synced movement settings
                DataTypes.WriteSignedVarInt(fields.spawnBlockZ);
                DataTypes.WriteSignedVarInt(fields.spawnBlockZ);
                DataTypes.WriteBool(true);
                //end of synced movement settings
            DataTypes.WriteLong(0);
            DataTypes.WriteSignedVarInt(0);
            DataTypes.WriteVarInt(0); //block
            DataTypes.WriteVarInt(0); //item
            DataTypes.WriteString("");
            DataTypes.WriteBool(true); //new inventory
            DataTypes.WriteString(Info.version);
            DataTypes.WriteCompoundTag(new fNbt.NbtCompound(""));
            DataTypes.WriteLong(0); //blockstate checksum
            var uuid = Guid.NewGuid();
            DataTypes.WriteUUID(uuid);
            DataTypes.WriteBool(true);
            DataTypes.WriteBool(false);
            DataTypes.WriteBool(true);
            PacketEncoder.handlePacket("bedrock");
        }
    }
}
