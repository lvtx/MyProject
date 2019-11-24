using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EqualsDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            //int i = 100;
            //int j = 100;
            //Console.WriteLine(i==j);
            ////int类型居然有equals方法？
            //Console.WriteLine(i.Equals(j));

            //Console.WriteLine();

            //MyClass obj1 = new MyClass();
            //MyClass obj2 = new MyClass();
            //Console.WriteLine(obj1 == obj2);
            //Console.WriteLine(obj1.Equals(obj2));

            Console.WriteLine();

            //String类型是引用类型还是值类型？
            String str1 = "Hello";
            String str2 = "Hello";
            Console.WriteLine(str1 == str2);
            Console.WriteLine(str1.Equals(str2));

            Console.ReadKey();
        }
    }

    class MyClass
    {
        private int value = 100;

        #region "比较对象"

        /// <summary>
        /// 如何比较两个对象的“内容”
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            else
            {
                if (obj is MyClass)
                {
                    return this.value == (obj as MyClass).value;
                }
            }
            return false;
        }

        public override int GetHashCode()
        {
            return value;
        }
        #endregion
    }
}
