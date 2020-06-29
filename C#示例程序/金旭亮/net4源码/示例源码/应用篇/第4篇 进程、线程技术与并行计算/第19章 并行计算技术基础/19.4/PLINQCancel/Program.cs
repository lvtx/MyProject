using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PLINQCancel
{
    class Program
    {
        static void Main(string[] args)
        {
            int[] source = Enumerable.Range(1, 10000000).ToArray();
            CancellationTokenSource cs = new CancellationTokenSource();
            //在另一个线程中取消操作
            Task.Factory.StartNew(() =>
            {
                CancelPLINQ(cs);
            });
            int[] results = null;
            try
            {
                results = (from num in source.AsParallel().WithCancellation(cs.Token)
                           where num % 3 == 0
                           orderby num descending
                           select num).ToArray();
            }
            catch (OperationCanceledException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (AggregateException ae)
            {
                if (ae.InnerExceptions != null)
                {
                    foreach (Exception e in ae.InnerExceptions)
                        Console.WriteLine(e.Message);
                }
              

            } 
            Console.WriteLine();
            Console.ReadKey();
        }

        /// <summary>
        /// 中途取消PLINQ查询操作
        /// </summary>
        /// <param name="cs"></param>
        static void CancelPLINQ(CancellationTokenSource cs)
        {
            Random rand = new Random();
            Thread.Sleep(rand.Next(150, 350));
            cs.Cancel();
        }

    }
}
