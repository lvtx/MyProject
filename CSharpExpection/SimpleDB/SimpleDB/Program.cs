using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace SimpleDB
{
    class Program
    {
        static void Main(string[] args)
        {
            Add();
            Add();
            Console.ReadLine();
        }
        static void Add()
        {
            MyClass m = new MyClass();
            Console.WriteLine("count + 1 = {0}",m.count + 1);
            int a = 1;
            Console.WriteLine("a + 1 = {0}",a + 1);
        }
    }
    class MyClass
    {
        public int count = 1;
    }
}
