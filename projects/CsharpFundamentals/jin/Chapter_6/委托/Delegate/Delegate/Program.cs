using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstDelegate
{
    public delegate int MathOptDelegate(int value1, int value2);
    class Program
    {
        static void Main(string[] args)
        {
            MathOptDelegate mathOptDel;
            MathOpt mathOpt = new MathOpt();
            mathOptDel = mathOpt.Add;
            //可以引用一个静态变量
            //mathOptDel = MathOpt.Sub;
            Console.WriteLine(mathOptDel(3,4));
            Console.WriteLine(UseDelegate(mathOptDel, 5, 6));
            Console.WriteLine(UseDelegate(mathOpt.Add,5,6));
            Console.WriteLine(UseDelegate(MathOpt.Sub,3,2));
            Console.ReadLine();
        }
        static int UseDelegate(MathOptDelegate mathOptDel,int value1,int value2)
        {
            return mathOptDel(value1, value2);
        }
    }
    class MathOpt
    {
        public int Add(int a,int b)
        {
            return a + b;
        }
        public static int Sub(int a,int b)
        {
            return a - b;
        }
    }
}
