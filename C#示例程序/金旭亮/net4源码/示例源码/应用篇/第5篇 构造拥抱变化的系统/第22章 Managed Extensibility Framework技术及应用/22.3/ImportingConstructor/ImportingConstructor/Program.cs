using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition;
using System.Reflection;

namespace ImportingConstructor
{
    class Program
    {
        static void Main(string[] args)
        {
            PartHost host = new PartHost();

            Console.WriteLine(host.part.Information);
            Console.WriteLine(host.part.DefaultTime);
            Console.ReadKey();

        }
    }
}
