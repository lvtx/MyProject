using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace UseTimerCallback
{
    //用于向回调函数提供参数信息
    class TaskInfo
    {
        public int count = 0;
    }

    class Program
    {
        static void Main(string[] args)
        {
            System.Console.WriteLine("敲任意键结束……");
            TaskInfo ti = new TaskInfo();
            //创建Timer对象，将一个回调函数传给它，每隔一秒调用一次
            Timer tm = new Timer(ShowTime, ti, 0, 1000);
            System.Console.ReadKey();
            tm.Dispose();

        }
        //被回调的函数
        static void ShowTime(Object ti)
        {
            TaskInfo obj = ti as TaskInfo;
            obj.count++;
            System.Console.WriteLine("({0}){1}", obj.count, DateTime.Now);
        }
    }
}
