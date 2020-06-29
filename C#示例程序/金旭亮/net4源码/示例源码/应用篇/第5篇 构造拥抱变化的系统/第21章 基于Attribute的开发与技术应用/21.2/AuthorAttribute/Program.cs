using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AuthorAttribute
{
    class Program
    {
        static void Main(string[] args)
        {
            PrintAuthorInfo(typeof(Parent));
            PrintAuthorInfo(typeof(Child));
            Console.ReadKey();

        }
        private static void PrintAuthorInfo(System.Type t)
        {
            System.Console.WriteLine("类{0}的开发者信息：", t);

            System.Attribute[] attrs = System.Attribute.GetCustomAttributes(t);  // Reflection.

            foreach (System.Attribute attr in attrs)
            {
                if (attr is AuthorAttribute)
                {
                    AuthorAttribute a = (AuthorAttribute)attr;
                    System.Console.WriteLine("   {0}, 版本 {1:f}", a.name, a.version);
                }
            }
        }


    }

    [Author("张三")]
    [Author("李四", version = 2.0)]
    public class Parent
    {
    }

    [Author("赵六", version = 3.0)]
    public class Child : Parent
    {
    }


    /// <summary>
    /// 自定义Attribute
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.Class |
                            System.AttributeTargets.Struct,
                            AllowMultiple = true,
                            Inherited = true
                            )]
    public class AuthorAttribute : System.Attribute
    {
        public string name;
        public double version = 1.0;

        public AuthorAttribute(string name)
        {
            this.name = name;
        }
    }

}
