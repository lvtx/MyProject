using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PartialClass
{
    class Program
    {
        static void Main(string[] args)
        {
            //创建程序集中的类的对象
            MyPublicClass obj = new MyPublicClass();

            //访问类的基本功能
            obj.i = 100;
            obj.Func();

            //访问类的扩充功能
            obj.j = 200;
            obj.OtherFunc();

            Console.ReadKey();

        }
    }
}
