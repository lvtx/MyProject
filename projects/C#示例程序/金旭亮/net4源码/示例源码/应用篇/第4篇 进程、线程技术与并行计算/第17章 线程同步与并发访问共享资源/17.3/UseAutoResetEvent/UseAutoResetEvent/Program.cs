using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace UseAutoResetEvent
{
    class Program
    {
        static Thread th1, th2, th3;
        static void Main(string[] args)
        {

            CreateThread();
           
            System.Console.WriteLine("G:放行,ESC:退出\n");

            while (true)
            {
               ConsoleKey key = Console.ReadKey(true).Key;

                if (key == ConsoleKey.G)
                    MyThread.Go();
                if (key == ConsoleKey.Escape)
                {
                    break;
                }

            }
            
        }
    
        //创建3个线程
        private static void CreateThread()
        {
            Thread[] ths = new Thread[10];
            for (int i = 0; i < 10; i++)
            {
                ths[i] = new Thread((new MyThread()).DoWork);
                ths[i].IsBackground = true;//不加这句,则程序不会结束
                ths[i].Start();
            }
        }
    }

    class MyThread
    {
        //多个对象共享"红绿灯"
        static AutoResetEvent are = new AutoResetEvent(false);

        //线程函数,多个线程都拥有多个副本,彼此不相关
        public void DoWork()
        {
            int Counter = 0;
            while (true)
            {
                are.WaitOne();
                Counter++;
                Thread.Sleep(new Random().Next(100,1000));
                System.Console.WriteLine("线程{0}:第{1}次执行", Thread.CurrentThread.ManagedThreadId, Counter);
            }
        }

        //通知阻塞的线程可以继续运行
        public static void Go()
        {
            are.Set();
            System.Console.WriteLine("\n绿灯,放行!");
        }
      
    }
}
