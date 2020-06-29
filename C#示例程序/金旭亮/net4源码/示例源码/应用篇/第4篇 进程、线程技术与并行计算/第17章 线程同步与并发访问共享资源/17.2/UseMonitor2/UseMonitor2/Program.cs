using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace UseMonitor2
{
    class Program
    {
        static void Main(string[] args)
        {
            //创建共享资源
            SharedResource obj = new SharedResource();
            //创建线程对象并启动
            Thread tha = new Thread(ThreadMethodA);
            Thread thb = new Thread(ThreadMethodB);
            tha.Start(obj);
            thb.Start(obj);

            //程序暂停
            System.Console.ReadKey();

        }

        static void ThreadMethodA(Object obj)
        {
            lock (obj)
            {
                (obj as SharedResource).InstanceCount+=100;
                System.Console.WriteLine("线程A完成工作，obj.InstanceCount={0}", (obj as SharedResource).InstanceCount);
                Monitor.Pulse(obj); //通知B线程
            }
        }

        static void ThreadMethodB(Object obj)
        {
            lock (obj)
            {
                //A线程还未工作，因为字段保持初始值0
                //如果注释掉此条件判断语句，则有可能会发生死锁
               if((obj as SharedResource).InstanceCount==0)
                    Monitor.Wait(obj);//等待
                (obj as SharedResource).InstanceCount+=100;
                System.Console.WriteLine("线程B完成工作，obj.InstanceCount={0}", (obj as SharedResource).InstanceCount);
            }
        }
    }

    //共享资源类
    class SharedResource
    {
        public int InstanceCount = 0;        //多线程共享的实例字段
    }

   
}
