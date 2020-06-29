using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Collections;
using System.Reflection;

namespace ExportMetadataExample
{
    public interface IMyPart
    {
        void IntroduceMySelf();
    }

    [Export(typeof(IMyPart))]
    [ExportMetadata("PartName", "MyPart")]
    public class MyPart:IMyPart
    {
        public void IntroduceMySelf()
        {
            Console.WriteLine("MyPart");
        }
    }

    [Export(typeof(IMyPart))]
    [ExportMetadata("PartName","MyOtherPart")]
    public class MyOtherPart : IMyPart
    {
        public void IntroduceMySelf()
        {
            Console.WriteLine("MyOtherPart");
        }
    }


    public class MyHost
    {
        
        [ImportMany]
        private IEnumerable<Lazy<IMyPart, IDictionary<string, object>>> Parts;

        public IMyPart GetPart(string PartName)
        {
            foreach (var part in Parts)
                if (part.Metadata["PartName"].Equals(PartName))
                    return part.Value;
            return null;
        }

    }

    class Program
    {
        static void Main(string[] args)
        {
            AssemblyCatalog cata = new AssemblyCatalog(Assembly.GetExecutingAssembly());
            CompositionContainer container = new CompositionContainer(cata);
            MyHost host = new MyHost();
            container.ComposeParts(host);
            IMyPart part = host.GetPart("MyPart");
            if (part != null)
                part.IntroduceMySelf();
            Console.ReadKey();

        }
    }
}
