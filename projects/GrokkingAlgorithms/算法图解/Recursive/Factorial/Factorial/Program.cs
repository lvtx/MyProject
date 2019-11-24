using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Factorial
{
    class Program
    {
        static void Main(string[] args)
        {
            MathematicalFunction mathematicalFunction = new MathematicalFunction();
            Console.WriteLine(mathematicalFunction.Fact(5));
        }
    }

    class MathematicalFunction
    {
        public int Fact(int x)
        {
            if(x == 1)
            {
                return 1;
            }
            return x * Fact(x - 1);
        }
    }
}
