using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Reflection;

namespace CreationPolicyExample
{

    public interface IMyPart
    {
        void IntroduceMyself();
    }

    [PartCreationPolicy(CreationPolicy.NonShared)]
    [Export]
    public class NonSharedPart : IMyPart
    {
        public void IntroduceMyself()
        {
            Console.WriteLine("NonSharedPart部件标识：{0}", this.GetHashCode());
        }
    }
    [PartCreationPolicy(CreationPolicy.Shared)]
    [Export]
    public class SharedPart : IMyPart
    {
        public void IntroduceMyself()
        {
            Console.WriteLine("SharedPart部件标识：{0}",this.GetHashCode());
        }
    }


    public class MyPart1
    {
        [Import(typeof(NonSharedPart))]
        public IMyPart innerPart;
        [Import(typeof(SharedPart))]
        public IMyPart innerPart2;

    }
    public class MyPart2
    {
        [Import(typeof(NonSharedPart))]
        public IMyPart innerPart;
        [Import(typeof(SharedPart))]
        public IMyPart innerPart2;
    }


    class Program
    {
        static void Main(string[] args)
        {
            MyPart1 obj1 = new MyPart1();
            MyPart2 obj2 = new MyPart2();

            AssemblyCatalog cata = new AssemblyCatalog(Assembly.GetExecutingAssembly());
            CompositionContainer container = new CompositionContainer(cata);
            container.ComposeParts(obj1, obj2);

            obj1.innerPart.IntroduceMyself();
            obj2.innerPart.IntroduceMyself();

            obj1.innerPart2.IntroduceMyself();
            obj2.innerPart2.IntroduceMyself();

            Console.ReadKey();

        }
    }
}
