namespace DeamonMC.Utils.Text
{
    public class Log
    {
        public static bool debugMode = false;
        public static void debug(string message)
        {
            if (debugMode)
            {
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine(message);
                Console.ResetColor();
            }
        }

        public static void info(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        public static void warn(string message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        public static void error(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ResetColor();
        }
    }
}
