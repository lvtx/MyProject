using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace DynamicInvokeMethod
{
    class Program
    {
        static void Main(string[] args)
        {
            //装入程序集
            Assembly ass = Assembly.LoadFrom("DynamicInvokeMethodLibrary.dll");
            //创建指定类型的对象
            object obj = ass.CreateInstance("DynamicInvokeMethodLibrary.SumNumber");
            //获取对象类型
            Type typ = obj.GetType();
            //获取Sum方法对象，拥有两个参数
            MethodInfo mtd1 = typ.GetMethod("Sum", new Type[] { typeof(int), typeof(int) });
             //获取Sum方法对象，拥有三个参数
            MethodInfo mtd2 = typ.GetMethod("Sum", new Type[] { typeof(int), typeof(int),typeof(int) });
            
            //动态方法调用
            object ret;
            ret = mtd1.Invoke(obj, new object[] { 100, 100 });
            System.Console.WriteLine(ret);
            ret = mtd2.Invoke(obj, new object[] { 100, 100,100 });
            System.Console.WriteLine(ret);

            //获取静态的Sum方法对象
            MethodInfo staticmtd = typ.GetMethod("Sum", BindingFlags.Static|BindingFlags.Public );
            int[] arr= new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9,10 };
            ret = staticmtd.Invoke(null, new object[] { arr });
            System.Console.WriteLine(ret);

            System.Console.ReadKey();
       
        }
    }
}
