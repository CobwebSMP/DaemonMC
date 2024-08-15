using DaemonMC.Utils.Text;
using YamlDotNet.Serialization.NamingConventions;
using YamlDotNet.Serialization;
using Org.BouncyCastle.Tls;

namespace DaemonMC
{
    public static class DaemonMC
    {
        public static string servername = "DaemonMC";
        public static string version = "1.21.20";
        public static int protocolVersion = 705;
        public static string maxOnline = "10";
        public static void Main()
        {
            Console.WriteLine(" _____                                ______   ______ ");
            Console.WriteLine("(____ \\                              |  ___ \\ / _____)");
            Console.WriteLine(" _   \\ \\ ____  ____ ____   ___  ____ | | _ | | /      ");
            Console.WriteLine("| |   | / _  |/ _  )    \\ / _ \\|  _ \\| || || | |      ");
            Console.WriteLine("| |__/ ( ( | ( (/ /| | | | |_| | | | | || || | \\_____ ");
            Console.WriteLine("|_____/ \\_||_|\\____)_|_|_|\\___/|_| |_|_||_||_|\\______)");
            Console.WriteLine("");
            Log.info($"Setting up server for {maxOnline} players with Minecraft {version}");

            Config.Set();
            //Console.WriteLine("Choose DaemonMC mode");
            //Console.WriteLine("1 - Server");
            //Console.WriteLine("2 - Client");
            //string mode = Console.ReadLine();
            var mode = "1";
            if (mode == "1")
            {
                Server.ServerF();
            }else if (mode == "2")
            {
                Client.ClientF();
            }
            else
            {
                Console.WriteLine("unknown mode");
                Console.WriteLine("");
                Main();
            }
        }
    }
}
