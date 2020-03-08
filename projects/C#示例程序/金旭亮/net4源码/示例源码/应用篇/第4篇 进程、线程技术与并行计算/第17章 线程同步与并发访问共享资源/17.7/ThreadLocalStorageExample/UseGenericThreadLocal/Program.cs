using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace UseGenericThreadLocal
{
    class Program
    {
        static void Main(string[] args)
        {
            Thread[] ths = new Thread[3];
            for (int i = 0; i < 3; i++)
            {
                ths[i] = new Thread(ThreadFunc);
                ths[i].Start();
            }

            foreach (Thread th in ths)
            {
                th.Join();
            }
            Console.WriteLine("辅助线程结束后,TLSData.Value={0}", TLSData.Value==null?"null":TLSData.Value.GetHashCode().ToString());
            Console.ReadKey();

        }

        static ThreadLocal<MyClass> TLSData = new ThreadLocal<MyClass>();

        static void ThreadFunc()
        {
           
            string InfoStr=string.Format("线程{0}负责创建并拥有此对象",Thread.CurrentThread.ManagedThreadId);

            TLSData.Value = new MyClass { Info = InfoStr };

            Console.WriteLine("TLSData.Value引用对象{0},它的Info属性值：“{1}”", TLSData.Value.GetHashCode(), TLSData.Value.Info);
        }
        
    }

    class MyClass
    {
        public string Info="";
    }
}
