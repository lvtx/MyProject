using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ObjectComparer
{
    class MyClassComparer : IComparer<MyClass>
    {
        public int Compare(MyClass x, MyClass y)
        {
            return x.Value.CompareTo(y.Value);
        }
    }
}
