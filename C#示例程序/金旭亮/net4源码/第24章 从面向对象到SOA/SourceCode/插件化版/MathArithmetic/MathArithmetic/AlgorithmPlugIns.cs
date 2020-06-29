using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Reflection;

namespace MathArithmetic
{
    public class AlgorithmPlugIns
    {
        [ImportMany(typeof(IAlgorithm))]
        public List<IAlgorithm> AlgorithmObjs; //保存所有的插件

        /// <summary>
        /// 查找所有的插件
        /// </summary>
        public void FindAllAlgorithmObjs()
        {
            DirectoryCatalog dirCatalog = new DirectoryCatalog(Environment.CurrentDirectory);
            AssemblyCatalog assCatalog = new AssemblyCatalog(Assembly.GetExecutingAssembly());
            AggregateCatalog aggCatalog = new AggregateCatalog(dirCatalog, assCatalog);
            var container = new CompositionContainer(aggCatalog);
            container.ComposeParts(this);
        }
    }
}
