using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WhatIsMEF2
{
    class Program
    {
        static void Main(string[] args)
        {
            MyHost host = new MyHost();
            host.Run();

            Console.ReadKey();
        }
    }
}
