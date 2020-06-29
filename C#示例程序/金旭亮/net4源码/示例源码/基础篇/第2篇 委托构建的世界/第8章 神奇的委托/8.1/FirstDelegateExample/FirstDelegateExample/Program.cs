using System;
using System.Collections.Generic;
using System.Text;

namespace FirstDelegateExample
{
    public class MathOpt
    {
        public int Add(int argument1, int argument2)
        {
            return argument1 + argument2;
        }
       
    }

    public delegate int MathOptDelegate(int value1, int value2);

    class Program
    {
        static void Main(string[] args)
        {
            MathOptDelegate oppDel;
            MathOpt obj = new MathOpt();
            oppDel = obj.Add;
            Console.WriteLine(oppDel(1, 2)); //输出 3
            Console.ReadKey();
        }
    }
}
