using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace UseThreadStatic
{
    class Program
    {
        static void Main(string[] args)
        {
            for (int i = 0; i < 3; i++)
            {
                Thread newThread = new Thread(ThreadData.ThreadStaticDemo);
                newThread.Start();
                //错开线程创建的时间
                Thread.Sleep(1000);
                
            }

            Console.ReadKey();
        }
    }
}
