using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Reflection;

namespace ImportingConstructor
{
    class PartHost
    {
        [Import]
        public MyPart2 part;

        public PartHost()
        {
            AssemblyCatalog cata = new AssemblyCatalog(Assembly.GetExecutingAssembly());
            CompositionContainer container = new CompositionContainer(cata);
            container.ComposeParts(this);

        }
    }
}
