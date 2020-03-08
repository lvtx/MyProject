using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OptionalArguments
{
    class Program
    {
        static void Main(string[] args)
        {
            Test(100);
            Test(100, "New Value");
        }

        static void Test(int required, string optionalString = "Default Value")
        {
        }
    }
}
