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

        static void Main(string[] args)
        {
            //需要并行执行的数据处理函数
            Func<object, long> ProcessData = (end) =>
            {
                long sum = 0;
                for (int i = 1; i <= (int)end; i++)
                {
                    sum += i;
                    //如果取消以下这句注释，会看到其异常被传播到后继任务中。
                    throw new DivideByZeroException();
                }
                return sum;
            };
            //用于取回处理结果的函数
            Action<Task<long>> GetResult = (finishedTask) =>
            {
                //依据任务状态，决定后继处理工作
                if (finishedTask.IsFaulted)
                    Console.WriteLine("任务在执行时发生异常：{0}", finishedTask.Exception);
                else
                    Console.Write("程序运行结果为{0}", finishedTask.Result);
            };

            //创建并行处理数据的任务对象
            Task<long> tskProcess = new Task<long>(ProcessData, 1000000);

            //当数据处理结束时，自动启动下一个工作任务，取回上一任务的处理结果
            Task tskGetResult = tskProcess.ContinueWith(GetResult);

            //开始并行处理数据……
            tskProcess.Start();

            Console.ReadKey();

        }
    }
}
