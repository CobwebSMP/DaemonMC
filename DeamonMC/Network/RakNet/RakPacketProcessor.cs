using DeamonMC.Utils.Text;

namespace DeamonMC.Network.RakNet
{
    public class RakPacketProcessor
    {
        public static void UnconnectedPing(UnconnectedPingPacket packet)
        {
            var pk = new UnconnectedPongPacket
            {
                Time = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                GUID = 1234567890123456789,
                Magic = "00ffff00fefefefefdfdfdfd12345678",
                MOTD = $"MCPE;DeamonMC;100;{DeamonMC.version};0;{DeamonMC.maxOnline};12345678912345678912;World;Survival;1;19132;19133;"
            };
            UnconnectedPong.Encode(pk);
        }

        public static void OpenConnectionRequest1(OpenConnectionRequest1Packet packet)
        {
            var pk = new OpenConnectionReply1Packet
            {
                Magic = "00ffff00fefefefefdfdfdfd12345678",
                GUID = 1234567890123456789,
                Mtu = packet.Mtu
            };
            OpenConnectionReply1.Encode(pk);
        }

        public static void OpenConnectionRequest2(OpenConnectionRequest2Packet packet)
        {
            var pk = new OpenConnectionReply2Packet
            {
                Magic = "00ffff00fefefefefdfdfdfd12345678",
                GUID = 1234567890123456789,
                Mtu = packet.Mtu
            };
            OpenConnectionReply2.Encode(pk);
        }

        public static void ConnectionRequest(ConnectionRequestPacket packet)
        {
            var pk = new ConnectionRequestAcceptedPacket
            {
                Time = packet.Time,
            };
            RakSessionManager.addSession(Server.clientEp, packet.GUID);
            //Console.WriteLine($"[Connection Request] --clientId: {packet.GUID}  time: {packet.Time} security: {packet.Security}");
            ConnectionRequestAccepted.Encode(pk);
        }

        public static void ACK(ACKPacket packet)
        {
            foreach (var ack in packet.ACKs)
            {
                //Console.WriteLine($"ACK: {ack.singleSequence} / {ack.sequenceNumber} / {ack.firstSequenceNumber} / {ack.lastSequenceNumber}");
            }
        }

        public static void NACK(NACKPacket packet)
        {
            foreach (var nack in packet.NACKs)
            {
                //Console.WriteLine($"NACK: {nack.singleSequence} / {nack.sequenceNumber} / {nack.firstSequenceNumber} / {nack.lastSequenceNumber}");
            }
        }

        public static void NewIncomingConnection(NewIncomingConnectionPacket packet)
        {
            //Log.warn($"NewIncomingConnectionPacket: {packet.serverAddress.IPAddress[0]}.{packet.serverAddress.IPAddress[1]}.{packet.serverAddress.IPAddress[2]}.{packet.serverAddress.IPAddress[3]}:{packet.serverAddress.Port} / {packet.incommingTime} / {packet.serverTime} / {packet.internalAddress.Count()}");
        }

        public static void ConnectedPing(ConnectedPingPacket packet)
        {
            var pk = new ConnectedPongPacket
            {
                pingTime = packet.Time,
                pongTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
            };
            ConnectedPong.Encode(pk);
        }

        public static void Disconnect(RakDisconnectPacket packet)
        {
            RakSessionManager.deleteSession(Server.clientEp);
        }
    }
}
