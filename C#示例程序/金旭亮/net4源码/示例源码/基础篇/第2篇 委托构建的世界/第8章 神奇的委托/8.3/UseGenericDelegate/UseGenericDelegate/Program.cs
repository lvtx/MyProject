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
        static int MyFunc(int value)
        { 
            return value * 2;
        }

        static void Main(string[] args)
        {
            //使用匿名方法给泛型委托变量赋值
            MyGenericDelegate<int> del = MyFunc;
                
            //调用泛型委托变量引用的匿名方法
            Console.WriteLine(del(100));
            Console.ReadKey();
        }
    }
}
