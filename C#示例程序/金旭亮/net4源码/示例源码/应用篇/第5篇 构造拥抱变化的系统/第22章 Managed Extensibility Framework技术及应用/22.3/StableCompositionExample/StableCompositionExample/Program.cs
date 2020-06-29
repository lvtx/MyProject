using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Reflection;

namespace StableCompositionExample
{
    #region "接口"
    interface IPart
    {
    }
    interface IPartAPlugIn
    {
    }
    interface IPartBPlugIn
    {
    }
    #endregion

    #region "部件"

    class PartHost
    {
        //为了测试由于插件缺失而导致的“拒绝”部件参与组合过程的情形
        //请注释掉下面的PartBPlugIn类
        [ImportMany(typeof(IPart))]
        public List<IPart> Parts { get; set; }

        //如果要测试部件“供过于求”的情况，注释掉上面的Parts集合
        //使用下面的Part字段。
        //[Import(typeof(IPart))]
        //public IPart part;
    }

    [Export(typeof(IPart))]
    class PartA:IPart
    {
        [Import(typeof(IPartAPlugIn))]
        public IPartAPlugIn PlugIn;
    }

    [Export(typeof(IPart))]
    class PartB:IPart
    {
        [Import(typeof(IPartBPlugIn))]
        public IPartBPlugIn PlugIn;
    }

    [Export(typeof(IPartAPlugIn))]
    class PartAPlugIn : IPartAPlugIn
    {
       
    }

    //为了测试由于插件缺失而导致的“拒绝”部件参与组合过程的情形
    //请注释掉PartBPlugIn类
    [Export(typeof(IPartBPlugIn))]
    class PartBPlugIn : IPartBPlugIn
    {
    }

    #endregion

    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                AssemblyCatalog cata = new AssemblyCatalog(Assembly.GetExecutingAssembly());
                CompositionContainer container = new CompositionContainer(cata);
                PartHost host = new PartHost();
                container.ComposeParts(host);
                Console.WriteLine("组合成功，PartHost包容{0}个部件",host.Parts.Count);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.ReadKey();


        }
    }
}
