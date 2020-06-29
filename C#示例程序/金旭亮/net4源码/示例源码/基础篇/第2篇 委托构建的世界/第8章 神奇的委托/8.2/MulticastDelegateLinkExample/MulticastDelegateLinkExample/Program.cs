using System;
using System.Collections.Generic;
using System.Text;

namespace MulticastDelegateLinkExample
{
    public delegate int MyDelegate(int value);

    public class A
    {
        public int f1(int i)
        {
            Console.WriteLine("f1.i={0}", i);
            return i;
        }
        public int f2(int i)
        {
            i *= 2;
            Console.WriteLine("f2.i={0}", i);
            return i;
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            A a = new A();
            MyDelegate s1 = new MyDelegate(a.f1);
            s1 += new MyDelegate(a.f2);

            //上面两句可以简写为以下形式：
            //MyDelegate s1 = a.f1;
            //s1 += a.f2;

            Delegate[] ds;
            
            ds = s1.GetInvocationList();
            Console.WriteLine("s1的方法调用列表中包含{0}个方法", ds.GetLength(0));

            s1(5); //先调用a.f1(),再调用a.f2()

            MyDelegate s2 = new MyDelegate(a.f1);
            s2 += new MyDelegate(a.f2);

            //组合委托

            Delegate mul;
            //mul = System.Delegate.Combine(s1, s2);
            mul = s1 + s2;
            ds = mul.GetInvocationList();
            Console.WriteLine("mul的方法调用列表中包含{0}个方法", ds.GetLength(0));
            int ret = (mul as MyDelegate)(10); //获取委托调用链最后一个方法调用的返回值

            Console.WriteLine("ret={0}", ret);

            Console.ReadKey();
        }
    }
}
