using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoxingAndUnboxing
{
    class Program
    {
        static void Main(string[] args)
        {
            Stopwatch watch = new Stopwatch(); 
            watch.Start();
            object obj = 0L;
            long temp;
            for (int i = 1; i <= 1000000; i++)
            {
                temp = (long)obj;
                obj = temp + i;
            }
            watch.Stop();
            Console.WriteLine("总和{0}花费的时间{1}ms",obj,watch.ElapsedMilliseconds);
            watch.Restart();
            long intValue = 0;
            for (int i = 1; i <= 1000000; i++)
            {
                intValue += i;
            }
            watch.Stop();
            Console.WriteLine("总和{0}花费的时间{1}ms",intValue,watch.ElapsedMilliseconds);
            Console.ReadKey();
        }
    }
}
