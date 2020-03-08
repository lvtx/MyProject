using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

//经典的生产者与消费者问题
namespace ProductorAndConsumer
{
    class Program
    {

        static void Main(string[] args)
        {
            Console.WriteLine("稍等一会，程序将自动演示，敲任意键退出...\n");
            Console.WriteLine("====================================");
            Producer p = new Producer();
            Consumer c=new Consumer();

            Thread tp = new Thread(p.Produce);
            tp.IsBackground = true;
            Thread tc = new Thread(c.Buy);
            tc.IsBackground = true;
            tp.Start();
            tc.Start();

            System.Console.ReadKey();
        }

        public static Shop drugstore=new Shop(); //某杂货店 

        //线程同步事件
        public static ManualResetEvent   mre = new ManualResetEvent  (false  );
        
    }

    //存放商品的商店
    class Shop
    {
        public string goods=""; //商店中的商品

    }

    //生产者
    class Producer
    {
        //生产商品
        private int count=0;
        public void Produce()
        {
            while(true)
            {
                Thread.Sleep(new Random().Next(100, 5000));
                Monitor.Enter(Program.drugstore);
                
                    if (Program.drugstore.goods == "")
                    {
                        count++;
                        string info = "新产品" + count.ToString();
                        Program.drugstore.goods = info;
                        System.Console.WriteLine("生产者生产:\t" + info); 
                        Monitor.Exit(Program.drugstore);
                    }
                    else
                    {
                             Monitor.Exit(Program.drugstore);
                        Program.mre.WaitOne();
                    }
                
                Program.mre.Set ();//通知消费者
            }
        }


    }
    //消费者
    class Consumer
    {
        //购买商品
        public void Buy()
        {
            while (true)
            {
                Thread.Sleep(new Random().Next(100,5000));
                Monitor.Enter(Program.drugstore);
                

                    if (Program.drugstore.goods != "")
                    {
                        System.Console.WriteLine("消费者买走:\t" + Program.drugstore.goods);
                        Program.drugstore.goods = "";
                        Monitor.Exit(Program.drugstore);
                    }
                    else
                    {
                        Monitor.Exit(Program.drugstore);
                        Program.mre.WaitOne();
                    }
                
                Program.mre.Set ();
            }
        }

    }
}
