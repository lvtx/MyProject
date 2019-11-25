using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericDelegateInBaseClassLibrary
{
    class Program
    {
        static void Main(string[] args)
        {
            UseFuncDelegate();
            UseActionDelegate();
            Console.ReadKey();


        }

        private static void UseFuncDelegate()
        {
            //使用匿名方法
            Func<int, int, long> add = delegate(int a, int b)
            {
                return a + b;
            };
            //使用Lambda表达式
            Func<int, int, int> subtract = (a, b) => a - b;

            //调用示例
            Console.WriteLine(add(5, 10));
            Console.WriteLine(subtract(10, 5));
        }

        private static void UseActionDelegate()
        {
            Action<string> show = (info) =>
            {
                Console.WriteLine(info);
            };
            show("Hello");
        }
    }
}
