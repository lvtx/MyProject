using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HowSlowlyIsBoxing
{
    class Program
    {
        static void Main(string[] args)
        {
            int num = 123;
            object obj = num;
            int value = (int)obj;


            Stopwatch watch = new Stopwatch();
            watch.Start();
            long resultLong = 0;

            for (int i = 1; i <= 1000000; i++)
            {
                resultLong += i;
            }
            watch.Stop();
            Console.WriteLine("使用long变量求和，计算结果为{0}，花费时间{1}毫秒。", resultLong, watch.ElapsedMilliseconds);
            watch.Restart();

            object resultObj = 0l;
            long temp;

            for (long i = 1; i <= 1000000; i++)
            {
                temp = (long)resultObj;
                resultObj = temp + i;
            }

            watch.Stop();
            Console.WriteLine("使用object变量求和，计算结果为{0}，花费时间{1}毫秒。",
                resultObj, watch.ElapsedMilliseconds);

            Console.ReadKey();
        }
    }
}
