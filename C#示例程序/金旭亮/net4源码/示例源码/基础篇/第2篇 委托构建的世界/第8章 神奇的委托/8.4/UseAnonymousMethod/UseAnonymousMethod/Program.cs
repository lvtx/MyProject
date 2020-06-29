using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UseAnonymousMethod
{
    //定义一个委托
    public delegate int AddDelegate(int i, int j);

    class Program
    {
        //使用委托类型的参数
        public static void invokeDelegate(AddDelegate del, int i, int j)
        {
            Console.WriteLine(del(i, j));
        }

        static void Main(string[] args)
        {
            //利用C#匿名方法特性直接给委托变量赋值
            AddDelegate del = delegate(int i, int j)
            {
                return i + j;
            };

            //通过委托变量调用匿名方法
            Console.WriteLine(del(100, 200));

            //直接将匿名方法作为函数参数
            invokeDelegate(delegate(int i, int j)
            {
                return i + j;
            }, 100, 200);

            Console.ReadKey();
        }
    }
}
