using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PartialClass
{
    public partial class MyPublicClass
    {
        public int j;
        public void OtherFunc()
        {
            System.Console.WriteLine("j={0}", j);
        }
    }

}
