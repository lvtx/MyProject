using System;
using System.Collections.Generic;
using System.Text;

namespace ObjectCompare
{
    class Program
    {
        static void Main(string[] args)
        {
            Circle obj1 = new Circle { Radius = 100.1 };
            Circle obj2 = new Circle { Radius = 100.9 };
            //测试覆盖的方法
            Console.WriteLine(obj1.GetHashCode());//100100
            Console.WriteLine(obj2.GetHashCode());//100900
            Console.WriteLine(obj1.CompareTo(obj2)); //-1
            Console.WriteLine(obj1.Equals(obj2));//false
            //测试重载的运算符
            Console.WriteLine(obj1 == obj2);    //false
            Console.WriteLine(obj1 != obj2);    //true
            Console.WriteLine(obj1 >= obj2);    //false

            //以下测试Circle对象数组的排序功能
            Circle[] circles = new Circle[10];  //创建Circle对象数组
            Random ran = new Random();
            for (int i = 0; i < 10; i++)
            {
                circles[i] = new Circle { Radius = ran.Next(1, 1000)/100.0 };
            }
            Console.WriteLine("原始数组：");
            Array.ForEach<Circle>(circles, (circle) => { Console.WriteLine("圆对象的哈希代码：{0}，半径：{1}", circle.GetHashCode(), circle.Radius); });
            Console.WriteLine("\n排序之后：");
            Array.Sort(circles);
            Array.ForEach<Circle>(circles, (circle) => { Console.WriteLine("圆对象的哈希代码：{0}，半径：{1}", circle.GetHashCode(), circle.Radius); });

            Console.ReadKey();
        }
    }
}
