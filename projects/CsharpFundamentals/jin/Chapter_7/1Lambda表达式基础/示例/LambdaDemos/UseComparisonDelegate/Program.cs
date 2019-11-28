using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UseComparisonDelegate
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
        private static List<MyClass> GetMyClasList()
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
            //使用Lambda表达式创建一个 Comparison<MyClass>委托对象
            Comparison<MyClass> cmp = (obj1,  obj2)=>
            {
                if (obj1.Value > obj2.Value)
                    return 1;
                else
                    if (obj1.Value < obj2.Value)
                        return -1;
                    else
                        return 0;
            };

            List<MyClass> lst = GetMyClasList();
            Console.WriteLine("生成的A对象集合为：");
            PrintList(lst);
            //调用泛型委托进行排序
            lst.Sort(cmp);
            Console.WriteLine("排序后的结果为：");
            PrintList(lst);

            Console.ReadKey();
        }
    }
}
