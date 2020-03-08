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
            Action<object> del = delegate(object end)
            {
                long sum=0;
                for (int i = 1; i <= (int)end; i++)
                    sum += i;
                //保存处理结果
                Interlocked.Exchange(ref Program.result, sum);
                mre.Set();
            };

            Task tsk = new Task(del, 1000000);
            tsk.Start();
            //等待并行处理的完成
            mre.WaitOne();
            Console.Write("程序运行结果为{0}", Program.result);
            Console.ReadKey();

        }
    }
}
