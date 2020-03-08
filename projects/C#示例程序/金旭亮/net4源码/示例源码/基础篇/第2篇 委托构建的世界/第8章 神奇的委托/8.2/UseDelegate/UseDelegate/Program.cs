using System;
using System.Collections.Generic;
using System.Text;

namespace UseDelegate
{
    delegate void MyDelegate(string s);

    class MyClass
    {
        public static void Hello(string s)
        {
            Console.WriteLine("你好, {0}!", s);
        }

        public static void Goodbye(string s)
        {
            Console.WriteLine("再见, {0}!", s);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
             MyDelegate a, b, c, d;

            // 创建引用 Hello 方法的 
            // 委托对象 a：
            a = MyClass.Hello ;
            Console.WriteLine("调用委托变量 a:");
            a("a"); 

            // 创建引用 Goodbye 方法的 
            // 委托对象 b：
            b = MyClass.Goodbye;
            Console.WriteLine("调用委托变量 b:");
            b("b");

            // a 和 b 两个委托合成 c， 
            c = a + b;
            Console.WriteLine("调用委托变量 c:");
            c("c = a + b");  // c 将按顺序调用两个方法

            // 从组合委托c中移除 a ，只留下b，用d代表移除结果， 
            d = c - a;     
            Console.WriteLine("调用委托变量 d:");
            d("d = c - a");// 后者仅调用 Goodbye 方法：

            Console.ReadKey();
        }
    }
}
