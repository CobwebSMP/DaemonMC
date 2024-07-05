namespace DeamonMC.Network
{
    public class Info
    {
        public enum RakNet
        {
            ConnectedPing = 0,                    //0x00
            UnconnectedPing = 1,                  //0x01
            ConnectedPong = 3,                    //0x03
            OpenConnectionRequest1 = 5,           //0x05
            OpenConnectionReply1 = 6,             //0x06
            OpenConnectionRequest2 = 7,           //0x07
            OpenConnectionReply2 = 8,             //0x08
            ConnectionRequest = 9,                //0x09
            ConnectionRequestAccepted = 16,       //0x10
            NewIncomingConnection = 19,           //0x13
            Disconnect = 21,                      //0x15
            UnconnectedPong = 28,                 //0x1c
            NACK = 160,                           //0xa0
            ACK = 192,                            //0xc0
        }

        public enum Bedrock
        {
            Disconnect = 5,                       //0x05
            NetworkSettings = 143,                //0xc1
            RequestNetworkSettings = 193,         //0x8F
        }
    }
}
