using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheSameName
{
    class MyClass
    {
        public void Process()
        {
            Console.WriteLine("MyClass.Process()");
        }
    }

   
  
    static class MyClassExtensions
    {
        public static void Process(this MyClass obj){
            Console.WriteLine("MyClassExtensions.Process()");
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            new MyClass().Process();
            Console.ReadKey();
        }
    }
}
