using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace UseTimerCallback
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("敲回任意键结束");
            TaskInfo ti = new TaskInfo();
            Timer timer = new Timer(ShowTime, ti, 0, 1000);
            Console.ReadKey();
            timer.Dispose();
        }
        static void ShowTime(object ti)
        {
            TaskInfo obj = (ti as TaskInfo);
            obj.value++;
            Console.WriteLine("{0}.{1}",obj.value,DateTime.Now);
        }
    }
}
