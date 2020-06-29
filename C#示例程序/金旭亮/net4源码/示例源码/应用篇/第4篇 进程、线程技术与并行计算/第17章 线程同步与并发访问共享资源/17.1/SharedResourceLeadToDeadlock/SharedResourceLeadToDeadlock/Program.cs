using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;


namespace SharedResourceLeadToDeadlock
{
    class Program
    {
        //共享资源
        static SharedResource R1 = new SharedResource();
        static SharedResource R2 = new SharedResource();
        
        static void Main(string[] args)
        {
            Thread th1 = new Thread(UseSharedResource1);
            Thread th2 = new Thread(UseSharedResource2);
            th1.Start();
            th2.Start();
            //等待两线程运行结束
            th1.Join();
            th2.Join();
        }

        static void UseSharedResource1()
        {
            System.Console.WriteLine("线程{0}申请使用资源R1", Thread.CurrentThread.ManagedThreadId);
            Monitor.Enter(R1);
            System.Console.WriteLine("线程{0}独占使用资源R1", Thread.CurrentThread.ManagedThreadId);
            Thread.Sleep(1000);
            System.Console.WriteLine("线程{0}申请使用资源R2", Thread.CurrentThread.ManagedThreadId);
            Monitor.Enter(R2);
            System.Console.WriteLine("线程{0}独占使用资源R2", Thread.CurrentThread.ManagedThreadId);
            Thread.Sleep(1000);
            System.Console.WriteLine("线程{0}资源R2使用完毕，放弃", Thread.CurrentThread.ManagedThreadId);
            Monitor.Exit(R2);
            System.Console.WriteLine("线程{0}资源R1使用完毕，放弃", Thread.CurrentThread.ManagedThreadId);
            Monitor.Exit(R1);
        }

        static void UseSharedResource2()
        {
            System.Console.WriteLine("线程{0}申请使用资源R2", Thread.CurrentThread.ManagedThreadId);
            Monitor.Enter(R2);
            System.Console.WriteLine("线程{0}独占使用资源R2", Thread.CurrentThread.ManagedThreadId);
            Thread.Sleep(500);
            System.Console.WriteLine("线程{0}申请使用资源R1", Thread.CurrentThread.ManagedThreadId);
            Monitor.Enter(R1);
            System.Console.WriteLine("线程{0}独占使用资源R1", Thread.CurrentThread.ManagedThreadId);
            Thread.Sleep(500);
            System.Console.WriteLine("线程{0}资源R1使用完毕，放弃", Thread.CurrentThread.ManagedThreadId);
            Monitor.Exit(R1);
            System.Console.WriteLine("线程{0}资源R2使用完毕，放弃", Thread.CurrentThread.ManagedThreadId);
            Monitor.Exit(R2);
        }

    }

    class SharedResource
    {
    }
}
