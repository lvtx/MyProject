using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;
using System.Threading;

namespace ProductorAndConsumerUseCDS
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("稍等一会，程序将自动演示，敲任意键退出...\n");
            Console.WriteLine("====================================");
            var Store = new BlockingCollection<string>(1);

            Action Produce = () =>
                {
                    int count = 1;
                    while (true)
                    {
                        Thread.Sleep(3000);
                        string Article = "产品" + count.ToString();
                        Store.Add(Article);
                        Console.WriteLine("生产者生产：" + Article);
                        count++;
                    }
                };

            Action Consume = () =>
                {
                    
                    while (true)
                    {
                       
                       Console.WriteLine("消费者买走：" + Store.Take());
                    }
                };

            Thread tp = new Thread(new ThreadStart(Produce));
            tp.IsBackground = true;
            tp.Start();
            Thread tc = new Thread(new ThreadStart(Consume));
            tc.IsBackground = true;
            tc.Start();
            Console.ReadKey();
        }
    }
}
