using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectCompare
{
    class Program
    {
        static void Main(string[] args)
        {
            Circle circle1 = new Circle() { radius = 1.001 };
            Circle circle2 = new Circle() { radius = 1.002 };
            Console.WriteLine(circle1.CompareTo(circle2));
            Console.WriteLine(circle2.CompareTo(circle1));
            Console.WriteLine(circle1.GetHashCode());
            Console.WriteLine(circle2.GetHashCode());
            Console.WriteLine("===========运算符重载===========");
            Console.WriteLine(circle1 > circle2);
            Console.WriteLine(circle1 < circle2);
            Console.WriteLine(circle1 == circle2);
            Console.WriteLine(circle1 != circle2);
            Console.WriteLine("========对象数组的排序功能========");
            Circle[] circles = new Circle[10];
            Random r = new Random();
            for (int i = 0; i < 10; i++)
            {
                circles[i] = new Circle() { radius = r.Next(1,1000) / 100.0};
            }
            Console.WriteLine("原始数组");
            Array.ForEach<Circle>(circles, (circle) => { Console.WriteLine("圆对象的哈希代码：{0}，半径：{1}", circle.GetHashCode(), circle.radius); });
            Console.WriteLine("\n排序之后：");
            Array.Sort(circles);
            Array.ForEach<Circle>(circles, (circle) => { Console.WriteLine("圆对象的哈希代码：{0}，半径：{1}", circle.GetHashCode(), circle.radius); });
            Console.ReadLine();
        }
    }
}
