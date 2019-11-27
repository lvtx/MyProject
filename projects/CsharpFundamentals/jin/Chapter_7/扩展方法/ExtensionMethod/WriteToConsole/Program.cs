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
            "my name is".WriteToConsole();
            Console.ReadLine();
        }
    }
    static class Extensions
    {
        public static void WriteToConsole(this string content)
        {
            Console.WriteLine(content);
        }
    }
}
