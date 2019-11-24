using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MethodParameters
{

    class MyClass
    {
        public int Value = 100;
    }

    class Program
    {
        static void Main(string[] args)
        {
            MyClass obj = new MyClass();
            Console.WriteLine(obj.Value); //输出：100
            ModifyValue(obj.Value);
            Console.WriteLine(obj.Value);  //输出：100
            ModifyValue(obj);
            Console.WriteLine(obj.Value);  //输出：200
            Console.ReadKey();
        }

       //参数类型：值类型
        static void ModifyValue(int value)
        {
            value *= 2;
        }
        //参数类型：引用类型
        static void ModifyValue(MyClass obj)
        {
            obj.Value *= 2;
        }
    }
}
