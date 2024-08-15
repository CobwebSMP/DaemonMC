namespace DaemonMC.Utils
{
    public class JwtPayload
    {
        public string ArmSize { get; set; }
        public long ClientRandomId { get; set; }
        public string DeviceId { get; set; }
        public string DeviceModel { get; set; }
        public int DeviceOS { get; set; }
        public string GameVersion { get; set; }
        public int GuiScale { get; set; }
        public bool IsEditorMode { get; set; }
        public string LanguageCode { get; set; }
        public string SelfSignedId { get; set; }
        public string ServerAddress { get; set; }
        public string SkinColor { get; set; }
        public string SkinData { get; set; }
        public string SkinGeometryData { get; set; }
        // ..... todo
    }
}
