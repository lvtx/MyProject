using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterfaceClass
{
    class Program
    {
        static void Main(string[] args)
        {
            Duck duck = new Duck();
            duck.Fly();
            duck.Swim();
            duck.Cook();
            Bird bird = duck;
            bird.Fly();
            //将Duck对象赋给ISwin接口变量
            ISwim s = duck;
            //只会游泳
            s.Swim();
            //将Duck对象赋给另一个实现的接口IFood接口变量
            IFood food = duck;
            //只会Cook
            food.Cook();
            Console.ReadKey();
        }
    }
    public abstract class Bird
    {
        public abstract void Fly();
    }
    public interface IFood
    {
        void Cook();
    }
    public interface ISwim
    {
        void Swim();
    }
    public class Duck : Bird, IFood, ISwim
    {
        public void Cook()
        {
            Console.WriteLine("会被烤着吃");
        }

        public void Swim()
        {
            Console.WriteLine("是鸭子就一起来游泳");
        }
        public override void Fly()
        {
            Console.WriteLine("家养的不会飞");
        }
    }
}
