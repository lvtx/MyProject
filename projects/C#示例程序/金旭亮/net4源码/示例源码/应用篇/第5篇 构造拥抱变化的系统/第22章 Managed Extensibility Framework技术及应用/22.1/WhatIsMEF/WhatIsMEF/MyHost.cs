using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Reflection;

namespace WhatIsMEF1
{
    public class MyHost
    {
        [Import(typeof(IPlugIn))]
        public IPlugIn plugin = null;
        public void Run()
        {
            //---------------------------------------
            //第一种方式：直接实例化部件对象，加入到部件容器中
            //
            //创建部件容器 
            var container = new CompositionContainer();
            //向部件容器中同时加入部件和部件使用者
            container.ComposeParts(this, new MyPlugIn());
             //---------------------------------------

            //第二种方式：使用Catalog来实现组合：
            //提取定义在当前程序集中的有导入与导出的类型，关联到部件容器中
            Assembly ExecutingAssembly = System.Reflection.Assembly.GetExecutingAssembly();
            var catalog = new AssemblyCatalog(ExecutingAssembly);
            var container = new CompositionContainer(catalog);
            container.ComposeParts(this);
            //---------------------------------------

            //使用部件
            if (plugin != null)
                plugin.Print("Hello!");

        }
    }
}
