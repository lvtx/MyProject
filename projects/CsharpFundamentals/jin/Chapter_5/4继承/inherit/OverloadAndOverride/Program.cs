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
            //Parent parent1;
            Child child = new Child();
            SecondChild second = new SecondChild();
            Parent parent = new Child();
            Parent parent1 = (Parent)second;
            parent1.OverrideF(); 
            child.OverrideF();
            parent.OverloadF();//parent overload
            //通过父类调用子类的方法
            parent.OverrideF();//child override
            (parent as Child).OverloadF(10);
            Console.ReadKey();
        }
    }

    class Parent
    {
        public void OverloadF()
        {
            Console.WriteLine("Parent Overload");
        }
        public virtual void OverrideF()
        {
            Console.WriteLine("Parent Override");
        }
    }
    class Child:Parent
    {
        public void OverloadF(int i)
        {
            Console.WriteLine("Child Overload {0}",i);
        }
        public override void OverrideF()
        {
            Console.WriteLine("Child Override");
        }
    }
    class SecondChild : Child
    {
        new public void OverrideF()
        {
            Console.WriteLine("Second Child");
        } 
    }
}
