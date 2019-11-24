using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarFactory
{
    class Program
    {
        static void Main(string[] args)
        {
            Car car = new Car();
            Console.WriteLine("\n敲任意键启动汽车……\n");
            Console.ReadKey(true);
            car.Start();
            Console.WriteLine("\n敲任意键刹车……\n");
            Console.ReadKey(true);
            car.Brake();
            Console.WriteLine("\n敲任意键退出程序……\n");
            Console.ReadKey(true);
        }
    }
    //代表发动机
    class Engine
    {
        public void Start()
        {
            Console.WriteLine("发动机启动");
        }
        public void Stop()
        {
            Console.WriteLine("发动机停止");
        }
    }

    //汽车轮
    class Wheel
    {

    }

    class Car
    {
        private Engine _engine = new Engine();

        private Wheel[] wheels = new Wheel[4];

        public void Start()
        {
            Console.WriteLine("插入钥匙，打火，启动……");
            _engine.Start();
            Console.WriteLine("启动成功，开始行驶");
        }
        public void Brake()
        {
            Console.WriteLine("刹车！");
            Stop();
        }
        public void Stop()
        {
            _engine.Stop();
            Console.WriteLine("汽车停车");
        }
    }
}
