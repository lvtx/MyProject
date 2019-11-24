using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseMultiDelegate
{
    //定义事件委托
    public delegate void MyEventDelegate(int value);
    class Program
    {
        static void Main(string[] args)
        {
            Publisher p = new Publisher();
            Subscriber s1 = new Subscriber();
            Subscriber s2 = new Subscriber();
            p.myEvent += s1.Method;
            p.myEvent += s2.Method;
            Console.WriteLine("\n直接调用委托变量触发事件MyEvent\n");
            p.myEvent(new Random().Next(1,100));
            Console.ReadLine();
        }
    }
    //事件发布者
    class Publisher
    {
        public Publisher()
        {
            Console.WriteLine("Publisher{0}创建",this.GetHashCode());
        }
        public MyEventDelegate myEvent;
    }
    //事件发布者
    class Subscriber
    {
        public Subscriber()
        {
            Console.WriteLine("Subscriber{0}创建",this.GetHashCode());
        }
        //事件触发时的回调方法
        public void Method(int value)
        {
            Console.WriteLine("Subscriber对象{0}响应MyEvent事件：value={1}",
                this.GetHashCode(),
                value);
        }
    }
}
