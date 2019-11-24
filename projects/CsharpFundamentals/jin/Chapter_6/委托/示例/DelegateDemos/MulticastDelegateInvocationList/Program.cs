using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MulticastDelegateInvocationList
{
    /// <summary>
    /// 定义一个委托变量
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public delegate int MyDelegate(int value);
    /// <summary>
    /// MyClass类中的两个方法Func1和Func2符合委托的要求
    /// </summary>
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

            //获取方法调用列表
            Delegate[] ds = del1.GetInvocationList();
            Console.WriteLine("del1的委托调用列表中包含{0}个方法", ds.GetLength(0));

            del1(5); //先调用obj.Func1(),再调用obj.Func2()

            MyDelegate del2 = obj.Func1;
            del2 += obj.Func2;
            Console.WriteLine("del2的委托调用列表中包含{0}个方法",
                del2.GetInvocationList().GetLength(0));
            del2(5); //先调用obj.Func1(),再调用obj.Func2()

            //组合两个委托变量
            MyDelegate mul = del1 + del2;
            ds = mul.GetInvocationList();
            Console.WriteLine("合并del1和del2之后，新的委托变量mul的委托调用列表中包含{0}个方法", ds.GetLength(0));
            int ret = mul(10); //获取委托调用列表最后一个方法的返回值

            Console.WriteLine("合并之后，新委托变量mul的返回值 = {0}", ret);

            mul -= obj.Func2;
            ////取消以下这句注释，观察程序运行结果你发现了什么特性？
            mul -= obj.Func2;
            mul -= obj.Func2;
            Console.WriteLine("移除Func2之后，委托变量mul的委托调用列表中包含{0}个方法",
                mul.GetInvocationList().GetLength(0));

            ret = mul(10); //获取委托调用列表最后一个方法的返回值
            Console.WriteLine("移除Func2之后，返回值 = {0}", ret);

            Console.ReadKey();
        }
    }
}
