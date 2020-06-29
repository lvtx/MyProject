using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace UseEventListExample
{

    public delegate void OneDelegate(int value);
    public delegate void TwoDelegate(String str);

    public class A
    {
        private EventHandlerList events= new EventHandlerList();

        public event OneDelegate Event1
        {
            add
            {
                events.AddHandler("Event1", value);
            }
            remove
            {
                events.RemoveHandler("Event1",value);
            }
        }

        public event TwoDelegate Event2
        {
            add
            {
                events.AddHandler("Event2", value);
            }
            remove
            {
                events.RemoveHandler("Event2",value);
            }

        }

        public void FireTwoEvents()
        {
            //对于事件访问器，不能直接这样激发事件
            //Event1(100);
            //Event2("100");

            (events["Event1"] as OneDelegate)(100);
            (events["Event2"] as TwoDelegate )("100");

        }
    }

    public class B
    {
        public void f1(int i)
        {
            Console.WriteLine("B.f1(int)");
        }
        public void f2(String str)
        {
            Console.WriteLine("B.f2(String)");
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            B b = new B();
            A a = new A();
            a.Event1 += b.f1;
            a.Event2 += b.f2;
            a.FireTwoEvents();

            Console.ReadKey();

        }
    }
}
