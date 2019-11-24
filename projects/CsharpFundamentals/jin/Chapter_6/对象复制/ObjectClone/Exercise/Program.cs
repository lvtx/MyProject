using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exercise
{
    class Program
    {
        static void Main(string[] args)
        {
            ClassA ObjA = new ClassA();
            ClassB ObjB = new ClassB();
            ClassC ObjC = new ClassC();
            ObjA.EmbedObjectC = new ClassC();
            ObjB.EmbedObjectA = new ClassA();
            ObjC.EmbedObjectB = new ClassB();
            ClassA OtherA;
            ClassB OtherB;
            ClassC OtherC;
            OtherA = (ObjA as ICloneable).Clone() as ClassA;
            OtherB = (ObjB as ICloneable).Clone() as ClassB;
            OtherC = (ObjC as ICloneable).Clone() as ClassC;
            Console.WriteLine(ObjA.EmbedObjectC == OtherA.EmbedObjectC);
            Console.WriteLine(ObjB.EmbedObjectA == OtherB.EmbedObjectA);
            Console.WriteLine(ObjC.EmbedObjectB == OtherC.EmbedObjectB);
        }
    }
    class ClassA:ICloneable
    {
        public int valueA = 100;
        public ClassC EmbedObjectC = new ClassC();
        public object Clone()
        {
            ClassA ObjA = new ClassA();
            ObjA.EmbedObjectC = (this.EmbedObjectC as ICloneable) as ClassC;
            return ObjA;
        }
    }
    class ClassB:ICloneable
    {
        public int ValueB = 200;
        public ClassA EmbedObjectA = new ClassA();
        public object Clone()
        {
            ClassB ObjB = new ClassB();
            ObjB.EmbedObjectA = (this.EmbedObjectA as ICloneable) as ClassA;
            return ObjB;
        }
    }
    class ClassC:ICloneable
    {
        public int valueC = 300;
        public ClassB EmbedObjectB = new ClassB();
        public object Clone()
        {
            ClassC ObjC = new ClassC();
            ObjC.valueC = this.valueC;
            ObjC.EmbedObjectB = (this.EmbedObjectB as ICloneable).Clone() as ClassB;
            return ObjC;
        }
    }
}
