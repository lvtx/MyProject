using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhatIsDelegate
{
    delegate void MyDel(int value);//声明委托类型
    class Program
    {
        static void Main(string[] args)
        {
            Random random = new Random();
            int randValue = random.Next(1, 100);
            MyDel del;
            del = (randValue <= 50) 
                ? new MyDel(Program.PrintLow) 
                : new MyDel(Program.PrintHigh);
            del(randValue);
            Console.ReadKey();
        }
        static void PrintLow(int value)
        {
            Console.WriteLine("{0}-Low Value",value);
        }
        static void PrintHigh(int value)
        {
            Console.WriteLine("{0}-High Value",value);
        }
    }
}
