using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseEvent
{
    public delegate void MyEventDelegate(int value);
    class Program
    {
        static void Main(string[] args)
        {
            Publisher p = new Publisher();
            Subscriber s1 = new Subscriber();
            Subscriber s2 = new Subscriber();
            p.myEvent += s1.MyMethod;
            p.myEvent += s2.MyMethod;
            p.FireEvent(new Random().Next(1, 1000));
            Console.ReadKey();
        }
    }
    class Publisher
    {
        public Publisher()
        {
            Console.WriteLine("Publisher对象{0}创建",this.GetHashCode());
        }
        public event MyEventDelegate myEvent;
        public void FireEvent(int EventArgu)
        {
            if(myEvent != null)
            {
                myEvent(EventArgu);
            }
        }
    }
    class Subscriber
    {
        public Subscriber()
        {
            Console.WriteLine("Subscriber对象{0}创建",this.GetHashCode());
        }
        public void MyMethod(int value)
        {
            Console.WriteLine("MyEvent事件触发value={0},响应者:{1}",value,this.GetHashCode());
        }
    }
}
