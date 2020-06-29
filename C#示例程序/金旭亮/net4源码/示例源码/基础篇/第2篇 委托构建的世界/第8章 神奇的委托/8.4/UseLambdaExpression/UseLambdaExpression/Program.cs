using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UseLambdaExpression
{
    public delegate string SomeDelegateType(int arguments);

    class Program
    {
        static void Main(string[] args)
        {
            //使用匿名方法：
            SomeDelegateType del1 = delegate(int arguments)
                        {
                            return arguments.ToString();
                        };
            // 使用Lambda表达式达到同样的目的：
            SomeDelegateType del2 = arguments => { return arguments.ToString(); };

            // 可以直接省略return关键字。
            SomeDelegateType del3 = arguments => arguments.ToString();

            //直接调用委托变量引用的匿名方法
            Console.WriteLine(del1(100));
            Console.WriteLine(del2(200));
            Console.WriteLine(del3(300));
            Console.ReadKey();




        }
    }
}
