using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectComparer
{
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
                value = 2,
                information = "ABC"
            };
            Console.WriteLine("\n查找对象：{0}",objToFind);
            int index = MyClasses.IndexOf(objToFind);
            Console.WriteLine("对象{0}在集合中的索引：{1}",objToFind,index);
            Console.ReadLine();
        }
        static List<MyClass> GenerateExampleCollection()
        {
            var collection = new List<MyClass>();
            collection.Add(new MyClass()
            {
                value = 1,
                information = "Hello"
            });
            collection.Add(new MyClass()
            {
                value = 2,
                information = "world"
            });
            collection.Add(new MyClass()
            {
                value = 2,
                information = "ABC"
            });
            return collection;
        }
    }
    class MyClass : IComparable<MyClass>
    {
        public int value { get; set; }
        public string information { get; set; }
        public int CompareTo(MyClass obj)
        {
            if (obj == null)
            {
                return -1;
            }
            int ret = this.value.CompareTo(obj.value);
            if (ret == 0)
            {
                return this.information.CompareTo(obj.information);
            }
            return ret;
        }
        public override bool Equals(object obj)
        {
            if (obj == null || obj is MyClass == false)
            {
                return false;
            }
            else
                return this.CompareTo((obj as MyClass)) == 0;
        }
        public override int GetHashCode()
        {
            return value;
        }
        public override string ToString()
        {
            return string.Format("value:{0}\t information:{1}\n", value, information);
        }
    }
}
