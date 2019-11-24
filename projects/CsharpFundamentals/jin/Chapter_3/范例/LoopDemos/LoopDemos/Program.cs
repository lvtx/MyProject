using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LoopDemos
{
    class Program
    {
        static void Main(string[] args)
        {
            //SumFrom1To100();

            //InputQuitToStop();
            //SumFrom1To100UseFor();

            //BreakAndContinue();
            //SumUseInfinteLoop();

            // ForEachDataCollection();

            //testKey();

            //QuitConsoleApp();
           // DisableControlC();
            UseCancelKeyPress();

            Console.WriteLine("\n演示结束，敲任意键退出……");
            Console.ReadKey();
        }

        #region "循环结构示例"
        /// <summary>
        /// 使用While循环计算“1+2+...+100=?”
        /// </summary>
        static void SumFrom1To100()
        {
            //此变量将用于保存求和结果
            int sum = 0;
            //此变量将作为循环变量
            int counter = 1;
            //只要counter变量的值小于100，就执行循环体语句
            //counter从1到100，循环体语句一共执行了100次
            while (counter <= 100)
            {
                //使用sum保存每次累加的结果
                sum = sum + counter;
                //上述代码可以简写为：
                //sum += counter;
                //循环变量自增
                counter++;
            }
            //输出结果
            Console.WriteLine(sum);
        }

        /// <summary>
        /// 使用特殊值结束输入
        /// </summary>
        static void InputQuitToStop()
        {
            string userInput = "";

            while (userInput.ToLower() != "quit")
            {
                Console.WriteLine("\n不断输入字符串，回车结束一次输入。不想再运行程序时，输入quit。");
                userInput = Console.ReadLine();
                if (string.IsNullOrEmpty(userInput) == false)
                {
                    Console.WriteLine("您输入了：{0}", userInput);
                }
            }
            Console.WriteLine("\n---------------------------------------------");
            Console.WriteLine("\n检测到quit命令，循环中止，敲任意键退出……");
        }

        /// <summary>
        /// 使用For循环计算“1+2+...+100=?”
        /// </summary>
        static void SumFrom1To100UseFor()
        {
            //此变量将用于保存求和结果
            int sum = 0;
            for (int i = 1; i <= 100; i++)
            {
                sum += i;
            }
            //输出结果
            Console.WriteLine(sum);
        }

        /// <summary>
        /// 理解Break和Continue的不同作用
        /// </summary>
        static void BreakAndContinue()
        {
            for (int i = 1; i <= 10; i++)
            {
                if (i == 5)
                {
                    //切换以下两句的注释，体会它们的不同作用
                    //continue;
                    break;

                }
                Console.WriteLine("第{0}轮循环", i);
            }
            
        }
        /// <summary>
        /// 使用死循环计算整数和
        /// </summary>
        static void SumUseInfinteLoop()
        {
            long sum = 0;
            int counter = 1;
            while (true)
            {
                sum += counter;
                counter++;
                if (counter > 100000)
                    break;
            }
            Console.WriteLine(sum);
        }
        /// <summary>
        /// 使用ForEach循环遍历数据集合
        /// </summary>
        static void ForEachDataCollection()
        {
            Console.WriteLine("遍历整数集合:\n");
            
            var IntValues = new List<int>() { 1, 2, 3, 4 };
            foreach (var value in IntValues)
            {
                //取消以下注释，尝试在遍历过程中从集合中移除数据，
                //将会遇到“System.InvalidOperationException”异常
                //if (value == 2)
                //    IntValues.Remove(value);
                Console.WriteLine(value);
            }
            Console.WriteLine("\n遍历对象集合:\n");
            var MyClasses = new List<MyClass>();
            //向集合中追加5个对象
            for (int i = 0; i < 5; i++)
            {
                MyClasses.Add(new MyClass()
                {
                    Id = i,
                    Description = "MyClass对象" + i
                });
            }
            
            foreach (var obj in MyClasses)
            {
                Console.WriteLine("{0}:{1}", obj.Id, obj.Description);
            }
        }

        #endregion

        #region "控制台应用编程小技巧"
        //测试如何检测按键
        static void testKey()
        {
            Console.WriteLine("随意敲任意键查看其键值，压ESC退出");
            ConsoleKeyInfo key;
            do
            {
                //用户敲了按键了吗？
                while (!Console.KeyAvailable)
                {
                    //啥也不干，等待……
                }
                //等待用户击键
                key = Console.ReadKey(true);
                Console.WriteLine();//输出一个空行
                Console.WriteLine("Modifiers值={0}", key.Modifiers);
                Console.WriteLine("KeyChar值={0}", (int)(key.KeyChar));
                Console.WriteLine("Key值={0}", key.Key);
                //CapsLock这个键是不能被捕获的，但我们可以检测出键盘的状态
                if (Console.CapsLock)
                {
                    Console.WriteLine("处于大写状态");
                }
                //NumberLock这个键是不能被捕获的，但我们可以检测出键盘的状态
                if (Console.NumberLock)
                {
                    Console.WriteLine("小键盘上的Num Lock键被按下");
                }
                //检测控制键
                if (key.Modifiers != 0)
                {
                    if ((key.Modifiers & ConsoleModifiers.Alt) != 0)
                    {
                        Console.WriteLine("Alt键被按下");
                    }
                    if ((key.Modifiers & ConsoleModifiers.Control) != 0)
                    {
                        Console.WriteLine("Ctrl键被按下");
                    }
                    if ((key.Modifiers & ConsoleModifiers.Shift) != 0)
                    {
                        Console.WriteLine("Shift键被按下");
                    }
                }
            } while (key.Key != ConsoleKey.Escape);
            Console.WriteLine("\n检测到ESC键，敲任意键退出……\n");
        }
        /// <summary>
        /// 展示系统内置的强制中止控制台程序的功能
        /// </summary>
        static void QuitConsoleApp()
        {
            Console.WriteLine("死循环：请使用Ctrl+C或Ctrl+Break强制中止本程序");
            while (true)
            {
                Console.WriteLine("当前时间：" + DateTime.Now.ToLocalTime());
                Thread.Sleep(2000);
            }
        }
        /// <summary>
        /// 禁用Ctrl+C
        /// </summary>
        static void DisableControlC()
        {
            Console.WriteLine("本程序只能通过ESC键结束，无法通过Ctrl+C而中止");
            Console.TreatControlCAsInput = true;
            do
            {
                var key = Console.ReadKey(true);

                if (key.Key == ConsoleKey.Escape)
                {
                    Console.WriteLine("检测到ESC键，敲任意键退出……");
                    break;
                }
            } while (true);

        }

        /// <summary>
        /// 响应UseCancelKeyPress事件，屏蔽掉Ctrl+C和Ctrl+Break
        /// </summary>
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
