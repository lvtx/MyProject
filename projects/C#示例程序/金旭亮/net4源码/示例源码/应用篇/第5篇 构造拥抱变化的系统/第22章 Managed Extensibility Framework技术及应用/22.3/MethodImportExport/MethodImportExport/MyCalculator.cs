using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;

namespace MethodImportExport
{
    public class MyCalculator
    {
        [Import(typeof(Func<int, int, long>))]
        public Func<int, int, long> Processor;

        public long Calculate(int num1, int num2)
        {
            if (Processor == null)
                throw new NullReferenceException("未装载任何插件");
            return Processor(num1, num2);
        }
        public MyCalculator()
        {
            var catalog = new TypeCatalog(typeof(AddProcessor));
            var container = new CompositionContainer(catalog);
            container.ComposeParts(this);
        }
    }

}
