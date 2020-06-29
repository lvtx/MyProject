using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace GetResultFromTaskWithoutThreadSync
{
    class Program
    {
        /// <summary>
        /// 用于保存处理结果的共享资源
        /// </summary>
        static long result = 0;
       

        static void Main(string[] args)
        {
            //需要并行执行的数据处理函数
            Action<object> ProcessData = delegate(object end)
            {
                long sum = 0;
                for (int i = 1; i <= (int)end; i++)
                    sum += i;
                //保存处理结果
                Interlocked.Exchange(ref Program.result, sum);
               
            };
            //用于取回处理结果的函数
            Action<Task> GetResult=delegate(Task finishedTask)
            {
                if(finishedTask.IsFaulted)
               
                    Console.WriteLine("任务在执行时发生异常：",finishedTask.Exception.Message);
             
                else
                 Console.Write("程序运行结果为{0}", Program.result);
            };
            //创建并行处理数据的任务对象
            Task tskProcess = new Task(ProcessData, 1000000);
            //当数据处理结束时，自动启动下一个工作任务，取回上一任务的处理结果
            Task tskGetResult = tskProcess.ContinueWith(GetResult);

            //开始并行处理数据……
            tskProcess.Start();
           
            Console.ReadKey();

        }
    }
}
