using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }

    class Vehicle
    {
        private int _speed;

        public virtual int Speed//属性重写
        {
            get { return _speed; }
            set { _speed = value; }
        }


        public virtual void Run()
        {
            Console.WriteLine("I'm running!");
            _speed = 100;
        }
    }

    class Car : Vehicle
    {
        private int _rpm;

        public override int Speed
        {
            get { return _rpm / 100; }
            set { _rpm = value * 100; }
        }

        public override void Run()
        {
            Console.WriteLine("Car is running!");
            _rpm = 5000;
        }
    }

    class Tuck : Vehicle
    {
        private int _rpm;

        public override int Speed
        {
            get { return _rpm / 100; }
            set { _rpm = value * 100; }
        }

        public override void Run()
        {
            Console.WriteLine("Tuck is running!");
            _rpm = 5000;
        }
    }
}
