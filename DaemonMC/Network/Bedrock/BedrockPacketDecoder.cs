using DaemonMC.Network.RakNet;
using DaemonMC.Utils.Text;
using Org.BouncyCastle.Crypto;

namespace DaemonMC.Network.Bedrock
{
    public class BedrockPacketDecoder
    {
        public static void BedrockDecoder(byte[] buffer)
        {
            if (RakSessionManager.getSession(Server.clientEp) != null)
            {
                if (RakSessionManager.getSession(Server.clientEp).decryptor != null)
                {
                    ReadOnlyMemory<byte> payload = buffer.AsMemory();
                    DataTypes.HexDump(buffer, buffer.Length);
                    buffer = buffer.Skip(1).ToArray();
                    DataTypes.HexDump(buffer, buffer.Length);
                    buffer = Decrypt(payload, RakSessionManager.getSession(Server.clientEp).decryptor).ToArray();
                    DataTypes.HexDump(buffer, buffer.Length);
                }
                if (RakSessionManager.getSession(Server.clientEp).initCompression)
                {
                    DataTypes.ReadByte(buffer);
                }
            }
            DataTypes.ReadVarInt(buffer); //packet size
            var pkid = DataTypes.ReadVarInt(buffer);
            Log.debug($"[Server] <-- [{Server.clientEp.Address,-16}:{Server.clientEp.Port}] {(Info.Bedrock)pkid}");

            switch (pkid)
            {
                case RequestNetworkSettings.id:
                    RequestNetworkSettings.Decode(buffer);
                    break;
                case Login.id:
                    Login.Decode(buffer);
                    break;
                case PacketViolationWarning.id:
                    PacketViolationWarning.Decode(buffer);
                    break;
                case ClientCacheStatus.id:
                    ClientCacheStatus.Decode(buffer);
                    break;
                case ResourcePackClientResponse.id:
                    ResourcePackClientResponse.Decode(buffer);
                    break;
                case RequestChunkRadius.id:
                    RequestChunkRadius.Decode(buffer);
                    break;

                default:
                    Log.error($"[Server] Unknown Bedrock packet: {pkid}");
                    DataTypes.HexDump(buffer, buffer.Length);
                    break;
            }
        }

        public static ReadOnlyMemory<byte> Decrypt(ReadOnlyMemory<byte> payload, IBufferedCipher decryptor)
        {
            byte[] decrypted = decryptor.DoFinal(payload.ToArray());
            if (decryptor == null)
            {
                Log.error("Decryptor is not set up.");
            }
            if (decrypted == null)
            {
                Log.error("Decryption failed. Decrypted == null.");
            }
            Log.info(decrypted.Length.ToString());
            return decrypted.AsMemory().Slice(0, decrypted.Length - 8);
        }
    }
}
