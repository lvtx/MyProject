using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NamedArgument
{
    class Program
    {
        static void Main(string[] args)
        {
            //传统方式
            SomeMethod(100, 200, 300, 400);
            //命名方式
            SomeMethod(x1: 100, y1: 300, x2: 200, y2: 400);
            //混合方式
            SomeMethod(100, y2: 400, x2: 200, y1: 300);
        }

        static void SomeMethod(int x1, int x2, int y1, int y2)
        {
        }
    }
}
