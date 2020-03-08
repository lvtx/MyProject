using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestPlugIns
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
