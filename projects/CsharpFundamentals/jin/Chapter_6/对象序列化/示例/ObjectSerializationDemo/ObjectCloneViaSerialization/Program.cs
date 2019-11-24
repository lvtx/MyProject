using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace ObjectCloneViaSerialization
{
    [Serializable]
    class MyClass
    {
        public int Index = 1;
    }

    class Program
    {
        static void Main(string[] args)
        {
            MyClass obj = new MyClass();
            //创建一个内存流对象
            using (MemoryStream ms = new MemoryStream())
            {
                IFormatter formator = new BinaryFormatter();
                //将对象序列化到内存流中 
                formator.Serialize(ms, obj);  
                //克隆100个对象
                for (int i = 0; i < 100; i++)
                {
                    //回到流的开头
                    ms.Seek(0, SeekOrigin.Begin);
                    //反序列化对象
                    obj = (formator.Deserialize(ms) as MyClass); 
                    obj.Index += i;   //设置对象字段
                    Console.WriteLine("对象{0}已创建。", obj.Index);
                }
            }
            Console.ReadKey();
        }
    }
}
