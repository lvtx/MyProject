using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ThreadCreate
{
    public class MyClass
    {
        public static void StaticMethod()
        {
            Console.WriteLine("线程{0}执行MyClass.StaticMethod",Thread.CurrentThread.ManagedThreadId);
        }
        public void InstanceMethod()
        {
            Console.WriteLine("线程{0}执行MyClass.InstanceMethod", Thread.CurrentThread.ManagedThreadId);
        }
    }
}
