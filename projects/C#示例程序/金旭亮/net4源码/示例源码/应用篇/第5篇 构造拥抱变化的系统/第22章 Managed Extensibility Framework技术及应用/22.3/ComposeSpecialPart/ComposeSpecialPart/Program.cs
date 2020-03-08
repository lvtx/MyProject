using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Reflection;

namespace ComposeSpecialPart
{
    class Program
    {
        
        static void Main(string[] args)
        {
            AssemblyCatalog cata = new AssemblyCatalog(Assembly.GetExecutingAssembly());
            CompositionContainer container = new CompositionContainer(cata);
            PartHost host=new PartHost();
            container.ComposeParts(host);
            Console.WriteLine(host.part.GetName());
            Console.ReadKey();

        }
    }

    public interface IPart
    {
        string GetName();
    }

    [Export("General",typeof(IPart))]
    class Part1 : IPart
    {
        public string GetName()
        {
            return "General";
        }
    }
    [Export("Special", typeof(IPart))]
    class Part2 : IPart
    {
        public string GetName()
        {
            return "Special";
        }
    }

    class PartHost
    {
        [Import("Special")]
        public IPart part=null;
    }
}
