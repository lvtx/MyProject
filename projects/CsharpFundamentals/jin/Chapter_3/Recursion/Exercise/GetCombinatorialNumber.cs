using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Numerics;
using System.Threading.Tasks;

namespace Exercise
{
    class GetCombinatorialNumber
    {
        #region "使用组合数公式来计算组合数"
        /// <summary>
        /// 计算组合数
        /// </summary>
        /// <param name="n"></param>
        /// <param name="k"></param>
        /// <returns></returns>
        static BigInteger CombinatorialNumber(int n, int k)
        {
            BigInteger nN = CalculateN(n);//n的阶乘
            BigInteger kN = CalculateN(k);//k的阶乘
            BigInteger n_kN = CalculateN(n - k);//(n - k)的阶乘
            BigInteger Ckn = nN / (kN * n_kN);//组合数的结果
            return Ckn;
        }
        /// <summary>
        /// 计算阶乘(递推)
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        static BigInteger CalculateN(int n)
        {
            BigInteger N = 1;
            for (long i = 1; i <= n; i++)
            {
                N *= i;
            }
            return N;
        }
        #endregion
        #region "使用杨辉三角计算组合数"
        /// <summary>
        /// 计算杨辉三角第h层第r个数
        /// 第n行的第1个数为1，
        /// 第二个数为1×(n-1)，
        /// 第三个数为1×(n-1)×（n-2）/2，
        /// 第四个数为1×(n-1)×（n-2）/2×（n-3）/3
        /// 以此类推
        /// </summary>
        static BigInteger GetYangHuiTriangleNum(int h, int r)//要计算的层数
        {
            BigInteger ret = 1;
            if (h == 1 || r == 1)
                return ret;
            for (int i = 1; i < r; i++)
            {
                ret = (ret * (h - i)) / i;//使用 *= 无法做到整除
            }
            return ret;
        }
        #endregion
        #region "使用递归的方法用组合数递推公式计算"
        static BigInteger UseRecursionCalculateN(int n, int m)
        {
            if (n == 0)
            {
                return 0;
            }
            if (m == 0)
            {
                return 1;
            }
            if (m == n)
            {
                return 1;
            }
            BigInteger ret;
            ret = UseRecursionCalculateN(n - 1, m - 1) + UseRecursionCalculateN(n - 1, m);
            return ret;
        }
        #endregion
    }
}
