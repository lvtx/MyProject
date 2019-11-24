using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2_Exercise
{
    class Program
    {
        static void Main(string[] args)
        {
            Computer computer = new Computer();
            computer.Assembled();
            Console.ReadKey(true);
            computer.Start();
            Console.ReadKey(true);
        }
    }
    class Computer
    {
        CentralProcessingUnit cpu = new CentralProcessingUnit();
        RandomAccessMemory ram = new RandomAccessMemory();
        HardDiskDrive hdd = new HardDiskDrive();
        ComputerCase atxCase = new ComputerCase();
        ComputerPowerSupply power = new ComputerPowerSupply();
        ComputerMonitor monitor = new ComputerMonitor();
        KeyBoard keyBoard = new KeyBoard();
        Mouse mouse = new Mouse();

        public void Assembled()
        {
            cpu.Install();
            ram.Install();
            hdd.Install();
            power.Install();
            atxCase.Install();
            monitor.Install();
            keyBoard.Install();
            mouse.Install();
        }
        public void Start()
        {
            power.Start();
            cpu.Start();
            ram.Start();
            hdd.Start();
            monitor.Start();
            keyBoard.Start();
            mouse.Start();
        }
    }
    class CentralProcessingUnit//CPU
    {
        public void Install()
        {
            Console.WriteLine("安装CPU");
        }
        public void Start()
        {
            Console.WriteLine("CPU启动");
        }
        public void Stop()
        {
            Console.WriteLine("CPU停止");
        }
    }
    class RandomAccessMemory
    {
        public void Install()
        {
            Console.WriteLine("安装大小为8G的内存");
        }
        public void Start()
        {
            Console.WriteLine("内存启动");
        }
        public void Stop()
        {
            Console.WriteLine("内存停止");
        }
    }
    class HardDiskDrive
    {
        public void Install()
        {
            Console.WriteLine("安装大小为1T的硬盘");
        }
        public void Start()
        {
            Console.WriteLine("硬盘启动");
        }
        public void Stop()
        {
            Console.WriteLine("硬盘停止");
        }
    }
    class ComputerCase
    {
        public void Install()
        {
            Console.WriteLine("组装好机箱");
            Console.WriteLine("完成");
        }
        public void PressThePowerButton()
        {
            Console.WriteLine("按下电源键");
        }
    }
    class ComputerPowerSupply
    {
        public void Install()
        {
            Console.WriteLine("安装450W的电源");
        }
        public void Start()
        {
            Console.WriteLine("开启电源，开始供电");
        }
        public void Stop()
        {
            Console.WriteLine("关闭电源");
        }
    }
    class ComputerMonitor
    {
        public void Install()
        {
            Console.WriteLine("插上显示器");
        }
        public void Start()
        {
            Console.WriteLine("点亮屏幕，显示器开始工作");
        }
        public void Stop()
        {
            Console.WriteLine("关闭显示器");
        }
    }
    class KeyBoard
    {
        public void Install()
        {
            Console.WriteLine("插上键盘");
        }
        public void Start()
        {
            Console.WriteLine("键盘灯亮开始工作");
        }
        public void Stop()
        {
            Console.WriteLine("键盘灯熄灭停止工作");
        }
    }
    class Mouse
    {
        public void Install()
        {
            Console.WriteLine("插上鼠标");
        }
        public void Start()
        {
            Console.WriteLine("鼠标开始工作");
        }
        public void Stop()
        {
            Console.WriteLine("鼠标停止工作");
        }
    }
}
