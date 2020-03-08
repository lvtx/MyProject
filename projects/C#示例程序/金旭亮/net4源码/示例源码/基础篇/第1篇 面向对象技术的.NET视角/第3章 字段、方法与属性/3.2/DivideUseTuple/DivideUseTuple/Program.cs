using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DivideUseTuple
{
    class Program
    {
        static void Main(string[] args)
        {
 
            Tuple<int, int> result = Divide(10, 3);
            Console.WriteLine("{0} {1}", result.Item1, result.Item2);	// 输出 "3 1"
        }
        static Tuple<int, int> Divide(int x, int y)
        {
           return new Tuple<int, int>(x / y, x % y);
        }

    }
}
