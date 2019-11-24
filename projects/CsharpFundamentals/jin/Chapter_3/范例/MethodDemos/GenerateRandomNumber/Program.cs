using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenerateRandomNumber
{
    class Program
    {
        static void Main(string[] args)
        {
            Random ran = new Random(System.Environment.TickCount);
            for (int i = 0; i < 100; i++)
                Console.Write(" {0}", ran.Next(1, 100));
            Console.ReadKey(true);
        }
    }
}
