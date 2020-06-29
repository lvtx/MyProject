using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace ParallelLoopCancel
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("敲任意键取消并行循环...");

            CancellationTokenSource cts=new CancellationTokenSource();

            Task tsk = new Task(() =>
                {
                    Console.ReadKey(true);  
                    cts.Cancel();
                }
            );
            tsk.Start();

            ParallelOptions opt=new ParallelOptions { CancellationToken=cts.Token};

             Action<int> body=delegate(int index)
            {
                opt.CancellationToken.ThrowIfCancellationRequested();
                Console.WriteLine("并行循环{0}", index);
                Thread.Sleep(1000);
                opt.CancellationToken.ThrowIfCancellationRequested();
            };
             ParallelLoopResult result = new ParallelLoopResult();
            try
            {
              result=  Parallel.For(0, 100, opt, body);
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("操作被取消。");
            }
            Console.WriteLine("ParallelLoopResult.IsCompleted={0}",result.IsCompleted);
           
                  
           

            Console.ReadKey();

        }
    }
}
