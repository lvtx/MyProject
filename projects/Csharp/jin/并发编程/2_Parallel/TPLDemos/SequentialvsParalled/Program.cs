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

        static void Main(string[] args)
        {
            int[] data1 = new int[DataSize];
            Console.WriteLine("\n敲任意键执行串行算法...\n");
            Console.ReadKey(true);

            Console.WriteLine("开始执行“纯”的顺序算法");
            Stopwatch sw = Stopwatch.StartNew();
            IncreaseNumberInSquence(data1, 0, data1.Length);
            Console.WriteLine("顺序算法执行结束，使用了{0}毫秒", sw.ElapsedMilliseconds);


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

            Console.WriteLine("\n敲任意键执行并行算法三...\n");
            Console.ReadKey(true);

            Console.WriteLine("开始执行并行算法三");
            sw = Stopwatch.StartNew();
            IncreaseNumberInParallel3(data1);
            Console.WriteLine("并行算法三执行结束，使用了{0}毫秒", sw.ElapsedMilliseconds);

            Console.ReadKey(true);

        }

        //依次给一个数组中指定部分的元素(共counter个)执行OperationCounterPerDataItem次操作
        static void IncreaseNumberInSquence(int[] arr, int startIndex, int counter)
        {
            for (int i = 0; i < counter; i++)
                for (int j = 0; j < OperationCounterPerDataItem; j++)
                    arr[startIndex + i]++;
        }

        //依据CPU核心数将任务划分为多个子任务，然后并行执行

        static void IncreaseNumberInParallel(int[] arr)
        {
            Console.WriteLine("手工创建并行任务，因为本机CPU为{0}核，所以分为{0}个子任务交给TPL并行执行", Environment.ProcessorCount);
            //计算每个子任务要处理的数据项数，这里假设能正好整除
            int counter = DataSize / Environment.ProcessorCount;

            Parallel.For(0, Environment.ProcessorCount, i =>
                {
                    int startIndex = i * counter;
                    IncreaseNumberInSquence(arr, startIndex, counter);
                }
            );

        }


        static void IncreaseNumberInParallel2(int[] arr)
        {
            Console.WriteLine("针对原始数组执行并行循环，使用任务并行库内部默认算法", arr.Length);
            Parallel.For(0, arr.Length, i =>
            {
                //直接在这里使用串行循环，则速度很快
                for (int j = 0; j < OperationCounterPerDataItem; j++)
                    arr[i]++;
            }
            );
        }
        //任务划分过细，反而减慢运行速度
        static void IncreaseNumberInParallel3(int[] arr)
        {
            //为每个数据项创建一个任务
            Console.WriteLine("共创建{0}个子任务并行执行", arr.Length);
            Parallel.For(0, arr.Length, i =>
                {
                    //为每个数据项的每个操作建立一个并行任务，
                    //则并行算法与前述所有算法相比，慢得象蜗牛！
                    Parallel.For(0, OperationCounterPerDataItem, j => arr[i]++);
                }
            );
        }
    }
}
