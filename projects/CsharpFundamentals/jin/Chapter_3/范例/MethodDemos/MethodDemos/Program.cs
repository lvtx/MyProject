using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MethodDemos
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine("{0}+{1}={2}",100,200,Add(100,200));

            //方法的重载

            var intResult = Add(100, 200);
            Console.WriteLine("{0}+{1}={2}", 100, 200, intResult);
            var doubleResult = Add(100.5d, 200.5d);
            Console.WriteLine("{0}+{1}={2}", 100.5, 200.5, doubleResult);
            var doubleResult2 = Add("100", "200");
            Console.WriteLine("{0}+{1}={2}", 100, 200, doubleResult2);
            Console.ReadKey();
        }

        static int Add(int x, int y)
        {
            Console.WriteLine("调用Add(int,int)");
            return x + y;
        }

        static double Add(double x, double y)
        {
            Console.WriteLine("调用Add(double,double)");
            return x + y;
        }

        static double Add(string x, string y)
        {
            Console.WriteLine("调用Add(string,string)");
            double dx = double.Parse(x);
            double dy = double.Parse(y);
            return dx + dy;
        }

       
    }
}
