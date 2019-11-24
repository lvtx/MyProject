using ClassLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseClassLib
{
    class Program
    {
        static void Main(string[] args)
        {
            MyClass obj = new MyClass();
            int result = obj.DoubleValue(1);
            if (result == 2)
            {
                Console.WriteLine("The Code is Correct.");
            }
            else
            {
                Console.WriteLine("The Code is not Correct");
            }
            Console.ReadKey();
        }
    }
}
