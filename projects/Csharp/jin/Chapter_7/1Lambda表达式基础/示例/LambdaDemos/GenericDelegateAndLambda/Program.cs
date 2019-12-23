using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UseGenericDelegate
{
    //定义泛型委托
    public delegate T MyGenericDelegate<T>(T obj);

    class Program
    {
        static void Main(string[] args)
        {
            //使用匿名方法给泛型委托变量赋值
            MyGenericDelegate<int> del = delegate(int vlaue)
            {
                return vlaue * 2;
            };
            //调用泛型委托变量引用的匿名方法
            Console.WriteLine(del(100));

            //使用Lambda表达式给泛型委托变量赋值
            MyGenericDelegate<int> del2 = (value) => value * 2;

            //调用泛型委托变量引用的Lambda表达式
            Console.WriteLine(del2(100));

            Console.ReadKey();
        }
    }
}
