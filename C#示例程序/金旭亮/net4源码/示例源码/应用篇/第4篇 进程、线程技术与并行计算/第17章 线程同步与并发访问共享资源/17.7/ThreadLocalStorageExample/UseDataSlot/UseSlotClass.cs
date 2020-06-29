using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace UseDataSlot
{
    public class UseSlotClass
    {
        static Random randomGenerator = new Random();

        public static void SlotTest()
        {
            // 在每个线程的数据槽中放置不同的数据
            Thread.SetData(Thread.GetNamedDataSlot("Random"),
                randomGenerator.Next(1, 200));

            //在控制台窗口中输出TLS中的内容
            Console.WriteLine("线程{0}的数据槽中保存的数据为: {1,3}",
                Thread.CurrentThread.ManagedThreadId,
                Thread.GetData(Thread.GetNamedDataSlot("Random")));


           //当前线程休眠一秒钟，让其他线程也能执行Thread.SetData()方法，从而展示出每个线程的数据槽都是独立的
            Thread.Sleep(1000);

            Console.WriteLine("休眠之后，线程{0}的数据槽中保存的数据仍为: {1,3}，表明各线程之间的数据槽是相互独立的",
                Thread.CurrentThread.ManagedThreadId,
                Thread.GetData(Thread.GetNamedDataSlot("Random")));

            //当前线程休眠一秒钟
            Thread.Sleep(1000);
            //使用位于另一个类中的方法，在此方法中可以修改本线程数据槽中的数据，因为这些代码运行在同一线程时
            OtherClass o = new OtherClass();
            o.ModifySlotData();

            Console.WriteLine("调用OtherClass对象的ModiftySlotData方法修改数据槽中的数据之后，线程{0}的数据槽中保存的数据为： {1,3}",
               Thread.CurrentThread.ManagedThreadId,
               Thread.GetData(
               Thread.GetNamedDataSlot("Random")));
          

        }


    }
}
