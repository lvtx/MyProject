using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaticAndInstanceField
{
    class Program
    {
        static void Main(string[] args)
        {
        }
        
    }
    class MyClass
    {
        private static int m = 10;
        private int n = 10;
        public static void StaticMethod()
        {
            m = 30;
        }
        public void InstanceMethod()
        {
            m = 20;
            n = 10;
        }
    }
}
