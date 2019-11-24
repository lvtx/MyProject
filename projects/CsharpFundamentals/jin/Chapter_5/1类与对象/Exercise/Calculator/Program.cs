using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyMathLibrary;

namespace Calculator
{
    class Program
    {
        static void Main(string[] args)
        {
            int m, n;
            MathOpt mathOpt = new MathOpt();
            Console.WriteLine("请输入数字x");
            m = int.Parse(Console.ReadLine());
            Console.WriteLine("请输入数字y");
            n = int.Parse(Console.ReadLine());
            Console.WriteLine("Add = {0}",mathOpt.Add(m,n));
            Console.WriteLine("Sub = {0}",mathOpt.Sub(m,n));
            Console.WriteLine("Mul = {0}",mathOpt.Mul(m,n));
            Console.WriteLine("Div = {0}",mathOpt.Div(m,n));
            Console.ReadKey();
        }
    }
}
