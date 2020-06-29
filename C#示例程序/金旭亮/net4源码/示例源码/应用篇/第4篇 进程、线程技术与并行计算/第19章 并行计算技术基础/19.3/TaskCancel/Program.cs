using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace TaskCancel
{
    class Program
    {
        static void Main(string[] args)
        {
            CancellationTokenSource cts = new CancellationTokenSource();

            //需要执行的任务函数
            Action taskFunction = delegate()
            {
                for (int i = 0; i < 100; i++)
                {

                    cts.Token.ThrowIfCancellationRequested();


                    Console.WriteLine(i);
                    Thread.Sleep(200);

                }
            };

            Console.WriteLine("敲任意键发出取消任务请求...");

            //请读者进行以下对比试验：
            //(1) 让Task对象关联一个令牌对象，然后运行程序
            //Task tsk = new Task(taskFunction, cts.Token);
            //(2) 让Task对象不关联任何一个令牌对象，然后运行程序 
            //Task tsk = new Task(taskFunction);

            Task tsk = new Task(taskFunction, cts.Token);

            tsk.Start();
            Console.ReadKey(true);
            //发出异步取消任务请求
            cts.Cancel();
            Console.WriteLine("主线程已发出取消请求！");

            //同步等待工作任务停止  
            try
            {
                tsk.Wait();
            }
            catch (AggregateException ae)
            {
                ae.Flatten(); //展平整个异常树
                foreach (Exception e in ae.InnerExceptions)
                {
                    if (e is TaskCanceledException)
                        Console.WriteLine("报告主线程,您发给我的取消请求已经收到，任务已经取消！");
                    if (e is OperationCanceledException)
                        Console.WriteLine("报告主线程,您发出的取消命令已经收到，任务已经取消！");
                    Console.WriteLine("{0}：{1}", e.GetType(), e.Message);
                }
            }


            Console.WriteLine("Task对象的当前状态：{0}", tsk.Status.ToString());
            Console.ReadKey();
        }
    }
}
