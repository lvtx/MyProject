using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WhatIsType
{
    public class MyClass
    {
        public int MyField;
        public void MyMethod() { }
        public string MyProperty { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            MyClass obj = new MyClass(); //创建对象
            //获取类型对象
            Type typ = obj.GetType();

            //输出类的名字
            Console.WriteLine(typ.Name);
            //判断其是否公有的
            Console.WriteLine(typ.IsPublic);
            //判断两个对象是否属于同一类型
            Console.WriteLine(IsTheSameType(obj,new MyClass()));
            Console.ReadKey();

        }


        static bool IsTheSameType(Object obj1, Object obj2)
        {
            return (obj1.GetType() == obj2.GetType());
        }

    }
}
