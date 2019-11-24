using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Equals
{
    class Program
    {
        static void Main(string[] args)
        {
            #region "使用int的equals()方法"
            int i = 100;
            int j = 100;
            Console.WriteLine(i == j);
            Console.WriteLine(i.Equals(j));
            #endregion
            #region "对象的Equals()方法"
            MyClass obj1 = new MyClass();
            MyClass obj2 = new MyClass();
            Console.WriteLine(obj1 == obj2);
            Console.WriteLine(obj1.Equals(obj2));
            #endregion
            #region "字符串的Equals()方法"
            //string类型为引用类型
            string str1 = "a";
            string str2 = "a";
            Console.WriteLine(str1 == str2);
            Console.WriteLine(str1.Equals(str2));
            Console.ReadKey();
            #endregion
        }
    }

    class MyClass
    {
        private int value = 100;
        /// <summary>
        /// 对Equals进行重构
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
                return this.value == (obj as MyClass).value;
            }
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
