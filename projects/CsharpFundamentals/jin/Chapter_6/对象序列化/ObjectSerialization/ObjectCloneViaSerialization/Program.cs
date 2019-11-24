using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace ObjectCloneViaSerialization
{
    class Program
    {
        static void Main(string[] args)
        {
            MyClass obj = new MyClass();
            using(MemoryStream ms = new MemoryStream())
            {
                IFormatter formator = new BinaryFormatter();
                formator.Serialize(ms, obj);
                for (int i = 0; i < 50; i++)
                {
                    ms.Seek(0, SeekOrigin.Begin);
                    obj = (formator.Deserialize(ms) as MyClass);
                    obj.Index += i;
                    Console.WriteLine("已创建{0}对象",obj.Index);
                }
            }
            Console.ReadLine();
        }
    }
    [Serializable]
    class MyClass
    {
       public int Index = 1;
    }
}
