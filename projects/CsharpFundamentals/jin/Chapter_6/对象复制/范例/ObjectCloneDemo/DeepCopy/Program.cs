using System;

namespace DeepCopy
{
    class ClassA : ICloneable
    {
        public int AValue = 100;
        public ClassB EmbedObject;    //ClassA包容一个ClassB的对象
        public Object Clone()
        {
            ClassA ObjA = new ClassA();
            ObjA.AValue = this.AValue;
            ObjA.EmbedObject = 
                (this.EmbedObject as ICloneable).Clone() as ClassB;
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
    class Program
    {
        static void Main(string[] args)
        {
            ClassA ObjA = new ClassA();
            ObjA.EmbedObject = new ClassB();
            //开始克隆
            ClassA other = (ObjA as ICloneable).Clone() as ClassA;

            Console.WriteLine(other.EmbedObject == ObjA.EmbedObject);   //false
            Console.ReadKey();
        }
    }
}
