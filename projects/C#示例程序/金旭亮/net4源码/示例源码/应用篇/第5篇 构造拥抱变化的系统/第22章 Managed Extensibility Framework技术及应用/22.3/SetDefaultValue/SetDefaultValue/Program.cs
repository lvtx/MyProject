using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using System.Reflection;
using System.ComponentModel.Composition.Hosting;

namespace SetDefaultValue
{
    class Program
    {
        static void Main(string[] args)
        {
            AssemblyCatalog cata = new AssemblyCatalog(Assembly.GetExecutingAssembly());
            CompositionContainer container = new CompositionContainer(cata);
            try
            {
                PartHost host=new PartHost();
                container.ComposeParts(host);
                Console.WriteLine("部件成功组合。PartHost.Part=null? {0}",host.Part==null);
            }
            catch (Exception ex)
            {
                Console.WriteLine("{0}:{1}", ex.GetType(), ex.Message);
            }
           
            Console.ReadKey();

        }
    }

    public interface IPart
    {
    }

    //如果注释掉MyPart类，则PartHost类中的[Import]的AllowDefault要等于true
    //否则会引发一个ChangeRejectedException异常
    [Export(typeof(IPart))]
    public class MyPart : IPart
    {
    }

    public class PartHost
    {
         [Import(typeof(IPart))]  //如果MyPart类被注释掉了，此导入将引发MEF抛出ChangeRejectedException异常，需改用下面的导入设置
        //[Import(typeof(IPart),AllowDefault=true)]
        public IPart Part=null;
    }
}
