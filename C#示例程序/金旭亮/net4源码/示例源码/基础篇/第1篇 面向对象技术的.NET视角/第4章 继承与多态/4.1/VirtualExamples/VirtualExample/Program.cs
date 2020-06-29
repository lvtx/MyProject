using System;
using System.Collections.Generic;
using System.Text;

namespace VirtualExamples
{
    class Parent
    {
        public virtual void OverrideF()
        {
            System.Console.WriteLine("Parent.OverrideF()");
        }


    }
    class Child : Parent
    {
        public override void OverrideF()
        {
            System.Console.WriteLine("Child.OverrideF()");
            
        }


    }
    class Program
    {
        static void Main(string[] args)
        {
            Child c = new Child();

            c.OverrideF();//调用父类的还是子类的同名方法？

            Parent p = new Parent();

            p.OverrideF();//调用父类的还是子类的同名方法？

            p = c;

            p.OverrideF();//调用子类的同名方法

           
            Console.ReadKey();
        }
    }
}
