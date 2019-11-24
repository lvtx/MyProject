using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inherit
{
    #region "访问继承的成员"
    class Parent
    {
        public string field1 = "Parent Class field";
        public void Method1(string value)
        {
            Console.WriteLine("父类的方法{0}", value);
        }
    }
    class Child : Parent
    {
        public string field2 = "Child Class field";
        public void Method2(string value)
        {
            Console.WriteLine("子类的方法{0}",value);
        }
    }
    #endregion
}
