using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhatIsRecursion
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("演示开始");
            DonotRunMe();
            Console.WriteLine("演示结束");
        }

        static void DonotRunMe()
        {
            DonotRunMe();
        }
    }
}
