using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hides
{
    class Parent
    {
        public int value = 100;
        public void HideF()
        {
            System.Console.WriteLine("Parent.HideF()");
        }
    }
    class Child : Parent
    {
        public int value = 200;
        public void HideF()
        {
            System.Console.WriteLine("Child. HideF()");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Parent p = new Parent();
            Child c = new Child();

            //p.HideF();
            //c.HideF();

            p = c;  //基类变量引用子类对象
            p.HideF();  //会输出什么结果？
            (p as Child).HideF();//会输出什么结果？

            //以下代码，输出哪个值？
            //Console.WriteLine(p.value);
            //Console.WriteLine((p as Child).value);

            Console.ReadKey();

        }
    }
}
