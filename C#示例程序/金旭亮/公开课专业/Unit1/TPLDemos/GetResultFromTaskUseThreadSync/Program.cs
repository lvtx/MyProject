using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GetResultFromTask
{
    class Program
    {

        /// <summary>
        /// 用于保存处理结果的共享资源
        /// </summary>
        static long result = 0;

        /// <summary>
        /// 用于通知启动任务的线程处理工作已完成
        /// </summary>
        private static  ManualResetEvent mre = new ManualResetEvent(false);

        static void Main(string[] args)
        {
            Action<object> TaskMethod = (end)=>
            {
                long sum=0;
                for (int i = 1; i <= (int)end; i++)
                    sum += i;
                //保存处理结果(使用Interlocked实现原子操作，无需加锁）
                Interlocked.Exchange(ref Program.result, sum);
                //通知调用者，工作己经完成，你可以取回结果了
                mre.Set();
            };
            
            //启动异步任务
            Task tsk = new Task(TaskMethod, 1000000);
            tsk.Start();

            //等待并行处理的完成以取回结果
            mre.WaitOne();

            Console.Write("程序运行结果为{0}", Program.result);
            Console.ReadKey();

        }
    }
}
