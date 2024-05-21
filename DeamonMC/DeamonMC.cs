using DeamonMC.Utils.Text;

namespace DeamonMC
{
    public static class DeamonMC
    {
        public static string servername = "DeamonMC";
        public static string version = "1.20.70";
        public static int protocolVersion = 671;
        public static string maxOnline = "10";
        public static void Main()
        {
            Log.debugMode = true;
            Console.WriteLine("Choose DeamonMC mode");
            Console.WriteLine("1 - Server");
            Console.WriteLine("2 - Client");
            string mode = Console.ReadLine();
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
