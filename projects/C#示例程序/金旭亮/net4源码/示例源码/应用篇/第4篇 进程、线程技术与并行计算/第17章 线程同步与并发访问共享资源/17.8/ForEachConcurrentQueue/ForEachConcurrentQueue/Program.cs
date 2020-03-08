using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;
using System.Threading;

namespace ForEachConcurrentQueue
{
    class Program
    {

        static void Main(string[] args)
        {
            var queue = new ConcurrentQueue<string>();
            Action FillQueue = () =>
                {
                    for (int i = 0; i < 10; i++)
                    {
                        Thread.Sleep(200);
                        queue.Enqueue("String" + i.ToString());
                    }
                    Console.WriteLine("Finished  Filling");
                };

            Thread th = new Thread(new ThreadStart(FillQueue));
            th.Start();
            Thread.Sleep(3000);
            foreach (string str in queue)
            {
                Console.WriteLine(str);
               
            }
           
            Console.ReadKey();

        }
    }
}
