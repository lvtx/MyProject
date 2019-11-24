using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectSort
{
    class MyClass : IComparable<MyClass>
    {
        public int Value { get; set; }
      
        public String Information { get; set; }
        /// <summary>
        /// 先按照Value字段值比较，如果Value值一样，
        /// 就按照Information字段值比较,
        /// 只有两个字段值都一样，才认为这两个对象相等
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(MyClass other)
        {
            if (other == null)
            {
                return -1;
            }
            int result = this.Value.CompareTo(other.Value);
            if (result == 0)
            {
                return this.Information.CompareTo(other.Information);
            }
            return result;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || obj is MyClass == false)
            {
                return false;
            }
            return this.CompareTo(obj as MyClass) == 0;
        }

        public override int GetHashCode()
        {
            return Value ;
        }

        public override string ToString()
        {
            return String.Format("Value:{0}\t Information:{1}\n", Value, Information) ;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var MyClasses = GenerateExampleCollection();
            Console.WriteLine("原始集合：");
            MyClasses.ForEach((obj) => { Console.WriteLine(obj); });
            MyClasses.Sort();
            Console.WriteLine("\n排序之后：");
            MyClasses.ForEach((obj) => { Console.WriteLine(obj); });

            MyClass objToFind = new MyClass()
            {
                Value = 2,Information = "ABC"
            };
            Console.WriteLine("\n查找对象：{0}", objToFind);
            int index = MyClasses.IndexOf(objToFind);
            Console.WriteLine("对象{0}在集合中的索引：{1}",objToFind,index);

            Console.ReadKey();
        }

        static List<MyClass> GenerateExampleCollection()
        {
            var collection = new List<MyClass>();
            collection.Add(new MyClass()
            {
                 Value=1, Information="Hello"
            });
            collection.Add(new MyClass()
            {
                Value = 2,
                Information = "World"
            });
            collection.Add(new MyClass()
            {
                Value = 2,
                Information = "ABC"
            });
            return collection;
        }
    }
}
