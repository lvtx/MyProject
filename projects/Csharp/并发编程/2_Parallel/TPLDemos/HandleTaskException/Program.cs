using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HandleTaskException
{
    class Program
    {
        static void Main(string[] args)
        {
            //定义3个任务，每个任务都抛出一个异常
            Task task1 = new Task(() =>
                {
                    throw new Exception();
                });
            Task task2= new Task(() =>
            {
                throw new IndexOutOfRangeException();
            });
            Task task3 = new Task(() =>
            {
                throw new DivideByZeroException();
            });
            //创建一个“父”任务，此任务包容着前面创建的3个子任务
            Task taskController = new Task(() =>
                {
                        task1.Start();
                        task2.Start();
                        task3.Start();
                        Task.WaitAll(task1, task2, task3);
                }
            );

            try
            {
                taskController.Start();
                taskController.Wait();
            }
            catch (AggregateException ae)
            {
                //“抹平”整个异常树，如果注释掉此句，则必须递归地遍历整个异常树，才能知道到底发生了哪些异常
                ae=ae.Flatten();
                Console.WriteLine("并行任务一共引发了{0}个异常",ae.InnerExceptions.Count);
                foreach (Exception ex in ae.InnerExceptions)
                {
                    Console.WriteLine("{0}:{1}",ex.GetType(),ex.Message);
                }
            }


            Console.ReadKey();


        }
    }
}
