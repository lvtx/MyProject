using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbstractClass
{
    class Program
    {
        static void Main(string[] args)
        {
            Fruit apple = new Apple();
            apple.GrowInArea();
            Fruit pineApple = new PineApple();
            pineApple.GrowInArea();
            Console.ReadKey();
        }
    }
    abstract class Fruit
    {
        public abstract void GrowInArea();
    }
    internal class Apple : Fruit
    {
        public override void GrowInArea()
        {
            Console.WriteLine("我是苹果，南方北方都可以种植我。");
        }
    }
    internal class PineApple : Fruit
    {
        public override void GrowInArea()
        {
            Console.WriteLine("我是菠萝我在南方");
        }
    }
}
