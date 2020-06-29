using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.IO;


namespace ObjectClone2
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
                formator.Serialize(ms, obj);  //将对象序列化到内存流中 

                //克隆100个对象
                for (int i = 0; i < 100; i++)
                {
                    ms.Seek(0, SeekOrigin.Begin);//回到流的开头
                    obj = (formator.Deserialize(ms) as MyClass); //反序列化对象
                    obj.Index += i;   //设置对象字段
                    Console.WriteLine("对象{0}已创建。", obj.Index);
                }
            }
            Console.ReadKey();
        }
    }
}
