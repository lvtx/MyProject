using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsePredicateDelegate
{
    class Program
    {
        class MyClass
        {
            public int Value;
            public string Information;
        }
        static void Main(string[] args)
        {
            List<MyClass> myClasses = new List<MyClass>();
            myClasses = GetAList();
            Predicate<MyClass> pre = (objs) =>
            {
                if (objs.Value > 50)
                {
                    return true;
                }
                else
                    return false;
            };
            Console.WriteLine("原序列");
            PrintList(myClasses);
            MyClass foundElement = myClasses.Find(pre);
            Console.WriteLine("筛选之后");
            if (foundElement != null)
                Console.WriteLine("找到了符合条件的对象。Infomation={0},Value={1}",
                    foundElement.Information, foundElement.Value);
            else
                Console.WriteLine("未找到符合条件的对象");
            Console.ReadLine();
        }
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
    }
}
