using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ThreadInterrupt
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("主线程：创建工作线程并启动它……");
            NotAllowedToSleep worker = new NotAllowedToSleep();
            Thread newThread =
                new Thread(new ThreadStart(worker.ThreadMethod));
            newThread.Start();

            // 允许工作线程进入休眠状态
            worker.CanSleep = true;

             Console.WriteLine("主线程：调用工作线程的Interrupt()方法");
            //如果新线程已处于休眠状态，通知它结束休眠，
            //如果新线程打算进入休眠，则不允许它进入休眠状态
            newThread.Interrupt();
  
            // 等待工作线程结束
            newThread.Join();
            Console.WriteLine("主线程：本线程结束，敲任意键结束整个程序。");
            Console.ReadKey();

        }
    }


}
