using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ThreadCreate
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("主线程ID：{0}",Thread.CurrentThread.ManagedThreadId);
            MyClass obj = new MyClass();
            //原始写法
            Thread th1 = new Thread(new ThreadStart(MyClass.StaticMethod));
            Thread th2 = new Thread(new ThreadStart(obj.InstanceMethod));

            //简化写法
            Thread th3 = new Thread(MyClass.StaticMethod);
            Thread th4 = new Thread(obj.InstanceMethod);

            //Lambda表达式写法
            Thread th5 = new Thread(() =>
            {
               Console.WriteLine("线程{0}执行Lambda表达式", Thread.CurrentThread.ManagedThreadId);
            });

            //启动3个线程运行
            th1.Start();
            th2.Start();
            th5.Start();
            Console.WriteLine("主线程{0}任务执行完毕，敲任意键退出", Thread.CurrentThread.ManagedThreadId);
            Console.ReadKey();
        }
    }
}
