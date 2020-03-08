using System;
using System.Collections.Generic;
using System.Text;

namespace InterfaceVirutalMethodInvoke
{
    interface IOne
    {
        void f1();
        void f2();
    }
    interface IOther
    {
        void g();
    }

    class MyClass1 : IOne, IOther
    {
        void IOne.f1()
        {
        }
        void IOne.f2()
        {
        }
        void IOther.g()
        {
        }
        //自己定义的其他方法
        public void MyOtherMethod()
        {
        }
    }
    class MyClass2 : IOne, IOther
    {
       
        void IOne.f1()
        {
        }

        void IOne.f2()
        {
        }

        void IOther.g()
        {
        }
        public void DoSomething()
        {
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            InvokeIOtherMethod(new MyClass1());
            InvokeIOtherMethod(new MyClass2());
        }
        //基于IOther接口调用虚拟方法
        static void InvokeIOtherMethod(IOther obj)
        {
            obj.g();
        }
    }
}
