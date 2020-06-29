using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Dynamic;

namespace ExpandoObjectBasics
{
    class Program
    {
        static void Main(string[] args)
        {
            dynamic dynamicObj = new ExpandoObject();
           
            //添加字段
            dynamicObj.Value = 100; 
            Console.WriteLine("字段Value已添加");
            //添加方法
            dynamicObj.Increment = new Action(() => dynamicObj.Value++);
            Console.WriteLine("方法Increment已添加");
            for (int i = 0; i < 10; i++)
                dynamicObj.Increment();

           Console.WriteLine("调用10次Increment()方法……");
            Console.WriteLine("dynamicObj.Value={0}",dynamicObj.Value);
            Console.WriteLine("移除Increment()方法");
            //移除Increment方法
            (dynamicObj as IDictionary<string, object>).Remove("Increment");

            try
            { 
                Console.WriteLine("尝试调用已移除的Increment()方法...");
                dynamicObj.Increment(); //将引发异常
            }
            catch (Exception ex)
            {

                Console.WriteLine("{0}:{1}", ex.GetType(), ex.Message);
            }

            Console.ReadKey();

        }
    }
}
