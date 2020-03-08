using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InvokeParentConstuctor
{
    class Parent
    {
        public Parent()
        {
            System.Console.WriteLine("Parent的默认构造函数被调用");
        }
        public Parent(String info)
        {
            System.Console.WriteLine("Parent.Parent(String)被调用:" + info);
        }
    }
    class Child : Parent
    {
        public Child() //调用父类默认构造函数
        {
            System.Console.WriteLine("Child的默认构造函数被调用");
        }
        public Child(String info)
            : base(info)//调用父类重载的构造函数
        {
            System.Console.WriteLine("Child.Child(String)被调用:" + info);
        }
    }


    class Program
    {
        static void Main(string[] args)
        {
            Child c1 = new Child();
            Child c2 = new Child("Hello");
            Console.ReadKey();
        }
    }
}
