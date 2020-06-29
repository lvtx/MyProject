using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace IsDynamicSlowly
{
    class MyClass
    {
        public void Fun()
        {
            Console.WriteLine("Doing work...");
        }
    }
    class Program
    {
        [DllImport("Kernel32.dll")]
        private static extern int QueryPerformanceCounter(out Int64 count);
        static void Main(string[] args)
        {
            MyClass obj = new MyClass();
            Stopwatch sw = new Stopwatch();
            dynamic d = obj;
            
            for (int i = 1; i <= 5; i++)
            {
                Int64 start, end;
                QueryPerformanceCounter(out start);
                //sw.Restart();
                d.Fun();
                //sw.Stop();
                QueryPerformanceCounter(out end);
                Console.WriteLine("第{0}次调用所用时间：{1} Ticks",i,end-start);
            }
            Console.ReadKey();
        }

       
    }
}
