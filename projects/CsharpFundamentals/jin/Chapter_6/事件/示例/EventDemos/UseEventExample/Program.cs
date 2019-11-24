using System;

namespace UseEventExample
{
    //定义事件委托
    public delegate void MyEventDelegate(int value);

    //事件发布者类
    public class Publisher
    {
        public Publisher()
        {
            Console.WriteLine("Publisher对象{0}创建",this.GetHashCode());
        }
        //使用C#的event关键字定义一个事件
        public event MyEventDelegate MyEvent;
        //激发事件
        public void FireEvent(int EventArgu)
        {
            if (MyEvent != null)
            {
                Console.WriteLine("Publisher对象{0}触发事件，事件参数：{1}",
                    this.GetHashCode(),EventArgu);
                MyEvent(EventArgu);
            }
        }
    }


    //事件响应者类
    public class Subscriber
    {
        public Subscriber()
        {
            Console.WriteLine("Subscriber对象{0}创建", this.GetHashCode());
        }
        //事件触发时的回调方法
        public void MyMethod(int value)
        {
            Console.WriteLine("MyEvent事件触发：value={0},响应者：{1}",
                value, this.GetHashCode());
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Publisher p = new Publisher();
            Console.WriteLine();
            Subscriber s1 = new Subscriber();
            Subscriber s2 = new Subscriber();
            //挂接事件响应代码
            p.MyEvent += s1.MyMethod;
            p.MyEvent += s2.MyMethod;


            //委托变量MyEvent前有一个event关键字，
            //所以无法直接调用Publisher类的MyEvent方法
            //以下代码无法通过编译
            //p.MyEvent(100);

            //只能通过Publisher类的公有方法间接地触发事件
            Console.WriteLine("\n通过Publisher对象的公有方法触发事件\n");
            p.FireEvent(new Random().Next(1, 100));

            Console.ReadKey();

        }
    }
}
