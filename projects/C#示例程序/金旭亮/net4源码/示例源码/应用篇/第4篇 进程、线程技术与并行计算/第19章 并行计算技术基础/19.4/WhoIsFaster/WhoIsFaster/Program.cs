using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace WhoIsFaster
{
    class Program
    {
        static int[] arr = Enumerable.Range(1000000000, 100000).ToArray();
        static void Main(string[] args)
        {
            Measure("串行算法", () =>
            {
                bool[] results = new bool[arr.Length];
                for (int i = 0; i < arr.Length; i++)
                    results[i] = IsPrime(arr[i]);
            });

            Measure("LINQ to Objects 查询", () =>
            {
                bool[] results = (from num in arr
                                  select IsPrime(num)).ToArray();
            });

            Measure("PLINQ 查询", () =>
            {
                bool[] results = (from num in arr.AsParallel()
                                  select IsPrime(num)).ToArray();
            });
            Console.ReadKey();
        }
        /// <summary>
        /// 某数是否是质数
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        static bool IsPrime(long x)
        {
            if (x <= 2) return x == 2;
            if (x % 2 == 0) return false;
            long sqrtx = (long)Math.Ceiling(Math.Sqrt(x));
            for (long i = 3; i <= sqrtx; i += 2)
            {
                if (x % i == 0) return false;
            }
            return true;

        }
        static void Measure(string methodName, Action method)
        {
            Console.WriteLine("---------------------");
            Stopwatch timer = new Stopwatch();
            timer.Start();
            method();
            timer.Stop();
            Console.WriteLine("{0}执行了{1}毫秒", methodName, timer.ElapsedMilliseconds);
            Console.WriteLine("---------------------\n");
        }
    }
}
