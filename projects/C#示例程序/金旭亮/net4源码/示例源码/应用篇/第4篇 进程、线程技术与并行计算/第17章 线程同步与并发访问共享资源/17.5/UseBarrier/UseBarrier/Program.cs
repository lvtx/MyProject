using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace UseBarrier
{
    class Program
    {
        public static Barrier barrier = null;
        static void Main(string[] args)
        {
          
            Action<Barrier> PhaseAction = delegate(Barrier barier)
            {
               
                switch (barrier.CurrentPhaseNumber)
                {
                    case 0:
                        Console.WriteLine("\n===============================");
                        Console.WriteLine("\n同步阶段一：张三、李四和王五都到达了鸟巢，开始参观……");
                   
                        Console.WriteLine("\n===============================");
                        break;
                    case 1:
                        Console.WriteLine("\n===============================");
                        Console.WriteLine("\n同步阶段二：参观结束，张三、李四和王五开始回家……");
           
                        Console.WriteLine("\n===============================");
                        break;
                    case 2:
                        Console.WriteLine("\n===============================");
                        Console.WriteLine("\n同步阶段三：所有人都回到了家，啊，多么快乐的一天！\n");
   
                        Console.WriteLine("\n===============================");
                        break;

                }

            };
            //有3个线程参与
            barrier = new Barrier(3, PhaseAction);

            //创建3个参与者对象
            List<IParticipant> Participants = new List<IParticipant>
            {
                new ZhangSan(),
                new LiSi(),
                new WangWu()
            };
            Console.WriteLine("敲任意键Barrier示例开始演示...");
            Console.ReadKey(true);
            Console.WriteLine("\n===============================");

            //启动3个线程
            List<Thread> ths = new List<Thread>();
            foreach (IParticipant man in Participants)
            {
                Thread th = new Thread(man.Go);
                ths.Add(th);
                th.Start();
            }
            //等待所有线程运行结束
            foreach (Thread th in ths)
            {
                th.Join();
            }

            Console.WriteLine("敲任意键退出……");
            Console.ReadKey();

        }
    }
}
