namespace DeamonMC.RakNet
{
    public class RakSession
    {
        public long GUID { get; set; }
        public bool initCompression { get; set; }

        public RakSession(long guid, bool compression = false)
        {
            GUID = guid;
            initCompression = compression;
        }
    }
}
