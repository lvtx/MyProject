using System;
using System.Collections.Generic;
using System.Text;

namespace TestVirtualMethod
{

    class Parent
    {
        public virtual void virtualMtd()
        {
        }
    }

    class Child : Parent
    {
        public override void virtualMtd()
        {
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Parent p = new Parent();
            p.virtualMtd(); 	//通过基类变量调用虚方法
            Child c = new Child();
            c.virtualMtd(); //通过子类变量调用虚方法
            p = c;
            p.virtualMtd(); //再次通过基类变量调用虚方法

        }
    }
}
