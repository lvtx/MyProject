using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StaticAndInstanceField
{
    class Program
    {
        static void Main(string[] args)
        {
            MyClass obj = null;
            //创建100个对象
            for (int i = 0; i < 100; i++)
            {
                obj = new MyClass();
                obj.increaseValue();
            }
            //查看静态字段与实例字段的值
            Console.WriteLine("dynamicVar=" + obj.dynamicVar);
            Console.WriteLine("staticVar=" + MyClass.staticVar);
            //程序暂停，敲任意键继续
            Console.ReadKey();
        }
    }
}
