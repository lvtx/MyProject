using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace ThreadAbort
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Console.WriteLine("主线程开始");
            //创建线程对象
            MyThread obj = new MyThread();
            Thread th = new Thread(obj.SomeLongTask);
            th.IsBackground = true;
            th.Start();//启动线程
            Thread.Sleep(300);  //主线程休眠0.3秒
            System.Console.WriteLine("主线程调用Abort方法提前中止辅助线程……");
            th.Abort();  //提前中止线程
            System.Console.WriteLine("主线程结束");
            Console.ReadKey();

        }
    }

    class MyThread
    {
        public void SomeLongTask()
        {
            try
            {
                System.Console.WriteLine("辅助线程开始...");
                for (int i = 0; i < 10; i++)
                {
                    System.Console.WriteLine(i);
                    Thread.Sleep(200);
                }
            }
            catch (ThreadAbortException e)
            {
                System.Console.WriteLine("辅助线程被提前中断:{0}", e.Message);
                Thread.ResetAbort();  //不加此句，CLR会再次抛出ThreadAbortException，从而导致函数最后一句代码不会执行。
            }
            finally
            {
                System.Console.WriteLine("完成清理辅助线程占用的资源工作");
            }
            //如果前面没有Thread.ResetAbort()，则程序流程不会执行到此句。
            System.Console.WriteLine("辅助线程结束");
        }
    }
}
