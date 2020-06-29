using System;
using System.Collections.Generic;
using System.Text;

namespace ObjectCounter
{
    class ObjectCounter
    {
        public static int counter=0;//对象计数器
        public ObjectCounter()
        {
            counter++; //累加读数
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            for (int i = 0; i < 100; i++)
                new ObjectCounter();
            Console.WriteLine("已创建了" + ObjectCounter.counter + "个对象。");
            Console.ReadKey();
        }
    }
}
