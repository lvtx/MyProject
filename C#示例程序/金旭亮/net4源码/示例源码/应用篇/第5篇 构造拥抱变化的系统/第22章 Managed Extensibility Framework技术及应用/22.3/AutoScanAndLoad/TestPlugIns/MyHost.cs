using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Reflection;
using PlugInInterface;

namespace TestPlugIns
{
    public class MyHost
    {
        [ImportMany(typeof(IPlugIn))]
        public List<IPlugIn> plugins = new List<IPlugIn>();
        public void Run()
        {
 
         
            var catalog = new DirectoryCatalog("plugins");
            var container = new CompositionContainer(catalog);
            container.ComposeParts(this);

            //使用插件
            foreach (IPlugIn plugin in plugins)
            {
                plugin.Print("Hello!");
            }
           
        }
    }
}
