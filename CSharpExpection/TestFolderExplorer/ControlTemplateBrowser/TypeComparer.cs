using System;
using System.Collections.Generic;

namespace ControlTemplateBrowser
{
    internal class TypeComparer : IComparer<Type>
    {
        public int Compare(Type x, Type y)
        {
            if (x.Name.CompareTo(y.Name) != 0)
            {
                return x.Name.CompareTo(y.Name);
            }
            else
                return 0;
        }
    }
}