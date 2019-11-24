using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Method
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine("{0} + {1} = {2}", 100, 200, Add(100, 200));
            //Add(1, 2);
            //Add(1d, 3d);
            //Add("1", "2");
            Console.ReadKey();
        }
        #region "调用Add()方法"
        static int Add(int x,int y)
        {
            Console.WriteLine("调用Add(int x,int y)");
            return x + y;
        }
        #endregion

        #region "方法的重载"
        static double Add(double x,double y)
        {
            Console.WriteLine("调用Add(double x,double y)");
            return x + y;
        }
        static double Add(string x,string y)
        {
            Console.WriteLine("调用Add(string x, string y)");
            return double.Parse(x) + double.Parse(y);
        }
        #endregion
    }
}
