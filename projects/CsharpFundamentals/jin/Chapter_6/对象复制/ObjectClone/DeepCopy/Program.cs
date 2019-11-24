using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepCopy
{
    class Program
    {
        static void Main(string[] args)
        {
            ClassA ObjA = new ClassA();
            ObjA.EmbedObject = new ClassB();
            ClassA other = (ObjA as ICloneable).Clone() as ClassA;
            Console.WriteLine(other.EmbedObject == ObjA.EmbedObject);
            Console.ReadKey();
        }
    }
    class ClassA : ICloneable
    {
        public int AValue = 100;
        public ClassB EmbedObject;
        public Object Clone()
        {
            ClassA ObjA = new ClassA();
            ObjA.AValue = this.AValue;
            ObjA.EmbedObject = (this.EmbedObject as ICloneable).Clone() as ClassB;
            return ObjA;
        }

    }
    class ClassB : ICloneable
    {
        public int BValue = 200;
        public Object Clone()
        {
            ClassB ObjB = new ClassB();
            ObjB.BValue = this.BValue;
            return ObjB;
        }
    }
}
