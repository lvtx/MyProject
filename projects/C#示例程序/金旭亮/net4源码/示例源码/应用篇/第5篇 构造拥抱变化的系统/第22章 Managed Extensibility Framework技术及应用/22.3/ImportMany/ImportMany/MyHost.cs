using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Reflection;

namespace WhatIsMEF2
{
    public class MyHost
    {
        [ImportMany(typeof(IPlugIn))]
        public List<IPlugIn> plugins = new List<IPlugIn>();
        public void Run()
        {
           
            

           Assembly ExecutingAssembly = System.Reflection.Assembly.GetExecutingAssembly();
           var catalog = new AssemblyCatalog(ExecutingAssembly);
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
