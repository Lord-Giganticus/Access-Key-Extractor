using System;
using Access_Key_Extractor.Library;

namespace Access_Key_Extractor.CLI
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length <= 0)
            {
                Console.WriteLine("Usage:\nExtractor.exe [path]");
                return;
            }
            var arg = args[0];
            var file = new RomFile(arg);
            var buf = Console.OutputEncoding.GetBytes($"Possible access keys (the correct key is usually one of the first){Environment.NewLine}");
            Console.OpenStandardError().Write(buf, 0, buf.Length);
            var ogcolor = Console.ForegroundColor;
            var keys = file.GetKeys();
            if (keys.Length <= 0)
            {
                Console.WriteLine("No possible access keys found.");
                return;
            }
            Console.Write("[ ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("'");
            for (int i = 0; i < (keys.Length - 1); i++)
            {
                Console.Write($"{keys[i]}'");
                Console.ForegroundColor = ogcolor;
                Console.Write(", ");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("'");
            }
            Console.Write($"{keys[^1]}'");
            Console.ForegroundColor = ogcolor;
            Console.Write(" ]");
            Console.ResetColor();
        }
    }
}
