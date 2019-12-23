using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericDelegateAndLambda
{
    //声明一个泛型委托
    delegate T GenericDelegate<T>(T x, T y);
    class Program
    {      
        static void Main(string[] args)
        {
            //使用匿名方法给泛型委托变量赋值 
            GenericDelegate<int> myDel = delegate (int x, int y)
            {
                return x + y;
            };
            Console.WriteLine(myDel(10,20));
            //使用lambda表达式给泛型委托变量赋值
            GenericDelegate<int> myDel2 = (x, y) => x + y;
            Console.WriteLine(myDel2(20, 30));
            Console.ReadLine();
        }
    }
}
