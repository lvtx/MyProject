using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UseExtensionMethods
{
    /// <summary>
    /// 将被“动态”扩展的原始类型
    /// </summary>
    class MyClass
    {
        public int value = 100;
    }

    /// <summary>
    /// 存放扩展方法的类
    /// </summary>
    static class MyExtensionMethods
    {
        //类A的扩展方法一
        public static int ExtendMethod1(this MyClass nouse, int a, int b)
        {
            return a + b;
        }
        //类A的扩展方法二
        public static int ExtendMethod2(this MyClass obj1, MyClass obj2)
        {
            return obj1.value + obj2.value;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            MyClass obj1= new MyClass();
            Console.WriteLine(obj1.ExtendMethod1(50, 60));

            MyClass obj2 = new MyClass();
            obj2.value = 200;
            Console.WriteLine(obj1.ExtendMethod2(obj2));

            Console.ReadKey();

        }
    }
}
