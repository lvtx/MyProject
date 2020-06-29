using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SortObjectArray
{

    class Program
    {
        static void Main(string[] args)
        {
            MyClass[] Objs = new MyClass[10];

            Random ran=new Random();
            for (int i = 0; i < 10; i++)
                Objs[i] = new MyClass { Value = ran.Next(1, 100) };

            Comparison<MyClass> WhoIsGreater = delegate(MyClass x, MyClass y)
            {
                if (x.Value > y.Value)
                    return 1;
                else
                    if (x.Value == y.Value)
                        return 0;
                    else
                        return -1;
            };

            Array.Sort<MyClass>(Objs,WhoIsGreater);

            Array.ForEach<MyClass>(Objs,(obj)=>{Console.WriteLine(obj.Value);});

            Console.ReadKey();
        }
    }

    class MyClass
    {
        public int Value;
    }
}
