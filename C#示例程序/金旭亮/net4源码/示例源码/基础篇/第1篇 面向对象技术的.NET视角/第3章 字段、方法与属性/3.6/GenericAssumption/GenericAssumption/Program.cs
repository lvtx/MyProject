using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericAssumption
{
    class Program
    {
        static void Main(string[] args)
        {
            DateTime.Now.Test();
            Console.ReadKey();
        }
    }

    static class MyExtensionMethods
    {
        public static void Test<T>(this T obj)
        {
            Console.WriteLine(obj.ToString());
        }
    }
}
