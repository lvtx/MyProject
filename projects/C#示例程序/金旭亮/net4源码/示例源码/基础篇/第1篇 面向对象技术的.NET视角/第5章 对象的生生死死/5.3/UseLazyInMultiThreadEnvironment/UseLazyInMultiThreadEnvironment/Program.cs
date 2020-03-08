using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;

namespace UseLazyInMultiThreadEnvironment
{
    class Program
    {
        /// <summary>
        /// 用于多线程共享的对象
        /// </summary>
        static Lazy<MyClass> Obj = null;

        /// <summary>
        /// 用于创建共享对象的工厂函数
        /// </summary>
        static Func<MyClass> valueFactory = delegate()
        {
            Thread.Sleep(new Random().Next(0,100));
            Console.WriteLine("调用工厂函数创建MyClass对象");
            MyClass obj = new MyClass { IntValue = (new Random()).Next(1,100) };
            return obj;
        };

        /// <summary>
        /// 线程函数，将被多个线程同时执行
        /// </summary>
        static void ThreadFunc()
        {            
           Console.WriteLine("对象{0}的IntValue={1}", Obj.Value.GetHashCode(), Obj.Value.IntValue);
        }

        static void Main(string[] args)
        {
            Console.WriteLine("\n敲任意键开始演示，ESC退出...");

            while (Console.ReadKey(true).Key != ConsoleKey.Escape)
            {
                Console.WriteLine();
                //注意将第2个参数改为不同的值：
                //1 None
                //2 PublicationOnly
                //3 ExecutionAndPublication
                //运行看看结果有何不同？
                Obj = new Lazy<MyClass>(valueFactory, LazyThreadSafetyMode.ExecutionAndPublication);
          
                for (int i = 0; i < 10; i++)
                {
                    Thread th = new Thread(ThreadFunc);
                  
                    th.Start();
                }

            }
        }
    }

    class MyClass
    {
        public MyClass()
        {
            Console.WriteLine("MyClass对象创建，其标识：{0}", this.GetHashCode());
        }
        public int IntValue
        {
            get;
            set;
        }
    }

}
