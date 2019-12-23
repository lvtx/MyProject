using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseComparisonDelegate
{
    class MyClass
    {
        public int value;
        public string Information;
    } 
    class Program
    {
        static void Main(string[] args)
        {
            List<MyClass> myClasses = new List<MyClass>();
            Comparison<MyClass> cmp = (objs1, objs2) =>
            {
                if (objs1.value > objs2.value)
                {
                    return 1;
                }
                else if (objs1.value < objs2.value)
                {
                    return -1;
                }
                else
                {
                    return 0;
                }
            };
            //生成一个集合
            myClasses = GetMyClassList();
            //未排序前
            Console.WriteLine("排序前");
            PrintObjs(myClasses);
            //排序后
            myClasses.Sort(cmp);
            Console.WriteLine("排序后");
            PrintObjs(myClasses);
            Console.ReadLine();
        }
        /// <summary>
        /// 生成一个集合
        /// </summary>
        /// <returns></returns>
        static List<MyClass> GetMyClassList()
        {
            Random ran = new Random();
            List<MyClass> myClasses = new List<MyClass>();
            MyClass myClass = null;
            for (int i = 1; i <= 10; i++)
            {
                myClass = new MyClass() { value = ran.Next(1, 100), Information = "object" + i.ToString() };
                myClasses.Add(myClass);
            }
            return myClasses;
        }
        /// <summary>
        /// 打印集合中每个元素包含变量的值
        /// </summary>
        /// <param name="myClasses"></param>
        static void PrintObjs(List<MyClass> myClasses)
        {
            if(myClasses == null)
            {
                return;
            }
            foreach (var item in myClasses)
            {
                Console.WriteLine("{0} value is {1}",item.Information,item.value);
            }
        }
    }
}
