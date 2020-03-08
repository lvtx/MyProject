using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PartialMethod
{
    class Program
    {
        static void Main(string[] args)
        {
            MyClass obj = new MyClass();

            obj.OridinaryMethod();//调用分部方法
            Console.ReadKey();
        }
    }


   

}
