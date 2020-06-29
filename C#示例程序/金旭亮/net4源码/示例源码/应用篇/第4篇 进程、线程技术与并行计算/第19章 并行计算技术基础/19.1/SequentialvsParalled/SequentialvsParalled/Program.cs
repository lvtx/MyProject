using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace SequentialvsParalled
{
    class Program
    {
        //要处理的数据项数
        const int DataSize = 1000000;
       
        //每个数据项进行的数据操作次数
        const int OperationCounterPerDataItem = 100;

        //划分的子任务数
        const int TaskCount = 4;


        static void Main(string[] args)
        {
            int[] data1 = new int[DataSize];
            Console.WriteLine("\n敲任意键执行串行算法...\n");
            Console.ReadKey(true);

            Console.WriteLine("开始执行顺序算法");
            Stopwatch sw = Stopwatch.StartNew();
            IncreaseNumberInSquence(data1,0,data1.Length);
            Console.WriteLine("顺序算法执行结束，使用了{0}毫秒",sw.ElapsedMilliseconds);
            Console.WriteLine("\n敲任意键执行并行算法一...\n");
            Console.ReadKey(true);

            Console.WriteLine("开始执行并行算法一");
            sw = Stopwatch.StartNew();
            IncreaseNumberInParallel(data1);
            Console.WriteLine("并行算法一执行结束，使用了{0}毫秒", sw.ElapsedMilliseconds);
            Console.WriteLine("\n敲任意键执行并行算法二...\n");
            Console.ReadKey(true);

            Console.WriteLine("开始执行并行算法二");
            sw = Stopwatch.StartNew();
            IncreaseNumberInParallel2(data1);
            Console.WriteLine("并行算法二执行结束，使用了{0}毫秒", sw.ElapsedMilliseconds);
          
            Console.ReadKey();

        }

        //依次给一个数组中指定部分的元素执行OperationCounterPerDataItem次操作
        static void IncreaseNumberInSquence(int[] arr,int startIndex,int counter)
        {
            for (int i = 0; i <counter; i++)
                for (int j = 0; j < OperationCounterPerDataItem; j++)
                    arr[startIndex+i]++;
        }

        //将任务划分为TaskCount个子任务，然后并行执行

        static void IncreaseNumberInParallel(int[] arr)
        {
            int counter = DataSize / TaskCount;

            Parallel.For(0, TaskCount, i =>
                {
                    int startIndex = i * counter;
                    IncreaseNumberInSquence(arr, startIndex, counter);
                }
            );
            
        }
        
        //任务划分过细，反而减慢运行速度
        static void IncreaseNumberInParallel2(int[] arr)
        {
            //为每个数据项创建一个任务
            Parallel.For(0, arr.Length, i =>
                {
                    
                    //for (int j = 0; j < OperationCounterPerDataItem; j++)
                    //    arr[i]++;
                    //如果注释掉上面的串行顺序，而采用下面这句，并每个数据项的每个操作建立一个并行任务，
                    //则并行算法比串行算法运行还要慢！
                    Parallel.For(0, OperationCounterPerDataItem, j => arr[i]++);
                }
            );
        }
    }
}
