using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TaskCooperation
{
    class Program
    {
        static void Main(string[] args)
        {
            //UseContinueWith();
            //UseConditionalContinueWith();
            //UseParentAndChildren();
            //UseParentAndChildren2();
            //UseWaitAll();
            //UseContinueWhenAll();
            Console.ReadKey(true);
        }

        #region "使用ContinueWith"
        static void UseContinueWith()
        {
            Task.Run(() => DoStep1()).ContinueWith((prevTask) => DoStep2());
        }

        static void DoStep1()
        {
            Console.WriteLine("第一步");
        }
        static void DoStep2()
        {
            Console.WriteLine("第二步");
        }

        static void UseConditionalContinueWith()
        {

            Task<int> task = Task.Run(() =>
            {
                int value = new Random().Next(1, 100);
                //要测试出错情况，取消以下注释，Ctrl+F5运行示例程序
                //throw new Exception("无效的数值");
                return value;
            });
            //正常运行结束，执行这句代码
            task.ContinueWith(prev =>
            {
                Console.WriteLine("前个任务传来的值为：{0}", prev.Result);
            }, TaskContinuationOptions.OnlyOnRanToCompletion);
            //出错了，执行以下这些代码
            task.ContinueWith(prev =>
            {
                Console.WriteLine("\n任务在执行时出现未捕获异常，其信息为：\n{0}", prev.Exception);

            }, TaskContinuationOptions.OnlyOnFaulted);

            try
            {
                task.Wait();
                Console.WriteLine("工作结束");
            }
            catch (Exception ex)
            {
                Console.WriteLine("\n使用try...catch捕获Wait()方法抛出的异常：\n{0}", ex);
            }


        }
        #endregion

        #region "一父多子类型的任务"
        //第一种方式，父任务中创建子任务，然后等待其完成
        static void UseParentAndChildren()
        {
            Task tskParent = new Task(() =>
            {
                Console.WriteLine("父任务开始……");
                //父任务完成的工作……
                Console.WriteLine("父任务启动了两个子任务");
                //创建后继子任务并自动启动
                Task child1 = Task.Run(() =>
                {
                    Console.WriteLine("子任务一在行动……");
                    Task.Delay(1000).Wait();
                    Console.WriteLine("子任务一结束");
                });
                Task child2 = Task.Run(() =>
                {
                    Console.WriteLine("子任务二在行动……");
                    Task.Delay(500).Wait();
                    Console.WriteLine("子任务二结束");
                });
                //如果没有WaitAll（）,那么，父任务将在子任务之前结束
                //可以试着注释掉以下这句，看看效果
                Task.WaitAll(child1, child2);
            });

            //启动父任务
            tskParent.Start();
            //等待整个任务树的完成
            tskParent.Wait();
            Console.WriteLine("父任务完成了自己的工作，功成身退。\n");
        }
        /// <summary>
        /// 方式二：不使用Task.Run()创建子任务，而是使用
        /// Task.Factory.StartNew()方法创建子任务，并
        /// 传给它一个TaskCreationOptions.AttachedToParent参数
        /// 从而无需在父任务中WaitAll()
        /// </summary>
        static void UseParentAndChildren2()
        {
            Task tskParent = Task.Factory.StartNew(() =>
            {
                Console.WriteLine("父任务开始……");
                //父任务完成的工作……
                Console.WriteLine("父任务启动了两个子任务");
                //创建后继子任务并自动启动
                var child1 = Task.Factory.StartNew(() =>
                {
                    Console.WriteLine("子任务一在行动……");
                    Task.Delay(1000).Wait();
                    Console.WriteLine("子任务一结束");
                }, TaskCreationOptions.AttachedToParent);

                var child2 = Task.Factory.StartNew(() =>
                {
                    Console.WriteLine("子任务二在行动……");
                    Task.Delay(500).Wait();
                    Console.WriteLine("子任务二结束");
                }, TaskCreationOptions.AttachedToParent);


            });

            //等待整个任务树的完成
            tskParent.Wait();
            Console.WriteLine("父任务完成了自己的工作，功成身退。\n");

        }
        #endregion

        #region"使用WaitAll"

        static void UseWaitAll()
        {
            Console.WriteLine("启动三个并行任务……\n");
            var t1 = Task.Run(() => DoSomeVeryImportantWork(1, 3000));
            var t2 = Task.Run(() => DoSomeVeryImportantWork(2, 1000));
            var t3 = Task.Run(() => DoSomeVeryImportantWork(3, 300));
            Task.WaitAll(new Task[] { t1, t2, t3 });
            Console.WriteLine("\n所有工作都执行完了。");
        }
        static void DoSomeVeryImportantWork(int id, int sleepTime)
        {
            Console.WriteLine("任务{0}正在执行……", id);
            Thread.Sleep(sleepTime);
            Console.WriteLine("任务{0}执行结束。", id);
        }
        #endregion

        #region "使用ContinueWhenAll"

        static void UseContinueWhenAll()
        {
            //创建“前期”任务数组
            Task[] tasks = new Task[]{
                Task.Run(() =>
                    {
                          Thread.Sleep(1000);  //模拟任务的延迟
                          Console.WriteLine("前期任务1");
                    }),
                Task.Run(()=>
                    {
                        Console.WriteLine("前期任务2");
                    })
            };
            //所有前期任务完成之后，启动下一个任务
            //to do:可以把ContinueWhenAll换成ContinueWhenAny进行试验，看看结果有何不同？
            Task.Factory.ContinueWhenAll(tasks, prevTasks =>
            {
                Console.WriteLine("前期共有任务{0}个，这是收尾任务！", prevTasks.Count());
            });
            //Task.Factory.ContinueWhenAny(tasks, prevTask =>
            //{
            //    Console.WriteLine("前期任务的状态是{0}，这是收尾任务！", prevTask.Status);
            //});
        }
        #endregion
    }
}
