using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace UseArray
{
    class Program
    {
        static void Main(string[] args)
        {
            Thread th = new Thread(DoWithArray);
            ThreadMethodHelper argu = new ThreadMethodHelper();
            argu.arr = new int[] { -1, 9, 100, 78, 23, 54, -90 };
            th.Start(argu);
            th.Join();
            Console.WriteLine("数组元素清单");
            foreach (int i in argu.arr)
            {
                Console.Write(i.ToString() + "  ");
            }
            Console.WriteLine();
            Console.WriteLine("最大值:{0}", argu.MaxValue);
            Console.WriteLine("最小值:{0}", argu.MinValue);
            Console.WriteLine("总和:{0}", argu.Sum );
            Console.WriteLine("平均值:{0}", argu.Average );

            Console.ReadKey();
        }

        static void DoWithArray(object  obj)
        {
            ThreadMethodHelper argu = obj as ThreadMethodHelper;
            for (int i = 0; i < argu.arr.Length; i++)
            {
                if (argu.arr[i] > argu.MaxValue)
                    argu.MaxValue = argu.arr[i];
                if (argu.arr[i] < argu.MinValue)
                    argu.MinValue = argu.arr[i];
                argu.Sum += argu.arr[i];
            }
            argu.Average = argu.Sum / argu.arr.Length;
        }
    }

    //封装线程的输入和输出信息
    class ThreadMethodHelper
    {
        //线程输入参数
        public int[] arr;
        //函数返回值
        public int MaxValue=0;
        public int MinValue=0;
        public long Sum=0;
        public double Average=0;
    }
}
