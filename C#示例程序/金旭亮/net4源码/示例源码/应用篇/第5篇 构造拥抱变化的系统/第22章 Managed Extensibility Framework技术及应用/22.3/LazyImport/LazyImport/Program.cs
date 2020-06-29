using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Reflection;

namespace LazyImport
{
    
    /// <summary>
    /// 部件接口
    /// </summary>
    public interface IMyPart
    {
        void IntroduceMySelf();
    }

    /// <summary>
    /// 部件
    /// </summary>
    [Export(typeof(IMyPart))]
    public class MyPart : IMyPart
    {
        public void IntroduceMySelf()
        {
            Console.WriteLine("I'm an instance of MyPart.");
        }
    }
    /// <summary>
    /// 部件宿主
    /// </summary>
    public class MyHost
    {
        [Import]
        private Lazy<IMyPart> part;

        public void Introduce()
        {
            if (part.Value != null)
                part.Value.IntroduceMySelf();
            
        }

        public MyHost()
        {
            AssemblyCatalog cata = new AssemblyCatalog(Assembly.GetExecutingAssembly());
            CompositionContainer container = new CompositionContainer(cata);
            container.ComposeParts(this);
        }
    }

   

    class Program
    {
        static void Main(string[] args)
        {
            MyHost host = new MyHost();
            host.Introduce();
            Console.ReadKey();
        }
    }
}
