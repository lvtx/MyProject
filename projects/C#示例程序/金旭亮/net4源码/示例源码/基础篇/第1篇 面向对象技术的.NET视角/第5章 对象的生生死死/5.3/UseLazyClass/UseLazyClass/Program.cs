using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UseLazyClass
{
    class Program
    {
        static void Main(string[] args)
        {
            Lazy<MyClass> Obj = new Lazy<MyClass>();
            Console.WriteLine("现在将给MyClass对象的IntValue属性赋值100");
            MyClass obj = Obj.Value; //此处导致对象创建！
            obj.IntValue = 100;
            Console.WriteLine("MyClass对象{0}的IntValue属性={1}",obj.GetHashCode(),obj.IntValue);
            Console.ReadKey();
        }
    }

    class MyClass
    {
        public MyClass()
        {
            Console.WriteLine("MyClass对象创建，其标识：{0}", this.GetHashCode());
        }
        public int IntValue
        {
            get;
            set;
        }
    }

}
