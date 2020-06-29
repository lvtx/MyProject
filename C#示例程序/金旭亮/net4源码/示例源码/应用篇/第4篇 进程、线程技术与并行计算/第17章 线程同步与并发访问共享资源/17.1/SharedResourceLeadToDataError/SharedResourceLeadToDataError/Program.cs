using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace SharedResourceLeadToDataError
{
    class Program
    {
        static void Main(string[] args)
        {
            Thread[] ths = new Thread[4];
            for (int i = 0; i < 4; i++)
            {
                ths[i] = new Thread(increaseCount);
                ths[i].Start();
            }
            System.Console.ReadKey();

        }

        static void increaseCount()
        {
            Random ran = new Random();
            Thread.Sleep(ran.Next(100, 5000));
            int beginNum = SharedResource.Count;
            System.Console.WriteLine("线程 {0} 读到的起始值为 {1}  ", Thread.CurrentThread.ManagedThreadId, beginNum);
            for (int i = 0; i < 10000; i++)
            {
                beginNum++;

            }
            SharedResource.Count = beginNum;
            System.Console.WriteLine("线程 {0} 结束，SharedResource.Count={1}", Thread.CurrentThread.ManagedThreadId, SharedResource.Count);


        }
    }

    class SharedResource
    {
        public static int Count = 0;

    }
}
