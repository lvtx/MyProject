using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace ThreadJoin
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("主线程开始运行");
            Thread th = new Thread(new ThreadStart(ThreadAMethod));
            th.Start();
            th.Join();
            Console.WriteLine("主线程退出");
            Console.ReadKey();
        }

        static void ThreadAMethod()
        {
            for (int i = 0; i < 5; i++)
            {
                Console.WriteLine("辅助线程正在执行:"+i.ToString());
                Thread.Sleep(200);
            }
            Console.WriteLine("辅助线程执行结束");
        }
    }
}
