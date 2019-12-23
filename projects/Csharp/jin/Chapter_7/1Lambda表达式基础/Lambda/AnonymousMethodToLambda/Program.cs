using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnonymousMethodToLambda
{
    //定义委托
    public delegate int AddDelegate(int x, int y);
    class MyClass
    {
        private int Add(int x,int y)
        {
            return x + y;
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            //使用C#匿名方法特性直接给委托变量赋值
            AddDelegate addDel = delegate (int x, int y)
            {
                return x + y;
            };
            //通过委托变量调用匿名委托
            int result = addDel(5,10);
            Console.WriteLine(result);
            //直接将匿名方法作为函数参数
            InvokeDelegate(delegate (int x, int y)
            {
                return x + y;
            }, 5, 10);
            //使用lambda表达式
            InvokeDelegate((x, y) => x + y, 10, 20);
            Console.ReadLine();
        }
        //使用委托类型参数的方法
        public static void InvokeDelegate(AddDelegate addDel, int x, int y)
        {
            Console.WriteLine(addDel(x, y)); 
        }
    }
}
