using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace BoxAndUnBox
{
    class Program
    {
        static void Main(string[] args)
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();
            long result = SumWithoutBox();
            watch.Stop();
            Console.WriteLine("SumWithoutBox()方法返回计算结果：{0}，用时{1}毫秒",
               result, watch.ElapsedMilliseconds);
            watch.Restart();
            result = SumWithBox();
            watch.Stop();
            Console.WriteLine("SumWithBox()方法返回计算结果：{0}，用时{1}毫秒",
         result, watch.ElapsedMilliseconds);
            Console.ReadKey();
        }

        static long SumWithoutBox()
        {
            long sum = 0;
            for (long i = 0; i < 10000000; i++)
                sum += i;
            return sum;
        }

        static long SumWithBox()
        {
            object sum = 0L; //装箱
            for (long i = 0; i < 10000000; i++)
                sum = (long)sum + i;//先拆箱,求和，再装箱
            return (long)sum;//拆箱
        }
    }
}
