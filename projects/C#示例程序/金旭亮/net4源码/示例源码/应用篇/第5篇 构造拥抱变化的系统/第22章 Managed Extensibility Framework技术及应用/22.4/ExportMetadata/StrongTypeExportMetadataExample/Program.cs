using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Reflection;

namespace StrongTypeExportMetadataExample
{
    public interface IMyPart
    {
        void IntroduceMySelf();
    }

    [MyExport(PartName="MyPart")]
    public class MyPart : IMyPart
    {
        public void IntroduceMySelf()
        {
            Console.WriteLine("MyPart");
        }
    }

    [MyExport(PartName="MyOtherPart")]
    public class MyOtherPart : IMyPart
    {
        public void IntroduceMySelf()
        {
            Console.WriteLine("MyOtherPart");
        }
    }

    /// <summary>
    /// 部件元数据视图
    /// </summary>
    public interface IMyMetadataView
    {
        string PartName { get; }
    }

    public class MyHost
    {

        [ImportMany]
        private IEnumerable<Lazy<IMyPart, IMyMetadataView>> Parts;

        public IMyPart GetPart(string PartName)
        {
            foreach (var part in Parts)
                if (part.Metadata.PartName==PartName)
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
