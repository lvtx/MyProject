using System;
using System.Collections.Generic;
using System.Text;

namespace Constructor
{
    class MyClass
    {
        private int i;
        public MyClass()
        {
            Console.WriteLine("调用类的默认构造函数");
        }
        public MyClass(int iValue)
        {
            i = iValue;
            Console.WriteLine("调用类的构造函数MyClass(int)");
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            MyClass obj1 = new MyClass();
            MyClass obj2 = new MyClass(1);
            Console.ReadKey();
        }
    }
}
