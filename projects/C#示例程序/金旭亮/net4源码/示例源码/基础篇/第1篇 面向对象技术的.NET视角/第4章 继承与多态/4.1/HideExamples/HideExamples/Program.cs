using System;
using System.Collections.Generic;
using System.Text;

namespace HideExamples
{
    class Parent
    {
        public void HideF()
        {
            Console.WriteLine("Parent.HideF()");
        }
    }
    class Child : Parent
    {
         public new void HideF()
        {
            Console.WriteLine("Child.HideF()");
           // base.HideF(); //调用基类被覆盖的方法
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            Child c = new Child();

            c.HideF ();//调用父类的还是子类的同名方法？

            Parent p = new Parent();

            p.HideF();//调用父类的还是子类的同名方法？

            p = c;

            p.HideF();//调用父类的同名方法

            ((Child)p).HideF();//调用子类的同名方法


            Console.ReadKey();
        }
    }
}
