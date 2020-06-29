using System;
using System.Collections.Generic;
using System.Text;

namespace UseMultiDelegateExample
{
    public delegate void MyEventDelegate(int value );

    //事件源
    public class  EventSource 
    {
        public MyEventDelegate handlers; //事件响应者清单
    }

   
    //事件响应者
    public class EventResponsor
    {
        //事件响应方法
        public void MyMethod(int i )
        {
            Console.WriteLine(i);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            //一个事件源对象
            EventSource p = new EventSource();
            //两个事件响应者
            EventResponsor Responsor1 = new EventResponsor();
            EventResponsor Responsor2 = new EventResponsor();
            //可以直接调用Delegate类的静态方法组合多个委托
            p.handlers = System.Delegate.Combine(p.handlers, new MyEventDelegate(Responsor1.MyMethod)) as MyEventDelegate;
            p.handlers = System.Delegate.Combine(p.handlers, new MyEventDelegate(Responsor2.MyMethod)) as MyEventDelegate;
            //或调用＋＝运算符组合委托
            //p.handlers += new MyEventDelegate(Responsor1.MyMethod);
            //p.handlers += new MyEventDelegate(Responsor2.MyMethod);
            //最简单的写法
            //p.handlers += Responsor1.MyMethod;
            //p.handlers += Responsor2.MyMethod;
            
            //直接调用委托变量，代表激发事件
            p.handlers(10);

            Console.ReadKey();

        }
    }
}
