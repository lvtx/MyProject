using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WriteToConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            "my String".WriteToConsole();
            "Press any Key...".WriteToConsoleAndWaitKeyPress();
        }
    }

    public static class Extensions
    {
        public static void WriteToConsole(this string content)
        {
            Console.WriteLine(content);
        }
        public static void WriteToConsoleAndWaitKeyPress(this string content)
        {
            Console.Write(content);
            Console.ReadKey();
        }
    }
}
