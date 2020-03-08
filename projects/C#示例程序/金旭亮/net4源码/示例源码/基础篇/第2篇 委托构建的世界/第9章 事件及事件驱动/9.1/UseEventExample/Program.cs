using System;
using System.Collections.Generic;
using System.Text;

namespace UseEventExample
{
    public delegate void MyEventDelegate(int value);

    //事件源
    public class EventSource
    {
        public event MyEventDelegate handlers; //定义一个事件

        public void FireEvent()
        {
            if (handlers != null)  //如果有响应者
                handlers(10); //激发事件
        }
    }


    //事件响应者
    public class EventResponsor
    {
        //事件处理函数
        public void MyMethod(int i)
        {
            Console.WriteLine(i);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            EventSource p = new EventSource();
            EventResponsor Responsor1 = new EventResponsor();
            EventResponsor Responsor2 = new EventResponsor();
            //声明为事件的委托无法直接调用Combine方法
            //以下两句将无法通过编译
            //p.handlers = System.Delegate.Combine(p.handlers, new MyEventDelegate(Responsor1.MyMethod)) as MyEventDelegate;
            //p.handlers = System.Delegate.Combine(p.handlers, new MyEventDelegate(Responsor2.MyMethod)) as MyEventDelegate;
            //必须使用＋＝运算符给事件追加委托
            p.handlers += new MyEventDelegate(Responsor1.MyMethod);
            p.handlers += new MyEventDelegate(Responsor2.MyMethod);
            //声明为事件的委托也不能直接调用，下面这句无法通过编译
            //p.handlers(10);
            //只能通过类的公有方法间接地引发事件
            p.FireEvent();

            Console.ReadKey();

        }
    }
}
