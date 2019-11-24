using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompareInputValue
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("输入两个数字");
            int value1 = InputValue();
            int value2 = InputValue();
            SizeOfValue(value1, value2);
            Console.ReadLine();
        }
        static int InputValue()
        {
            string useInput = Console.ReadLine();
            int value = Convert.ToInt32(useInput);
            return value;
        }
        static void SizeOfValue(int value1,int value2)
        {
            if (value1 > value2)
            {
                Console.WriteLine("最大值为{0}",value1);
            }
            else
            {
                Console.WriteLine("最大值为{0}",value2);
            }
        }
    }
}
