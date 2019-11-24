using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ObjectComparer
{
    class Program
    {
        static void Main(string[] args)
        {
            MyClass[] objs = new MyClass[]
            {
                new MyClass{Value=124},
                new MyClass{Value=100},
                new MyClass{Value=1},
                new MyClass{Value=75}
            };
            Console.WriteLine("原始数组：");
            Array.ForEach<MyClass>(objs, obj => { Console.Write("Value={0} ,", obj.Value); });

            Array.Sort<MyClass>(objs, new MyClassComparer());

            Console.WriteLine("\n排序之后：");
            Array.ForEach<MyClass>(objs, obj => { Console.Write("Value={0} ,", obj.Value); });

            Console.ReadKey();
        }
    }
}
