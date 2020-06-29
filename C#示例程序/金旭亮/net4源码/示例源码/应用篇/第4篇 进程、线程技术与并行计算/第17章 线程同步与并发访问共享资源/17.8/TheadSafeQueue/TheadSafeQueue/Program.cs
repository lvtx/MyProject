using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using System.Threading;

namespace TheadSafeQueue
{
    class Program
    {

        static void Main(string[] args)
        {

           // GetDataFromQueue();

            GetDataFromConcurrentQueue();

            Console.ReadKey();

        }

        /// <summary>
        /// 多线程从ConcurrentQueue中提取数据
        /// </summary>
        private static void GetDataFromConcurrentQueue()
        {
            //向队列中存入10个整数
            ConcurrentQueue<int> cq = new ConcurrentQueue<int>();
            for (int i = 0; i < 10; i++) cq.Enqueue(i);
            //线程函数
            Action action = () =>
            {
                int localValue;
                Random ran = new Random();
                while (cq.TryDequeue(out localValue))
                {
                    Thread.Sleep(ran.Next(1, 100));
                    Console.WriteLine("Thread {0}取出{1}", Thread.CurrentThread.ManagedThreadId, localValue);
                }
            };
            //启动四个线程
            for (int i = 0; i < 4; i++)
            {
                Thread th = new Thread(new ThreadStart(action));
                th.Start();
            }
          
        }

        /// <summary>
        /// 多线程从Queue中提取数据
        /// </summary>
        private static void GetDataFromQueue()
        {
            //向队列中存入10个整数
            var cq = new Queue<int>();
            for (int i = 0; i < 10; i++) cq.Enqueue(i);
            //线程函数
            Action ThreadFunc = () =>
            {
                Random ran = new Random();
                try
                {
                    while (cq.Count > 0)
                    {
                        Thread.Sleep(ran.Next(1, 100));
                        Console.WriteLine("Thread {0}取出{1}", Thread.CurrentThread.ManagedThreadId, cq.Dequeue());
                    }
                }
                catch (Exception ex)
                {

                    Console.WriteLine("{0}:{1}", ex.GetType().Name, ex.Message);
                }
            };
            //启动四个线程
            for (int i = 0; i < 4; i++)
            {
                Thread th = new Thread(new ThreadStart(ThreadFunc));
                th.Start();
            }
          
        }
    }
}
