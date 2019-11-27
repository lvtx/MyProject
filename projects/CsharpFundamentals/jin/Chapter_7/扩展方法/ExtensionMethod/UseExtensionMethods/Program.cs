using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseExtensionMethods
{
    static class PersonExtensions 
    { 
        public static void SayHello(this Person obj)
        {
            Console.WriteLine("{0} say : Hello", obj.name);
        }
        public static void GetYouName(this Person obj)
        {
            Console.WriteLine("She/He is {0}",obj.name);
        }
    }

    class Person
    {
        public string name { get; set; }
        public int age { get; set; }
    }
    class Program
    {
        static void Main(string[] args)
        {
            Person person = new Person();
            person.name = "Tom";
            person.SayHello();
            person.GetYouName();
            Console.ReadLine();
        }
    }
}
