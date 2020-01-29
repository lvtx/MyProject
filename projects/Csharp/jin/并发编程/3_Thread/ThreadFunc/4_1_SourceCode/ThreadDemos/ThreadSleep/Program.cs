using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ThreadSleep
{
    class Program
    {
        static void Main(string[] args)
        {
            Thread thread = new Thread(() =>
            {
                for(int i=0;i<10;i++)
                {
                    //修眠0.3秒
                    Thread.Sleep(300);
                    Console.WriteLine(i);
                }

            });
            thread.Start();
            Console.ReadKey();
        }
    }
}
