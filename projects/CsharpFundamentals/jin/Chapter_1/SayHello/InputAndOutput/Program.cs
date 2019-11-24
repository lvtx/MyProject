using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InputAndOutput
{
    class Program
    {
        static void Main(string[] args)
        {
            TestConsoleProperty();
            Console.Write("Hello World!");//属性的定义要放在之前
            TestInputAndOutput();//测试输入输出方法 
            TestBeep();
            Console.ReadKey();
        }
        static void TestConsoleProperty()/*Console的重要属性*/
        {
            Console.BackgroundColor = ConsoleColor.Red;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Title = "Hello" + DateTime.Now;
        }
        //控制台窗口的输入与输出
        static void TestInputAndOutput()
        {
            Console.Write("user input a string:");//结尾不换行
            //输入
            string userInput = Console.ReadLine();
            //输出
            Console.WriteLine("user input " + userInput);
            Console.WriteLine("user input {0} length:{1}",userInput,userInput.Length);//使用占位符
        }
        //ReadKey的基本用法
        static void TestReadKey()
        {
            Console.WriteLine("First");
            Console.ReadKey();
            Console.WriteLine("Second");
            Console.ReadKey(true);//用户的输入不显示在屏幕上
            Console.WriteLine("Last");
            Console.ReadKey();
        }
        //Beep的使用
        static void TestBeep()
        {
            Console.Beep();
            Console.WriteLine("Press any key to quit ...");
        }
    }
}
