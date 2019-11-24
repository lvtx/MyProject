using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectComparer
{
    class MyClassComparer:IComparer<MyClass>
    {
        public int Compare(MyClass objA,MyClass objB)
        {
            return objA.value.CompareTo(objB.value);
        }
    }
}
