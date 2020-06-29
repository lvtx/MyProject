using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ParallelForUseTLS
{
    class Program
    {
        static void Main(string[] args)
        {
            Func<ThreadData> localInit = delegate()
            {
                Random ran = new Random();
                Thread.Sleep(ran.Next(100,2000));
                Console.WriteLine("线程{0}工作开始于{1}，此值被保存到线程局部存储区中\n",Thread.CurrentThread.ManagedThreadId, DateTime.Now);
                return new ThreadData { Time = DateTime.Now };
            };

            Action<ThreadData> localFinally = delegate(ThreadData data)
            {
                Console.WriteLine("\n线程{0}完成于{1}，它负责执行了{2}次并行循环\n",Thread.CurrentThread.ManagedThreadId, data.Time,data.ExecuteCounter);
            };

            Parallel.For<ThreadData>(0, 10, localInit, DoWork, localFinally);

            Console.ReadKey();

        }

        static ThreadData DoWork(int index, ParallelLoopState state, ThreadData CurrentData)
        {
            Thread.Sleep(1000);
          
            Console.WriteLine("线程{0}第{1}次工作，负责完成并行循环“{2}”,本次循环执行时线程局部存储区中保存的时间为{3}", Thread.CurrentThread.ManagedThreadId, 
                CurrentData.ExecuteCounter+1,
                index + 1, CurrentData.Time);
            CurrentData.ExecuteCounter++;
            CurrentData.Time = DateTime.Now;
           
            return CurrentData;
        }
    }

    /// <summary>
    /// 将被保存到TLS中的数据
    /// </summary>
    class ThreadData
    {
        /// <summary>
        /// 保存当前时间
        /// </summary>
        public DateTime Time;
        /// <summary>
        /// 本线程执行的次数
        /// </summary>
        public int ExecuteCounter;
        
    }
}
