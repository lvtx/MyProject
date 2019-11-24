using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AreYouEqual
{
    class Program
    {
        static void Main(string[] args)
        {
            double i = 0.0001;
            double j = 0.00010000000000000001;
            Console.WriteLine("计算机能表达的数值精度是有限的，所以以下两个数被认为是数值相等：");
            Console.WriteLine("0.0001 ==  0.00010000000000000001? {0}",i == j);  //输出：true

            Console.WriteLine("\n在实际开发时，我们通常会设定一个误差范围（比如10的负10次方），只要两个数的差在这个范围内，就认为两者是相等的。");
            i = 0.0001;
            j = 0.00010000008903434;
            if (Math.Abs(i - j) < 1e-10)
                Console.WriteLine("{0} == {1} ? {2}",i,j,"true");
            else
                Console.WriteLine("{0} == {1} ? {2}",i,j,"false");

            Console.ReadKey();

        }
    }
}
