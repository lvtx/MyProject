using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MathArithmetic
{
    /// <summary>
    /// 算术表达式的算法接口
    /// </summary>
    public interface IAlgorithm
    {

        /// <summary>
        /// 计算出表达式expr的值
        /// </summary>
        /// <param name="expr">要计算的四则运算表达式</param>
        /// <returns></returns>
        double Calculate(string expr);
        /// <summary>
        /// 向外界返回算法的名字
        /// </summary>
        /// <returns></returns>

        string GetAlgorithmName();
    }
}
