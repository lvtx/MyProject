using System;
using System.Collections.Generic;
using System.Text;

namespace UseMultiDelegateExample
{
    //定义事件委托
    public delegate void MyEventDelegate(int value );

    //事件发布者类
    public class  Publisher 
    {
        public Publisher()
        {
            Console.WriteLine("Publisher对象{0}创建", this.GetHashCode());
        }
        //利用多路委托变量保存多个事件响应者方法引用
        public MyEventDelegate MyEvent; 
    }

   
    //事件响应者类
    public class Subscriber
    {
        public Subscriber()
        {
            Console.WriteLine("Subscriber对象{0}创建",this.GetHashCode());
        }
        //事件触发时的回调方法
        public void MyMethod(int value)
        {
            Console.WriteLine("Subscriber对象{0}响应MyEvent事件：value={1}",
                this.GetHashCode(),
                value);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            //一个事件源对象
            Publisher p = new Publisher();
            Console.WriteLine();
            //两个事件响应者
            Subscriber s1 = new Subscriber();
            Subscriber s2 = new Subscriber();
            
            //挂接事件响应方法
            p.MyEvent += s1.MyMethod;
            p.MyEvent += s2.MyMethod;

            Console.WriteLine("\n直接调用委托变量触发事件MyEvent\n");
            //直接调用委托变量，代表触发事件
            p.MyEvent(new Random().Next(1,100));

            Console.ReadKey();
        }
    }
}
