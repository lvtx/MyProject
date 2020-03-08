using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ThreadInterrupt
{
    class NotAllowedToSleep
    {
        bool _CanSleep = false;
        /// <summary>
        /// 通知线程停止等待
        /// </summary>
        public bool CanSleep
        {
            set { _CanSleep = value; }
        }

        /// <summary>
        /// 线程函数
        /// </summary>
       public void ThreadMethod()
        {
            Console.WriteLine("\n工作线程:本线程正在执行线程函数...");
            while (!_CanSleep)
            {

                // 插入几条什么事也不做的循环指令让CPU耗费一点时间
                Thread.SpinWait(10000000);
            }
            try
            {
                Console.WriteLine("工作线程:本线程准备休眠了...");

                //休眠开始，但这时如果主线程调用了Interrupt()方法，休眠将提前会终止
                //CLR为此线程抛出一个ThreadInterruptedException异常
                Thread.Sleep(Timeout.Infinite);
            }
            catch (ThreadInterruptedException)
            {
                Console.WriteLine("工作线程:主线程通知我不能睡觉了，我得起床继续干活...");
            }
           
            Thread.SpinWait(10000000);
            Console.WriteLine("工作线程:活干完了，本线程中止运行，功成身退!\n");

        }
    }
}
