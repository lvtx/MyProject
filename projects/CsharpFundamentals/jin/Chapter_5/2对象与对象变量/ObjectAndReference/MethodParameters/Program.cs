using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MethodParameters
{
    class MyClass
    {
        public int value = 100;
    }
    class Program
    {
        static void Main(string[] args)
        {
            MyClass obj = new MyClass();
            ModifyValue(obj.value);
            Console.WriteLine(obj.value);
            ModifyValue(obj);
            Console.WriteLine(obj.value);
            Console.WriteLine();
            Console.ReadKey();
        }
        static void ModifyValue(int value)
        {
            value *= 2;
        }
        static void ModifyValue(MyClass obj)
        {
            obj.value *= 2;
        }
    }
}
