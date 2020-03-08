using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ParallelLoopStop
{
    class Program
    {
        /// <summary>
        /// 要处理的数据项
        /// </summary>
        private static int DataSize = 10000000;

        /// <summary>
        /// 每个数据项要执行的递增操作次数
        /// </summary>
        private static int OperationCounterPerDataItem = 10;

        /// <summary>
        /// 并行循环个数
        /// </summary>
        private static int TaskCount = 5;

        static void Main(string[] args)
        {
            int[] arr = new int[DataSize];
            ParallelLoopResult result = new ParallelLoopResult();
            try
            {
                //启动并行循环
                result = IncreaseNumberInParallel(arr);
                //检测并行循环结果
                Console.WriteLine("\n并行循环结束后的返回值：");
                Console.WriteLine("ParallelLoopResult.IsCompleted={0}", result.IsCompleted);
                if (result.LowestBreakIteration.HasValue)
                    Console.WriteLine("ParallelLoopResult.LowestBreakIteration={0}", result.LowestBreakIteration);
                else
                    Console.WriteLine("ParallelLoopResult.LowestBreakIteration.HasValue={0}", result.LowestBreakIteration.HasValue);
            }
            catch (Exception ex)
            {
                Console.WriteLine("{0}:{1}", ex.GetType(), ex.Message);
            }

            Console.ReadKey();

        }

        //对数组中的元素施加递增操作
        static void IncreaseNumberInSquence(int[] arr, int startIndex, int counter, ParallelLoopState state)
        {
            for (int i = 0; i < counter; i++)
                for (int j = 0; j < OperationCounterPerDataItem; j++)
                {
                    //检测是否需要中止执行
                    if (state.ShouldExitCurrentIteration)
                    {
                        Console.WriteLine("由于检测到其他线程已中止，本线程{0}响应此请求，终止自身循环，未完成所有工作", Thread.CurrentThread.ManagedThreadId);
                        return;
                    }
                    arr[startIndex + i]++;
                }

            Console.WriteLine("线程{0}完成工作", Thread.CurrentThread.ManagedThreadId);
        }

        //将任务划分为TaskCount个并行循环，然后并行执行
        static ParallelLoopResult IncreaseNumberInParallel(int[] arr)
        {
            int counter = DataSize / TaskCount; //每个并行循环要处理的数据项数

            Random ran = new Random();


            //启动并行循环
            ParallelLoopResult result = Parallel.For(0, TaskCount, (int i, ParallelLoopState state) =>
                 {
                     Console.WriteLine("线程{0}正在执行并行循环，索引={1}", Thread.CurrentThread.ManagedThreadId, i);

                     //flag变量用于随机发出线程中止通知
                     int flag = ran.Next(1, 100);
                     Console.WriteLine("线程{0}随机生成的Flag={1}", Thread.CurrentThread.ManagedThreadId, flag);
                     //当生成的随机数可以被3整除时，此线程将中止执行
                     if (flag % 3 == 0)
                     {
                         Console.WriteLine("线程{0}主动终止循环，未完成所有工作", Thread.CurrentThread.ManagedThreadId);

                         //当从以下两句代码中选一句执行时，程序的执行结果是不一样的，请读者运行程序测试一下。
                         state.Stop();
                         //state.Break();
                         return;  //当使用state.Break();时，也应该注释掉return语句

                         //如果将上面的这三句全部注释掉，再取消以下这句的注释，当程序运行时，
                         //会看到在并行循环中抛出的异常也会导致并行循环的中止
                         //throw new Exception("主动抛出的异常");
                     }
                     int startIndex = i * counter;
                     IncreaseNumberInSquence(arr, startIndex, counter, state);

                 });
            return result;


        }


    }
}
