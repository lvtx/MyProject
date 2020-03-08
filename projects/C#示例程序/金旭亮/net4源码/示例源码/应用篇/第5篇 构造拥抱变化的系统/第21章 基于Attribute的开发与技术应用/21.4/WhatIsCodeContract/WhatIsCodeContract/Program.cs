using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using System.Diagnostics;

namespace WhatIsCodeContract
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(DoSomething(100, 0));
            Console.WriteLine(DoSomething(-100, 2));
            Console.ReadKey();
        }

        static double result = 0;
        static double DoSomething(double divisor, double dividend)
        {
            //if (divisor < 0 || dividend <= 0)
            //    throw new Exception("除数与被除数必须大于0,并且除数不能为0");
            //Debug.Assert(divisor >= 0 && dividend > 0, "除数与被除数必须大于0,并且除数不能为0");

            Contract.Requires(divisor >= 0 && dividend > 0, "除数与被除数必须大于0,并且除数不能为0");
            Contract.Ensures(result > 0,"非法数据，必须为一个大于0的实数");
            result = divisor / dividend;
           // Debug.Assert(result > 0, "非法数据，必须为一个大于0的实数");
            return result;
        }
    }
}
