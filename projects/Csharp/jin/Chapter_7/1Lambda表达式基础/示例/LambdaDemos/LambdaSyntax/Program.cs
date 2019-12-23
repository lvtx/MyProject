using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3_LambdaSyntax
{
    class Program
    {
        static void Main(string[] args)
        {
            Func<int, bool> del1 = x => x > 0;
            Func<int, int, bool> del2 = (x, y) => x == y;
            Func<int, string, bool> del3 = (int x, string s) => s.Length > x;
            Action del4 = () => { Console.WriteLine("No arguments"); };



            Console.WriteLine(del1(100));  //true;

            Console.WriteLine(del2(100, 200));  //false

            Console.WriteLine(del3(5, "Hello"));  //false

            del4();//No arguments
        }
    }
}
