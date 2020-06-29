using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskSynchronizeDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            //WhenAllAndContinueWithTest();
            ContinueWhenAllTest();
            Console.ReadKey();
        }
        /// <summary>
        /// 使用WhenAll设置指定多个子任务执行完毕，之后启动一个后继任务处理其结果
        /// 使用Task.WhenAll()配合Task.ContinueWith()方法实现 
        /// </summary>
        private static void WhenAllAndContinueWithTest()
        {
            Task<string> subTask1 = new Task<string>(() =>
            {
                throw new InvalidOperationException("throwed in sub Task1");
                //return "sub task1";
            });
            Task<string> subTask2 = new Task<string>(() =>
            {
                return "sub task2";
            });
            subTask1.Start();
            subTask2.Start();

            //Task.WhenAll()方法将创建一个新任务getTaskResults,
            //当subTask1和subTask2完成之后，此任务将进入完成状态
            Task<String[]> getTaskResults = Task.WhenAll(subTask1, subTask2);
            //指定当getTaskResults任务完成后的后继任务
            getTaskResults.ContinueWith(t =>
            {
                Console.WriteLine("Two sub task is completed");
                //只要有一个子任务抛出异常，getTaskResult任务都会进入Faulted状态
                if (t.IsFaulted)
                {
                    //接收到的异常类型为：AggregateException
                    Console.WriteLine(t.Exception);
                }
                else
                {
                    //t.Result是一个String[]
                    foreach (string result in t.Result)
                    {
                        Console.WriteLine(result);
                    }
                   
                }
            });
            //由WhenAll（）创建的任务无需显式启动，如果硬要启动，会触发：'System.InvalidOperationException' 
            //可以取消以下注释进行测试
            //getTaskResults.Start();
        }
        /// <summary>
        /// 使用WhenAll设置指定多个子任务执行完毕，之后启动一个后继任务处理其结果
        /// 使用TaskFactory.ContinueWhenAll()方法实现
        /// </summary>
        private static void ContinueWhenAllTest()
        {
            Task<string> subTask1 = new Task<string>(() =>
            {
                throw new InvalidOperationException("throwed in sub Task1");
                //return "sub task1";
            });
            Task<string> subTask2 = new Task<string>(() =>
            {
                return "sub task2";
            });
            subTask1.Start();
            subTask2.Start();
            //使用TaskFactory.ContinueWhenAll()方法,可以独立地处理每个子任务对象
            Task.Factory.ContinueWhenAll(
                new[] { subTask1, subTask2 },  //要等待的子任务数组
                (Task<string>[] tasks) =>       //当子任务全部完成时，tasks参数引用这一任务数组
                {
                    foreach (var task in tasks)
                    {
                        if (task.IsFaulted)
                        {
                            Console.WriteLine(task.Exception);
                        }
                        else
                        {
                            Console.WriteLine(task.Result);
                        }
                    }

                });

           
        }
    }
}
