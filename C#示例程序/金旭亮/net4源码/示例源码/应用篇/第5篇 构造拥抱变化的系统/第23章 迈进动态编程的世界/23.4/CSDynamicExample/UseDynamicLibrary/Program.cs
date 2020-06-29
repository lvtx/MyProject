using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyDynamicLibrary;

namespace UseDynamicLibrary
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("当程序运行时，敲任意键一步步地看演示...\n");
            dynamic obj = new MyDynamicType();
            Console.WriteLine("动态添加两个字段Name和Age...");
            Console.ReadKey(true);
            obj.Name = "C#";
            obj.Age = 10;
            Console.WriteLine("访问动态对象的静态方法VisitDynamicObject,显示Name和Age的值...");
            Console.ReadKey(true);
            MyDynamicType.VisitDynamicObject(obj);
            Console.ReadKey(true);
            Console.WriteLine("\n添加一个实例方法DynamicMethod()");
            Console.ReadKey(true);
            Action<dynamic> act = (self) => Console.WriteLine("来自{0}的问候!", self.Name);
            obj.DynamicMethod = act;
            Console.WriteLine("调用DynamicMethod()");
            Console.ReadKey(true);
            obj.DynamicMethod();
            Console.WriteLine("\n添加另一个实例方法DynamicMethod2（ConsoleColor）");
            Console.ReadKey(true);
            Action<dynamic, dynamic> act2 = (self, color) =>
            {
                Console.ForegroundColor = color;
                Console.WriteLine("来自{0}的问候!", self.Name);
            };

            obj.DynamicMethod2 = act2;
            Console.WriteLine("调用DynamicMethod2(ConsoleColor.Yellow)");
            Console.ReadKey(true);
            obj.DynamicMethod2(ConsoleColor.Yellow);
            Console.ReadKey(true);

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("\n查询动态对象的所有成员");

            Console.ReadKey(true);
            Console.WriteLine(obj.ToXml());

            Console.ReadKey(true);
        }
    }
}
