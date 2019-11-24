using System;
using System.Windows;
using System.Data;

namespace HelloCsharp
{
    class Program
    {
        static void Main(string[] args)
        {
            TestInputAndOutput();
            Console.ReadKey();
        }
        static void TestConsoleProperty()
        {
            Console.Title = "Current Time:" + DateTime.Now;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.BackgroundColor = ConsoleColor.Green;
        }
        static void TestInputAndOutput()
        {
            Console.Write("Please input a string:");//光标直接跳到冒号后面
            string userInput = Console.ReadLine();//等待用户输入一串字符
            Console.WriteLine("user input " + userInput);
        }
    }
}
