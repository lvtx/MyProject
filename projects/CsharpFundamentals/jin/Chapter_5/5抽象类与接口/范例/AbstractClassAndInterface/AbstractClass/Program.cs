using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AbstractClass
{
    abstract class Fruit    //抽象类
    {
        public abstract void GrowInArea(); //抽象方法
    }

    class Apple : Fruit //苹果
    {
        public override void GrowInArea()
        {
            Console.WriteLine("我是苹果，南方北方都可以种植我。");
        }
    }
    class Pineapple : Fruit //菠萝
    {
        public override void GrowInArea()
        {
            Console.WriteLine("我是菠萝，喜欢温暖，只能在南方看到我。");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Fruit f;
            f = new Apple();
            f.GrowInArea();
            f = new Pineapple();
            f.GrowInArea();
            Console.ReadKey();

        }
    }
}
