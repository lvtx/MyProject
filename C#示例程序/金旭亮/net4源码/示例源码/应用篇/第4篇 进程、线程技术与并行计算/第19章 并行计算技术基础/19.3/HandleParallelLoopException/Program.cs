using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HandleParallelLoopException
{
    class Program
    {
        static void Main(string[] args)
        { 
            Random ran = new Random();
            //并行循环体函数
            Action<int, ParallelLoopState> LoopBody =
                (i, state) =>
                {
                    Console.WriteLine("线程{0}在执行并行循环，循环变量={1}",Thread.CurrentThread.ManagedThreadId,i);
                    Thread.Sleep(ran.Next(10, 1000));

                    if (state.IsExceptional)
                    {
                        Console.WriteLine("检测到其他线程发生了异常,本线程{0}终止执行", Thread.CurrentThread.ManagedThreadId);
                        state.Stop();
                        return;
                    }
                   

                    
                    int flag = ran.Next(1, 100);
                    if (flag % 9 == 0)
                    {
                        Console.WriteLine("线程{0}主动抛出异常", Thread.CurrentThread.ManagedThreadId);
                        throw new OperationCanceledException();
                       
                    }
                  
                };

            //并行执行的循环
            ParallelLoopResult ret=new ParallelLoopResult();
            try
            {
            ret = Parallel.For(0, 100, LoopBody);
            Console.WriteLine("并行循环结束");
            }
            catch (AggregateException ae)
            {

                ae.Flatten();
                foreach (Exception ex in ae.InnerExceptions)
                {
                    Console.WriteLine("{0}:{1}",ex.GetType(),ex.Message);
                }

            }

            Console.WriteLine("ret.IsCompleted={0}",ret.IsCompleted);
            if(ret.LowestBreakIteration.HasValue)
                Console.WriteLine("ret.LowestBreakIteration={0}",ret.LowestBreakIteration);
            else
                Console.WriteLine("ret.LowestBreakIteration=null");
          
           Console.ReadKey();

        }
    }
}
