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
        private static List<MyClass> GetAList()
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
        static void Main(string[] args)
        {
            //定义对象查询条件
            Predicate<MyClass> pred = (elem)=>
            {
                if (elem.Value > 50)
                    return true;
                else
                    return false;
            };

            List<MyClass> lst = GetAList();
            Console.WriteLine("生成的A对象集合为：");
            PrintList(lst);
            //在集合中查询对象
           MyClass foundElement= lst.Find(pred);
           if (foundElement != null)
               Console.WriteLine("找到了符合条件的对象。Infomation={0},Value={1}", 
                   foundElement.Information, foundElement.Value);
           else
               Console.WriteLine("未找到符合条件的对象");

            Console.ReadKey();
        }
    }
}
