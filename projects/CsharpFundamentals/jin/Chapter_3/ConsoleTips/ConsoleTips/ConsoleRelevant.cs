using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace ConsoleTips
{
    public class ConsoleRelevant
    {
        #region "测试按键"
        public static void TestKey()
        {
            Console.WriteLine("敲击任意键查看其键值，压ESC退出");
            ConsoleKeyInfo key;
            do
            {
                //等待用户敲击按键
                while (!Console.KeyAvailable)
                {
                    //死循环，等待
                }
                key = Console.ReadKey(true);//将按键传入key变量
                Console.WriteLine();
                Console.WriteLine("Modifies值={0}",key.Modifiers);
                Console.WriteLine("KeyChar值={0}",(int)key.KeyChar);
                Console.WriteLine("Key值={0}",key.Key);
                //CapsLock键不能被捕获，但是可以检测到
                if (Console.CapsLock)
                {
                    Console.WriteLine("处于大写状态");
                }
                if (Console.NumberLock)
                {
                    Console.WriteLine("小键盘上的Num Lock键被按下");
                }
                //检测控制键
                if (key.Modifiers != 0)
                {
                    if((key.Modifiers & ConsoleModifiers.Alt) != 0)
                    {
                        Console.WriteLine("Alt键被按下");
                    }
                    if ((key.Modifiers & ConsoleModifiers.Control) != 0)
                    {
                        Console.WriteLine("Ctrl键被按下");
                    }
                    if((key.Modifiers & ConsoleModifiers.Shift) != 0)
                    {
                        Console.WriteLine("Shift键被按下");
                    }
                }
            } while (key.Key != ConsoleKey.Escape);
            Console.WriteLine("检测到Esc键，敲击任意键退出...");
        }
        #endregion

        #region "展示系统内置的强制中止控制台程序的功能"
        public static void QuitConsoleApp()
        {
            Console.WriteLine("死循环：请使用Ctrl+C或Ctrl+Break强制中止本程序");
            while (true)
            {
                Console.WriteLine("当前时间：" + DateTime.Now.ToLocalTime());
                Thread.Sleep(2000);
            }
        }
        #endregion

        #region "禁用Ctrl+C"
        public static void DisableControlC()
        {
            Console.WriteLine("本程序只能通过ESC键结束，无法通过Ctrl+C而中止");
            Console.TreatControlCAsInput = true;
            do
            {
                var key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.Escape)
                {
                    Console.WriteLine("检测到esc键，敲任意键退出……");
                    break;
                }
            } while (true);
        }
        #endregion

        #region "响应UseCancelKeyPress事件，屏蔽掉Ctrl+C和Ctrl+Break"
        static void UseCancelKeyPress()
        {
            Console.WriteLine("本程序只能通过ESC键结束");
            //响应CancelKeyPress事件（即Ctrl+C和Ctrl+Break被按下）
            Console.CancelKeyPress += Console_CancelKeyPress;

            do
            {
                var key = Console.ReadKey(true);

                if (key.Key == ConsoleKey.Escape)
                {
                    Console.WriteLine("\n检测到ESC键，敲任意键退出……");
                    break;
                }
                if (key.KeyChar != '\0')
                {
                    if (key.Key == ConsoleKey.Enter)
                    {
                        Console.WriteLine();
                    }
                    else
                    {
                        Console.Write(key.KeyChar);
                    }
                }
            } while (true);

        }
        private static void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            var isCtrlC = e.SpecialKey == ConsoleSpecialKey.ControlC;
            var isCtrlBreak = e.SpecialKey == ConsoleSpecialKey.ControlBreak;
            //如果是Ctrl+C和Ctrl+Break
            if (isCtrlC || isCtrlBreak)
            {
                //屏蔽掉它们，让它们不起作用
                e.Cancel = true;
                //通知用户，Ctrl+C和Ctrl+Break已经不起作用了……
                Console.WriteLine(isCtrlC ? "Ctrl+C已被屏蔽" : "Ctrl+Break已被屏蔽");
            }
        }
        #endregion
    }
}
