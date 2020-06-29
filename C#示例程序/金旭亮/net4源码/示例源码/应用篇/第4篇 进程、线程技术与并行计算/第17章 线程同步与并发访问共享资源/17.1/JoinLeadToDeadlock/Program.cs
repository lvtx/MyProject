using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace JoinLeadToDeadlock
{
    class Program
    {
        static Thread mainThread;

        static void Main(string[] args)
        {
            Console.WriteLine("主线程开始运行");
            mainThread = Thread.CurrentThread;

            Thread ta = new Thread(new ThreadStart(ThreadAMethod));
            ta.Start();
              Console.WriteLine("主线程等待线程A结束……");
            ta.Join(); //等待线程A结束
            Console.WriteLine("主线程退出");

        }

        static void ThreadAMethod()
        {
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine(Convert.ToString(i) + ": 线程A正在执行");
                Thread.Sleep(1000);
            }
            Console.WriteLine("线程A等待主线程退出……");
            //等待主线程结束
            mainThread.Join();
        }

    }
}
