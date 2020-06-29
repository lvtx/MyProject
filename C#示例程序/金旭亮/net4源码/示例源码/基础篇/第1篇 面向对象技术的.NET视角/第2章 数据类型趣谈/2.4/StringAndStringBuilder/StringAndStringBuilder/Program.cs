using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace StringAndStringBuilder
{
    class Program
    {
        static void Main(string[] args)
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();
            UseString();

            watch.Stop();
            Console.WriteLine("UseString()方法耗费时间{0}毫秒", watch.ElapsedMilliseconds);
            watch.Restart();
            UseStringBuilder();
            Console.WriteLine("UseStringBuilder()方法耗费时间{0}毫秒", watch.ElapsedMilliseconds);
            Console.ReadKey();
        }

        static void UseString()
        {
            String str = "";
            for (int i = 1; i <= 10000; i++)
            {
                str += i;
                if (i < 1000)
                    str += "+";
            }

        }

        static void UseStringBuilder()
        {
            //预先分配4K的内存空间
            StringBuilder buffer = new StringBuilder(4096);
            for (int i = 1; i <= 10000; i++)
            {
                buffer.Append(i);
                if (i < 1000)
                    buffer.Append("+");
            }
            String result = buffer.ToString();

        }
    }
}
