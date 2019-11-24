using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("输入第1个数字");
            int value1 = InputValue(1);
            Console.WriteLine("输入第2个数字");
            int value2 = InputValue(2);
            Console.WriteLine("输入运算符");
            Calculate(value1, value2);
            Console.ReadKey();
        }

        static int InputValue(int count)
        {
            string useInput = Console.ReadLine();
            int value = Convert.ToInt32(useInput);
            return value;
        }
        static void Calculate(int value1,int value2)
        {
            string op = Console.ReadLine();//运算符
            switch (op)
            {
                case "+":
                    int value = value1 + value2;
                    Console.WriteLine("结果为{0}",value);
                    break;
                case "-":
                    value = value1 - value2;
                    Console.WriteLine("结果为{0}",value);
                    break;
                case "*":
                    value = value1 * value2;
                    Console.WriteLine("结果为{0}",value);
                    break;
                case "/":
                    value = value1 / value2;
                    Console.WriteLine("结果为{0}",value);
                    break;
                default:
                    break;
            }
        }
    }
}
