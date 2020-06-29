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
            //如果在此任务中取消以下异常捕获代码的注释，则3个子任务中的一个所抛出的DivideByZeroException会被“吃掉”
            //外界将不会得知曾发生过这个异常
            Task taskController = new Task(() =>
                {
                    try
                    {

                        task1.Start();
                        task2.Start();
                        task3.Start();
                        Task.WaitAll(task1, task2, task3);
                    }
                    catch (AggregateException ae)
                    {
                        //”吃掉”DivideByZeroException 
                        ae.Handle((ex) =>
                            {
                                if (ex is DivideByZeroException)
                                    return true;
                                else
                                    return false;
                            });
                    }
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

                foreach (Exception ex in ae.InnerExceptions)
                {
                    Console.WriteLine("{0}:{1}",ex.GetType(),ex.Message);
                }
            }


            Console.ReadKey();


        }
    }
}
