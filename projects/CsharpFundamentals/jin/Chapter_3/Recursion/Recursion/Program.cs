using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recursion
{
    class Program
    {
        static void Main(string[] args)
        {
            long y1 = Factorial.Calcuate(10);
            long y2 = Factorial.Calcuate2(10);
            Console.WriteLine("10的阶乘为{0}", y1);
            Console.WriteLine("10的阶乘为{0}", y2);
            Console.ReadKey();
        }
    }
}
