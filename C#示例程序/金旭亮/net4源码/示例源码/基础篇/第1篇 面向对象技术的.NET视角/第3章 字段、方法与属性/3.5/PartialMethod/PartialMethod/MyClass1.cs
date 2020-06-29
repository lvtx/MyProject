using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PartialMethod
{
   

    partial class MyClass
    {
        partial void PartialMethod(); //分部方法声明

        public void OridinaryMethod()  //普通的方法，
        {
            PartialMethod();　//调用分部方法
            Console.WriteLine("MyClass.OridinaryMethod()");
        }
    }
}
