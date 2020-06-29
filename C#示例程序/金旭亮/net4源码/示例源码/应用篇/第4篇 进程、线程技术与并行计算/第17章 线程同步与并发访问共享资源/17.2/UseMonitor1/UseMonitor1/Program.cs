using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

//展示用Monitor访问共享资源
namespace UseMonitor1
{
    class Program
    {
        static void Main(string[] args)
        {
            SharedResource obj = new SharedResource();

            Thread[] ths = new Thread[4];
            for (int i = 0; i < 4; i++)
            {
                ths[i] = new Thread(increaseCount);
                ths[i].Start(obj);
            }
            System.Console.ReadKey();


        }
        static void increaseCount(Object obj)
        {
           //访问实例字段
            VisitInstanceField(obj);
            //访问静态字段
            VisitStaticField();

        }

        //访问静态字段
        private static void VisitStaticField()
        {
            //访问静态字段
            Monitor.Enter(typeof(SharedResource));
            int beginNumber = SharedResource.StaticCount;
            System.Console.WriteLine("线程 {0} 读到的StaticCount起始值为 {1}  ", Thread.CurrentThread.ManagedThreadId, beginNumber);
            for (int i = 0; i < 10000; i++)
            {
                beginNumber++;

            }
            SharedResource.StaticCount = beginNumber;
            System.Console.WriteLine("线程 {0} 结束， SharedResource.StaticCount={1}",
            Thread.CurrentThread.ManagedThreadId, SharedResource.StaticCount);
           Monitor.Exit(typeof(SharedResource));
        }

        //访问实例字段
        private static void VisitInstanceField(Object obj)
        {

             Monitor.Enter(obj);
            //lock (obj)
            //{
                int beginNumber = (obj as SharedResource).InstanceCount;
                System.Console.WriteLine("线程 {0} 读到的InstanceCount起始值为 {1}  ", Thread.CurrentThread.ManagedThreadId, beginNumber);
                for (int i = 0; i < 10000; i++)
                {
                    beginNumber++;

                }
                (obj as SharedResource).InstanceCount = beginNumber;
                System.Console.WriteLine("线程 {0} 结束，Obj.InstanceCount={1}",
                Thread.CurrentThread.ManagedThreadId, (obj as SharedResource).InstanceCount);
            //}
                Monitor.Exit(obj);
            
        }
    }
    //共享资源类
    class SharedResource
    {
        public int InstanceCount = 0;        //多线程共享的实例字段
        public static int StaticCount = 0;  //多线程共享的静态字段

    }
}
