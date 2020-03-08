using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ParallelFor
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("顺序执行的For循环...");
            for (int i = 0; i < 10; i++)
                DoWork(i);
            Console.WriteLine("并行执行的For循环...");
            Parallel.For(0, 10, (i) => DoWork(i));
            Console.ReadKey();

        }

        static void DoWork(int index)
        {
            Thread.Sleep(100);
            Console.WriteLine(index);
        }
    }
}
