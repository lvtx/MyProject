using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VariableScope
{
    /// <summary>
    /// 展示变量的作用域
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            TestSingleClass();
            Console.ReadKey(true);
        }
        /// <summary>
        /// 测试单类中的变量作用域
        /// </summary>
        private static void TestSingleClass()
        {
            int value = 100;
            Console.WriteLine(value);
            MyClass obj = new MyClass();
            obj.PrintValue();

            if (value == 100)
            {
                int otherValue = 400;
            }

            //otherValue具有块作用域，在块之外无法使用，所以下面这句无法编译
            //Console.WriteLine(otherValue);

            int nouse;
            //变量必须明确地初始化，只定义而不赋值，变量将无法使用。
            //Console.WriteLine(nouse);
           
        }
    }
    /// <summary>
    /// 展示同一类中同名变量的屏蔽原则
    /// </summary>
    class MyClass
    {
        private int value = 200;

        public void PrintValue()
        {
            //如果取消以下这句的注释，则输出的是本地变量value的值：300
            int value = 300;
            //可以直接访问定义在本类中的变量
            Console.WriteLine(value);

        }
    }
    /// <summary>
    /// 展示同一类中的字段访问约定
    /// </summary>
    class Parent
    {
        private int privateValue = 100;
        protected int protectedValue = 200;
        public int publicValue = 300;

        /// <summary>
        /// 同一类中的方法可以访问所有字段
        /// </summary>
        public void parentFunc()
        {
            privateValue++;
            protectedValue++;
            publicValue++;
        }
    }
    /// <summary>
    /// 展示子类访问父类成员的相关语法特性
    /// </summary>
    class Child : Parent
    {
        /// <summary>
        /// 子类可以访问父类中的protected和public的字段，但不能访问private的字段
        /// </summary>
        public void childFunc()
        {
            //privateValue++;
            protectedValue++;
            publicValue++;
        }
    }
}
