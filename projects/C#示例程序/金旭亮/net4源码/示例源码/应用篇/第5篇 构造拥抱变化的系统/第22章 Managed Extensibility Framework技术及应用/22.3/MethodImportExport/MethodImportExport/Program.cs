using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MethodImportExport
{
    class Program
    {
        static void Main(string[] args)
        {
            MyCalculator calc = new MyCalculator();
            Console.WriteLine(calc.Calculate(100, 200));  //输出300
            Console.ReadKey();
        }
    }
}
