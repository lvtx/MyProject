using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DynamicExample
{
    class Program
    {
        static void Main(string[] args)
        {
            dynamic d = 3;   //动态推断d为int类型
            Console.WriteLine(d + 3);  //输出结果为6

            ExampleClass ec = new ExampleClass();
            Console.WriteLine(ec.exampleMethod(10));//数字：10
            Console.WriteLine(ec.exampleMethod("value"));//字串：value
            //以下对ec.exampleMethod()方法的调用将引发编译错误，因为此方法只接收一个参数
            //Console.WriteLine(ec.exampleMethod(10, 4));


            dynamic dynamic_ec = new ExampleClass();
            Console.WriteLine(dynamic_ec.exampleMethod(10));//数字：10
            // 由于 dynamic_ec 是dynamic的，因此，编译器不会检测其参数
            //以下代码将可以顺利通过编译，却会引发一个运行时错误
            Console.WriteLine(dynamic_ec.exampleMethod(10, 4));
            Console.ReadKey();
        }
    }

    class ExampleClass
    {
        static dynamic field;  //静态的dynamic字段

        dynamic prop { get; set; }  //dynamic属性

        public dynamic exampleMethod(dynamic d)//dynamic方法
        {
           
            if (d is int)
            {
                return "数字:"+d.ToString() ;
            }
            else
            {
                return "字串:"+(string)d;
            }
        }
    }
}
