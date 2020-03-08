using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UsePredicateDelegate
{
    class MyClass
    {
        public int Value;
        public string Information;
    }

    class Program
    {
        /// <summary>
        /// 创建一个MyClass类型的对象集合
        /// </summary>
        /// <returns></returns>
        private static List<MyClass> GetMyClassList()
        {
            List<MyClass> lst = new List<MyClass>();
            Random ran = new Random();
            MyClass obj = null;
            for (int i = 0; i < 10; i++)
            {
                obj = new MyClass { Value = ran.Next(1, 100), Information = "object" + i.ToString() };
                lst.Add(obj);
            }
            return lst;
        }

        /// <summary>
        /// 打印一个MyClass对象集合的所有成员
        /// </summary>
        /// <param name="lst"></param>
        private static void PrintList(List<MyClass> lst)
        {
            if (lst == null)
                return;
            foreach (MyClass obj in lst)
                Console.WriteLine("Infomation={0},Value={1}", obj.Information, obj.Value);
        }

        
        static bool GreaterThan50(MyClass elem)
        {
            if (elem.Value > 50)
                    return true;
            return false;
        }

        static void Main(string[] args)
        {
            Predicate<MyClass> pred = GreaterThan50;
            List<MyClass> lst = GetMyClassList();
            Console.WriteLine("生成的MyClass对象集合为：");
            PrintList(lst);
           MyClass foundElement= lst.Find(pred);
           if (foundElement != null)
               Console.WriteLine("找到了符合条件的对象。Infomation={0},Value={1}", foundElement.Information, foundElement.Value);
           else
               Console.WriteLine("未找到符合条件的对象");
            Console.ReadKey();
        }
    }
}
