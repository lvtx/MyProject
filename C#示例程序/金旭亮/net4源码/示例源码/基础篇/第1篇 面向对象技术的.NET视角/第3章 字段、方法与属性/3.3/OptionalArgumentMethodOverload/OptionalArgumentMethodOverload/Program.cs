using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OptionalArgumentMethodOverload
{
    class Program
    {
        static void Main(string[] args)
        {
            M(100);
            Console.ReadKey();
        }

        static void M(string s, int i = 1)
        {
            Console.WriteLine(" M(string s, int i = 1)");
        }

        static void M(object o)
        { 
            Console.WriteLine("M(object o)");
        }

        static void M(int i, string s = "Hello")
        {
            Console.WriteLine(" M(int i, string s = \"Hello\")");
        }

        static void M(int i)
        { 
            Console.WriteLine("M(int i)");
        }

    }
}
