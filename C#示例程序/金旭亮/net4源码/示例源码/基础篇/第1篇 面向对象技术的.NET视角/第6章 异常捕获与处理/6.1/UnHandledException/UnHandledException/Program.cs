using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnHandledException
{
    class Program
    {
        static void Main(string[] args)
        {
            int[] values = new int[10];
            for(int i=1;i<=10;i++)
                Console.WriteLine(values[i]);
            Console.ReadKey();
        }
    }
}
