using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericProgram
{
    class Program
    {
        static void Main(string[] args)
        {
            #region "1.泛型基础语法"
            //// 用int作为实际参数来初始化泛型类型
            //List<int> intList = new List<int>();
            //// 从int列表添加元素3
            //intList.Add(3);

            //// 用string作为实际参数来初始化泛型类型
            //List<string> stringList = new List<string>();
            //// 向string列表添加元素
            //stringList.Add("learninghard");
            #endregion
            #region "3.调用:"
            //Console.WriteLine(Compare<int>.compareGeneric(3,4));
            //Console.WriteLine(Compare<string>.compareGeneric("abc","cba"));
            //Console.ReadLine();
            #endregion
            #region "4.调用:"
            //Property.TestGeneric();
            //Property.testNonGeneric();
            //Console.ReadLine();
            #endregion
            #region "5调用:"
            //// Dictionar<,>是一个开放类型，有两个类型参数
            //Type t = typeof(Dictionary<,>);
            //Console.WriteLine("是否为开放类型：" + t.ContainsGenericParameters);
            //// DictionaryStringKey<>也是一个开放类型，但它只有一个参数类型
            //t = typeof(DictionaryStringKey<>);
            //Console.WriteLine("是否为开放类型："+ t.ContainsGenericParameters);
            //// DictionaryStringKey<int>是一个封闭类型
            //t = typeof(DictionaryStringKey<int>);
            //Console.WriteLine("是否为开放类型：" + t.ContainsGenericParameters);
            //// Dictionary<int, int>是一个封闭类型
            //t = typeof(Dictionary<int, int>);
            //Console.WriteLine("是否为开放类型：" + t.ContainsGenericParameters);
            //Console.ReadLine();
            #endregion
            #region "6.调用"
            // 使用不同的类型实参来实例化泛型实例
            //TypeWithStaticField<int>.field = "一";
            //TypeWithStaticField<string>.field = "二";
            //TypeWithStaticField<Guid>.field = "三";

            //// 非泛型类的静态字段
            //NoGenericTypeWithStaticField.field = "非泛型类静态字段一";
            //NoGenericTypeWithStaticField.field = "非泛型类静态字段二";
            //NoGenericTypeWithStaticField.field = "非泛型类静态字段三";

            //NoGenericTypeWithStaticField.OutField();

            //TypeWithStaticField<int>.OutField();
            //TypeWithStaticField<string>.OutField();
            //TypeWithStaticField<Guid>.OutField();
            //Console.ReadLine();
            #endregion
            #region "7.调用"
            //GenericClass<int>.Print();
            //GenericClass<Guid>.Print();
            //GenericClass<string>.Print();
            //NonGenericClass.Print();
            //Console.ReadLine();
            #endregion
            #region "8.泛型方法对类型参数的推断"
            int n1 = 1;
            int n2 = 2;
            // 不使用类型推断的代码
            Detective.GenericMethod<int>(ref n1, ref n2);
            //使用类型推断的代码
            Detective.GenericMethod(ref n1, ref n2);
            Console.WriteLine("n1的值现在为：" + n1);
            Console.WriteLine("n2的值现在为：" + n2);
            Console.ReadLine();
            #endregion
        }
    }
    #region "2.为什么要使用泛型，起因"
    /// <summary>
    /// 当比较不同类型数字大小的时候
    /// 需要更改代码很麻烦
    /// 引出泛型
    /// </summary>
    public class Compare
    {
        public static int compareInt(int int1,int int2)
        {
            if (int1.CompareTo(int2) < 0)
            {
                return int2;
            }
            else
                return int1;
        }

        public static string compareString(string str1,string str2)
        {
            if (str1.CompareTo(str2) < 0)
            {
                return str2;
            }
            else
                return str1;
        }
    }
    #endregion
    #region "3.实现:为了解决2中的问题出现了泛型"
    // Compare<T>为泛型类，T为类型参数
    public class Compare<T> where T : IComparable
    {
        public static T compareGeneric(T t1,T t2)
        {
            if (t1.CompareTo(t2) > 0)
            {
                return t1;
            }
            else
                return t2;
        }
    }
    #endregion
    #region "4.通过对比泛型与非泛型类来证明泛型的性能优势"
    public static class Property{
        public static void TestGeneric()
        {
            Stopwatch stopwatch = new Stopwatch();
            List<int> intGeneric = new List<int>();
            stopwatch.Start();//开始计时
            for (int i = 0; i < 10000000; i++)
            {
                intGeneric.Add(i);
            }
            stopwatch.Stop();//结束计时
            //输出所用时间
            TimeSpan ts = stopwatch.Elapsed;
            string elapsedTime = string.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
            Console.WriteLine("泛型类型运行时间："+elapsedTime);
        }
        public static void testNonGeneric()
        {
            Stopwatch stopwatch = new Stopwatch();
            // 非泛型数组
            ArrayList arrayList = new ArrayList();
            // 开始计时
            stopwatch.Start();
            for (int i = 0; i < 10000000; i++)
            {
                arrayList.Add(i);
            }
            //输出所用时间
            TimeSpan ts        = stopwatch.Elapsed;
            string elapsedTime = string.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
            Console.WriteLine("泛型类型运行时间：" + elapsedTime);
        }
    }
    #endregion
    #region "5开放泛型类型"
    public class DictionaryStringKey<T> : Dictionary<string, T>
    {

    }
    #endregion
    #region "6.泛型中的静态字段与静态函数"
    public static class TypeWithStaticField<T>
    {
        // 静态字段
        public static string field;
        // 静态方法
        public static void OutField()
        {
            Console.WriteLine(field + ":" + typeof(T).Name);
        }
    }
    // 非泛型类
    public static class NoGenericTypeWithStaticField
    {
        public static string field;
        public static void OutField()
        {
            Console.WriteLine(field);
        }
    }
    #endregion
    #region "7.泛型中的静态构造函数"
    public static class GenericClass<T>
    {
        static GenericClass()
        {
            Console.WriteLine("泛型的静态构造函数默认被调用，其类型为:"+typeof(T));
        }
        // 静态方法
        public static void Print()
        {

        }
    }
    // 非泛型类
    public static class NonGenericClass
    {
        static NonGenericClass()
        {
            Console.WriteLine("非泛型类的静态构造函数被调用");
        }
        public static void Print()
        {

        }
    }
    #endregion
    #region "8.类型参数的推断"
    public static class Detective
    {
        public static void GenericMethod<T>(ref T t1,ref T t2)
        {
            T temp = t1;
            t1 = t2;
            t2 = temp;
        }
    }
    #endregion
    #region "9.类型参数的约束"
    public static class Smaple<T> where T : Stream
    {
        static void Test(T t)
        {
            t.Close();
            int i = new int();
        }
    }
    #endregion
}
