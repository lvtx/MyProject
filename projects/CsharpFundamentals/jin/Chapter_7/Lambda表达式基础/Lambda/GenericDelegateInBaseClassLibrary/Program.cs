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
            //将匿名方法赋值给泛型委托变量
            Func<int, int> myDel = delegate (int x)
             {
                 return x * 2;
             };
            Console.WriteLine(myDel(5));
            //使用lambda表达式给泛型委托变量赋值
            Func<int, int, long> myDel2 = (x, y) => x + y;
            Console.WriteLine(myDel2(3,5));
            UseActionDelegate();
            Console.ReadLine();
        }
        static void UseActionDelegate()
        {
            Action<string> myDel = (sayHello) =>
            {
                Console.WriteLine(sayHello);
            };
            myDel("Hello");
        }
    }
}
