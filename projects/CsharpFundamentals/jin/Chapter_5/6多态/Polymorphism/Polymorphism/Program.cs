using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fruit
{
    class Program
    {
        static void Main(string[] args)
        {
            CallPolymorphismMethod();
            ShowPolymorphism();
            Console.ReadKey();
        }
        static void ShowPolymorphism()
        {
            Fruit f;
            f = new Apple();
            //这句输出什么？
            f.GrowInArea();
            f = new Pineapple();
            //与前面相同的代码，输出什么？
            f.GrowInArea();

        }
        /// <summary>
        /// 使用了“多态”特性的方法，其代码具有稳定性，与具体子类无关
        /// 此方法用于显示特定水果的“适宜种植区域”信息
        /// </summary>
        /// <param name="fruit"></param>
        static void ShowFruitGrowInAreaInfo(Fruit fruit)
        {
            fruit.GrowInArea();
        }
        /// <summary>
        /// 多态代码的典型用法：
        /// 在运行时动态地传递对象给它
        /// </summary>
        static void CallPolymorphismMethod()
        {
            ShowFruitGrowInAreaInfo(new Apple());
            ShowFruitGrowInAreaInfo(new Pineapple());
        }
    }
    abstract public class Fruit
    {
        abstract public void GrowInArea();
    }
    public class Apple:Fruit
    {
        public override void GrowInArea()
        {
            Console.WriteLine("我是苹果南方北方都有我");
        }
    }
    public class Pineapple : Fruit
    {
        public override void GrowInArea()
        {
            Console.WriteLine("我是菠萝只在南方");
        }
    }
}
