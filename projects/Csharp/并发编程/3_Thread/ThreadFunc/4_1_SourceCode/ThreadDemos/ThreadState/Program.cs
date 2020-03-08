using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ThreadState
{
    class Program
    {
        static void Main(string[] args)
        {
            Thread thread = new Thread(() =>
            {
                Console.WriteLine(Thread.CurrentThread.ThreadState);
            });
            Console.WriteLine(thread.ThreadState);
            thread.Start();
            thread.Join();
            Console.WriteLine(thread.ThreadState);
            Console.ReadKey();


        }
    }
}
