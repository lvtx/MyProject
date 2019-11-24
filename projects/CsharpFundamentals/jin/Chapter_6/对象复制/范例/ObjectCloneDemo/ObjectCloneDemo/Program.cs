using System;

namespace ObjectCloneDemo
{
    #region "支持内容比对的简单对象"
    class MyClass
    {
        private int MyClassValue = 100;
        public override string ToString()
        {
            return "MyClassValue:" + MyClassValue;
        }
        public override bool Equals(object obj)
        {
            if (obj == null || obj is MyClass == false)
            {
                return false;
            }
            return MyClassValue == (obj as MyClass).MyClassValue;
        }
        public override int GetHashCode()
        {
            return MyClassValue;
        }

        public static MyClass CloneObject(MyClass obj)
        {
            MyClass newObj = new MyClass();
            newObj.MyClassValue = obj.MyClassValue;		//字段复制
            return newObj;
        }

    }
    #endregion

    #region "组合的对象"
    class ClassB
    {
        public int BValue = 200;
    }
    class ClassA
    {
        public int AValue = 100;
        public ClassB EmbedObject;    //ClassA包容一个ClassB的对象
        public ClassA()
        {
            EmbedObject = new ClassB();		//创建被包容对象
        }
        public static ClassA CloneObject(ClassA obj)
        {
            ClassA newObj = new ClassA();
            newObj.AValue = obj.AValue;		//字段复制
            newObj.EmbedObject = obj.EmbedObject;	//引用复制
            return newObj;
        }

    }
    #endregion

    class Program
    {
        static void Main(string[] args)
        {
            //ObjectCopyViaFieldCopy();
            ObjectCopyViaFieldCopy2();

            Console.ReadKey();
        }
        /// <summary>
        /// 基于字段值拷贝的简单对象复制
        /// </summary>
        static void ObjectCopyViaFieldCopy()
        {
            MyClass obj = new MyClass();
            MyClass other = MyClass.CloneObject(obj);
            Console.WriteLine("原对象：{0},新对象：{1}", obj, other);
            Console.WriteLine(obj == other);
            Console.WriteLine(obj.Equals(other));
        }

        /// <summary>
        /// 基于字段值拷贝的组合对象复制
        /// </summary>
        static void ObjectCopyViaFieldCopy2()
        {
            ClassA obj = new ClassA();
            ClassA other = ClassA.CloneObject(obj);
            Console.WriteLine(obj == other);
            Console.WriteLine(obj.AValue == other.AValue);
            Console.WriteLine(obj.EmbedObject == other.EmbedObject);
            Console.WriteLine(obj.EmbedObject.BValue == other.EmbedObject.BValue);

        }


    }
}
