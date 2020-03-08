using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IntroduceTask
{
    class Program
    {
        static void Main(string[] args)
        {
            //UseTask();
            UseTaskDely();
           
            Console.WriteLine("演示结束，敲任意键退出...");
            Console.ReadKey(true);
        }
        //用于封装需要并行执行的代码
        static void DoSomeVeryImportantWork(int id, int sleepTime)
        {
            Console.WriteLine("任务{0}正在执行……", id);
            Thread.Sleep(sleepTime);
            Console.WriteLine("任务{0}执行结束。", id);
        }
      

        static void UseTask()
        {
            Console.WriteLine("创建三个Task对象并启动其运行……");
            //任务方式一
            var t1 = new Task(() => DoSomeVeryImportantWork(1, 1500));
            t1.Start();

            //任务方式二
            var t2 = Task.Factory.StartNew(() => DoSomeVeryImportantWork(2, 3000));

            var t3 = Task.Run(() => DoSomeVeryImportantWork(3, 2000));

            Task.WaitAll(t1, t2, t3);
            Console.WriteLine("各任务的状态为：{0}，{1}，{2}",t1.Status,t2.Status,t3.Status);
        }

       
        static void UseTaskDely()
        {
            Console.WriteLine("使用Task.Delay()方法拖慢程序运行速度，仅供演示！");
            Task.Run(() =>
            {
                for (int i = 1; i <= 10; i++)
                {
                    Console.WriteLine("{0}",i);
                    //在真正的程序中，多用await Task.Delay(500);
                    Task.Delay(500).Wait();
                }
            }).Wait();

        }
      

       
    }
}
