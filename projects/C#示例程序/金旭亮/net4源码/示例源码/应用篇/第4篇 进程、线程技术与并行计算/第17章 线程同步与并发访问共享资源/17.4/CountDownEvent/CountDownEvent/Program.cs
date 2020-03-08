using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace CountDownEvent
{
    class Program
    {
        static void Main(string[] args)
        {
            CountdownEvent cde = new CountdownEvent(10);//初始计数值10
            Console.WriteLine("计数初始值：{0}", cde.InitialCount);

            //计数初始值可以被修改，取消以下两句的注释，计数将从20开始。
            //cde.AddCount(10);  //CurrentCount=20
            //Console.WriteLine("人工设置CurrentCount：{0}", cde.CurrentCount);
            int thcount = cde.CurrentCount;
            Console.WriteLine("敲任意键开始计数...");
            Console.ReadKey();
            Console.WriteLine("========================");

            
            //创建CurrentCount个线程，每个线程负责调用一次Signal，给计数器值减1
            for (int i = 0; i < thcount; i++)
            {
                new Thread(new ParameterizedThreadStart(Dummy)).Start(cde);

            }
            //等待计数结束
            cde.Wait();
            Console.WriteLine("计数完成");
            Console.WriteLine("========================");
            Console.ReadKey();
        }

        static void Dummy(object o)
        {
            (o as CountdownEvent).Signal();
            Console.WriteLine("当前计数值:{0}", (o as CountdownEvent).CurrentCount);
            

        }

    }
}
