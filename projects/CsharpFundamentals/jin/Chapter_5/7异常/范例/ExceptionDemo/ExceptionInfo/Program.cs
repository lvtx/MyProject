using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExceptionInfo
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                int[] values = new int[10];
                for (int i = 1; i <= 10; i++)
                    Console.WriteLine(values[i]);

            }
            catch (Exception e)
            {
                Console.WriteLine("在输出values数组值时发生数组越界错误");
                Console.WriteLine("异常种类:" + e.GetType().Name);
                Console.WriteLine("系统给出的出错信息：" + e.Message);
                Console.WriteLine("系统调用堆栈信息:" + e.StackTrace);
                Console.WriteLine("引发此错误的方法：" + e.TargetSite);
            }
            Console.ReadKey();
        }
    }
}
