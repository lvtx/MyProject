using System;
using System.Collections.Generic;
using System.Text;

namespace JITCompileExample
{
    class MyClassExample
    {
        public void f()
        {
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            MyClassExample obj = new MyClassExample();
            obj.f();
        }
    }
}
