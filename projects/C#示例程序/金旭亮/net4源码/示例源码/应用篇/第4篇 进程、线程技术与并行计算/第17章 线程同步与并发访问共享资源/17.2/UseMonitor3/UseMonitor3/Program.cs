using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace UseMonitor3
{
    class Program
    {
        static void Main(string[] args)
        {
            //创建共享资源
            SharedResource obj = new SharedResource();

            //创建线程对象并启动
            Thread th = new Thread(FirstThreadMethod);
            th.Start(obj);

            
            Thread[] ths = new Thread[10];
            for (int i = 0; i < 10; i++)
            {
                ths[i] = new Thread(NextThreadMethod);
                ths[i].Start(obj);
            }

            //程序暂停
            System.Console.ReadKey();

        }

        static void FirstThreadMethod(Object obj)
        {
            lock (obj)
            {
                (obj as SharedResource).InstanceCcount += 100;
                System.Console.WriteLine("线程{0}完成工作，obj.InstanceCcount={1}", Thread.CurrentThread.ManagedThreadId, (obj as SharedResource).InstanceCcount);
                Monitor.Pulse(obj); //通知下一线程
            }
        }

        static void NextThreadMethod(Object obj)
        {
            lock (obj)
            {
                //初始线程还未工作，因为字段保持初始值0
                if ((obj as SharedResource).InstanceCcount == 0)
                    Monitor.Wait(obj);//等待
                (obj as SharedResource).InstanceCcount += 100;
                System.Console.WriteLine("线程{0}完成工作，obj.InstanceCcount={1}", Thread.CurrentThread.ManagedThreadId, (obj as SharedResource).InstanceCcount);
                Monitor.Pulse(obj);
            }
        }
    }

    //共享资源类
    class SharedResource
    {
        public int InstanceCcount = 0;        //多线程共享的实例字段
    }
}
