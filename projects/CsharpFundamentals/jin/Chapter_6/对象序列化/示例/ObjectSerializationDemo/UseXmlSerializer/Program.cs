using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace UseXmlSerializer
{
    public class MyClass
    {
        public int IntField ;
     
        public string StringField ;
      
    }
    class Program
    {
        static void Main(string[] args)
        {
            var Serializer = new XmlSerializer(typeof(MyClass));

            MyClass obj = new MyClass() { 
                IntField = 100, 
                StringField = "Hello" };
            Serializer.Serialize(Console.Out, obj);
            Console.ReadKey();
        }
    }
}
