using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Inherits
{
    class Parent
    {
        public int Pi=100;
        public void Pf()
        {
            Console.WriteLine("Parent.Pf()");
        }
        protected int Pj=200;
        protected void Pg()
        {
            Console.WriteLine("Parent.Pg()");
        }
        private int k = 300;
    }

    class Child:Parent
    {
        public void cf()
        {
            Pg(); //子类可以访问父类的保护成员
            Pj += 200;
            Console.WriteLine("Child.cf()");
            Console.WriteLine("Parent.Pj=" + Pj.ToString());

        }
    }


    class Program
    {
        static void Main(string[] args)
        {
            Child c = new Child();
            //可以通过子类变量访问定义在基类的公有成员
            c.Pi = 300;
            Console.WriteLine("Parent.Pi=" + c.Pi.ToString());
            c.Pf();
            //c.Pj = 1000; //Error!不能访问保护级别的成员
            c.cf(); //可以通过子类定义的公有方法访问基类保护级别的成员
            Console.ReadKey();
        }
    }
}
