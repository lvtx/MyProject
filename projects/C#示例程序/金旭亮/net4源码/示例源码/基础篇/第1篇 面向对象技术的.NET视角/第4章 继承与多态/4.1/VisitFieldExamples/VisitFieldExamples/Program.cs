using System;
using System.Collections.Generic;
using System.Text;

namespace VisitFieldExamples
{
    class Parent
    {
        public int i = 100;

    }
    class Child : Parent
    {
        public new int i = 200;
    }

    class Program
    {
        static void Main(string[] args)
        {
            Child c = new Child();

            Console.WriteLine(c.i); //200

            Parent p = new Parent();

            Console.WriteLine(p.i);//100

            p = c;  //父类变量引用子类对象

            Console.WriteLine(p.i); //100


            Console.ReadKey();
        }
    }
}
