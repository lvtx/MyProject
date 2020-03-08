using System;
using System.Collections.Generic;
using System.Text;

namespace OverloadOperator
{
    class Program
    {
        static void Main(string[] args)
        {
            MyIntArray x = new MyIntArray();
            MyIntArray y = new MyIntArray();

            x.arr = new int[] { 1, 2, 3 };
            y.arr = new int[] { 4, 5, 6 };

            int[] sum = x + y;//调用重载的加法运算符

            for (int i = 0; i < sum.Length; i++)
                Console.WriteLine(sum[i]);

            Console.ReadKey();  //敲任意键结束整个程序
        }
    }
}
