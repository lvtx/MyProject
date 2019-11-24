using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ObjectSerialization
{
    class Program
    {
        static void Main(string[] args)
        {
            var serializer = new XmlSerializer(typeof(MyClass));
            MyClass obj = new MyClass()
            {
                IntField = 100,
                StringField = "Hello"
            };
            serializer.Serialize(Console.Out, obj);
            Console.ReadLine();
        }
    }
    public class MyClass
    {
        public int IntField;
        public string StringField;
    }
}
