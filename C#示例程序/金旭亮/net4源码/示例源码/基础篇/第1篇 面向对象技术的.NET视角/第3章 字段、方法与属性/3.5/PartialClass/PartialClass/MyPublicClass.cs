using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PartialClass
{
    public partial class MyPublicClass
    {
        public int i;
        public void Func()
        {
            System.Console.WriteLine("i={0}", i);
        }
    }

}
