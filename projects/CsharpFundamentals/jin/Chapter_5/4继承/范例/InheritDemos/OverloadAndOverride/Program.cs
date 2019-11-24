using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OverloadAndOverride
{
    class Program
    {
        static void Main(string[] args)
        {
            Parent p = new Child();
            //调用父类方法
            p.OverloadF();
            //调用子类重载的方法
            (p as Child).OverloadF(100);
            ////通过父类变量调用子类方法
            p.OverrideF();

            Console.WriteLine();

            Console.ReadKey();
        }
    }
    class Parent
    {
        public void OverloadF()
        {
            Console.WriteLine("Parent.OverloadF()");
        }
        public virtual void OverrideF()
        {
            Console.WriteLine("Parent.OverrideF()");
        }
    }
    class Child : Parent
    {
       
        public void OverloadF(int i)
        {
            Console.WriteLine("Child.OverloadF({0})", i);
        }
        public override void OverrideF()
        {
            Console.WriteLine("Child.OverrideF()");
        }
    }

}
