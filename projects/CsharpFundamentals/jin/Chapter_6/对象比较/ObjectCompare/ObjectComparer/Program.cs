using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectComparer
{
    class Program
    {
        static void Main(string[] args)
        {
            MyClass[] myClasses = new MyClass[]
            {
                new MyClass(){ value = 5 },
                new MyClass() { value = 10 },
                new MyClass() { value = 3 },
                new MyClass() { value = 9 }
            };
            Console.WriteLine("原始数组");
            Array.ForEach<MyClass>(myClasses, myClass => { Console.WriteLine("value{0},", myClass.value); });
            Array.Sort<MyClass>(myClasses,new MyClassComparer());
            Console.WriteLine("排序之后的数组");
            Array.ForEach<MyClass>(myClasses, myClass => { Console.WriteLine("value{0},", myClass.value); });
            Console.ReadLine();
        }
    }
}
