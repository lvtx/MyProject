using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recursion
{
    /// <summary>
    /// 阶乘计算
    /// </summary>
    class Factorial
    {
        public static long Calcuate(long n)
        {
            if(n == 1)
            {
                return 1;
            }
            else
            {
                return Calcuate(n - 1) * n;
            }
        }
        public static long Calcuate2(long n)
        {
            long ret = 1;
            for (int i = 1; i <= n; i++)
            {
                ret *= i;
            }
            return ret;
        }
    }
}
