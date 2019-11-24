using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inherit
{
    class Program
    {
        static void Main(string[] args)
        {
            #region "访问继承的成员"
            Child child = new Child();
            child.Method1(child.field1);
            child.Method1(child.field2);
            child.Method2(child.field1);
            child.Method2(child.field2);
            #endregion
            Console.ReadKey();
        }
    }
}
