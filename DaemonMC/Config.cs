using DaemonMC.Utils.Text;
using YamlDotNet.Serialization.NamingConventions;
using YamlDotNet.Serialization;

namespace DaemonMC
{
    public class Config
    {
        public string serverName { get; set; }
        public string maxOnline { get; set; }
        public bool debug { get; set; }

        public static void Set()
        {
            string configFile = "DaemonMC.yaml";
            string yamlContent = File.ReadAllText(configFile);

            var deserializer = new DeserializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();

            Config config = deserializer.Deserialize<Config>(yamlContent);

            Log.debugMode = config.debug;
            DaemonMC.servername = config.serverName;
            DaemonMC.maxOnline = config.maxOnline;
        }
    }
}
