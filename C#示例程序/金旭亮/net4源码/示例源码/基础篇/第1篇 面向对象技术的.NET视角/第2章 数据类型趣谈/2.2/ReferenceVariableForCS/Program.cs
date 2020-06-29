using System;
using System.Collections.Generic;
using System.Text;

namespace ReferenceVariableForCS
{
    class MyClass
    {
        public int Value;
    }
    class Program
    {
        static void Main(string[] args)
        {
            MyClass obj1 = new MyClass() { Value = 100 };
            MyClass obj2 = null;
            obj2 = obj1;  //引用类型变量的赋值
            Console.WriteLine("obj2.Value = " + obj2.Value); //输出： obj2.Value = 100

            //TheFirst与TheSecond引用不同的对象
            MyClass TheFirst = new MyClass();
            MyClass TheSecond = new MyClass();
            Console.WriteLine(TheFirst == TheSecond);//输出：False
            TheSecond = TheFirst;//TheFirst与TheSecond引用相同的对象
            Console.WriteLine(TheFirst == TheSecond);//输出：True

            //程序暂停       

            Console.ReadKey();
        }
    }
}
