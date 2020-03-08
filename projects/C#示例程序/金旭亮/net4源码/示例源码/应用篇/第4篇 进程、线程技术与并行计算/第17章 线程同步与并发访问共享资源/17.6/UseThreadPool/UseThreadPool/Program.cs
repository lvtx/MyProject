using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;


namespace UseThreadPool
{
    class Program
    {
        static void Main(string[] args)
        {
            //创建工作任务信息对象
            TaskInfo ti = new TaskInfo() { EndNumber = 10000 };
           
            //将线程函数加入线程池的等待队列
            ThreadPool.QueueUserWorkItem(CalculateSum, ti);
            are.WaitOne();//等待线程池通知工作已完成
            System.Console.WriteLine("从1加到{0}的和为{1}", ti.EndNumber, ti.Sum);
            System.Console.ReadKey();
        }

        //同步信号
       static  AutoResetEvent are = new AutoResetEvent(false);

        //线程函数
        static void CalculateSum(Object argu)
        {
            TaskInfo ti=argu  as TaskInfo;
            ti.Sum = 0;
            for (int i = 1; i <= ti.EndNumber; i++)
                ti.Sum += i;
            are.Set();//通知主线程工作完成
        }


    }
    class TaskInfo
    {
        public int EndNumber=0; //要累加到的数
        public int Sum=0;       //累加结果
    }
}
