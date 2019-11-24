using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Duck
{
    class Program
    {
        static void Main(string[] args)
        {
            Duck d = new Duck();
            //Duck对象d可以使用3种方法：
            //1.自身定义的；
            //2.父类定义的
            //3.接口定义的
            d.Fly();
            d.Cook();
            d.Swim();
            //将子类（Duck）对象赋给基类变量
            Bird b = d;
            //现在只能使用基类定义的Fly()方法
            b.Fly();
            //将Duck对象赋给ISwin接口变量
            ISwim s = d;
            //现在只能使用接口定义的Swim()方法
            s.Swim();
            //将Duck对象赋给另一个实现的接口IFood接口变量
            IFood f = d;
            //现在只能使用接口定义的Cook()方法
            f.Cook();
            Console.ReadKey(true);
        }
    }
    //定义两个接口
    public interface ISwim
    {
        void Swim();
    }
    public interface IFood
    {
        void Cook();
    }
    //定义一个抽象类
    public abstract class Bird
    {
        public abstract void Fly();

    }
    //继承自一个抽象类，实现两个接口
    public class Duck : Bird, IFood, ISwim
    {
        //实现ISwim接口
        public void Swim()
        {
            Console.WriteLine("是鸭子就会游泳");
        }
        //实现IFood接口
        public void Cook()
        {
            Console.WriteLine("鸭子经常被烧烤，北京烤鸭就很有名");
        }
        //实现抽象类Bird中的抽象方法
        public override void Fly()
        {
            Console.WriteLine("只有野鸭才会飞");
        }
    }
}
