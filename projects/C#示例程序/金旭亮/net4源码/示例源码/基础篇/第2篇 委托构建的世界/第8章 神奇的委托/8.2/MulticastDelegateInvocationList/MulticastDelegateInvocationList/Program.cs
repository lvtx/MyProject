using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MulticastDelegateInvocationList
{
    public delegate int MyDelegate(int value);

    public class MyClass
    {
        public int Func1(int argument)
        {
            Console.WriteLine("Func1: i={0}", argument);
            return argument;
        }
        public int Func2(int argument)
        {
            argument *= 2;
            Console.WriteLine("Func2: i={0}", argument);
            return argument;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            MyClass obj = new MyClass();
            MyDelegate del1 = new MyDelegate(obj.Func1);
            del1 += new MyDelegate(obj.Func2);

            //上面两句可以简写为以下形式：
            //MyDelegate del1 = obj.Func1;
            //del1 += obj.Func2;

            Delegate[] ds;

            ds = del1.GetInvocationList();
            Console.WriteLine("del1的委托调用列表中包含{0}个方法", ds.GetLength(0));

            del1(5); //先调用obj1.Func1(),再调用obj1.Func2()

            MyDelegate del2 = new MyDelegate(obj.Func1);
            del2 += new MyDelegate(obj.Func2);

            //组合委托

            Delegate mul = del1 + del2;
            ds = mul.GetInvocationList();
            Console.WriteLine("mul的委托调用列表中包含{0}个方法", ds.GetLength(0));
            int ret = (mul as MyDelegate)(10); //获取委托调用列表最后一个方法的返回值

            Console.WriteLine("ret = {0}", ret);

            Console.ReadKey();
        }
    }
}
