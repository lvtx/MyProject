using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using System.Threading;

namespace VisitBlockingCollection
{
    class Program
    {
        static void Main(string[] args)
        {
            var bc = new BlockingCollection<int>();
          
            Action Produce = () =>
                {
                    int i = 0;
                    while (true)
                    {
                        Thread.Sleep(1000);
                        bc.Add(i++);
                    }
                };


            Action Consume = () =>
                {
                    foreach (var value in bc.GetConsumingEnumerable())
                    {
                        Console.WriteLine(value);
                    }
                };

            Thread tp=new Thread(new ThreadStart(Produce));
            Thread tc=new Thread(new ThreadStart(Consume));
            tp.IsBackground=true;
            tc.IsBackground=true;
            tp.Start();
            tc.Start();
    
            Console.ReadKey();
        }
    }
}
