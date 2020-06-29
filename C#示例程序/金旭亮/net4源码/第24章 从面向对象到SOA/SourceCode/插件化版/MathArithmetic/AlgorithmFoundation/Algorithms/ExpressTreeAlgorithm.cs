using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;

namespace MathArithmetic
{
  
    /// <summary>
    /// 使用语法树算法求值
    /// </summary>
    [Export(typeof(IAlgorithm))]
    public class ExpressTreeAlgorithm : IAlgorithm
    {
        /// <summary>
        /// 根据中序表达式计算算法
        /// </summary>
        /// <param name="expr"></param>
        /// <returns></returns>
        public double Calculate(string expr)
        {
            //创建前序表达式
            string PrefixExpr = Converter.InfixToPrefix(expr);
            //创建语法树
            ExpressTreeNode root = ExpressTree.CreateTree(PrefixExpr);
            //求值
            return ExpressTree.EvaluateExprTree(root);
        }

        public string GetAlgorithmName()
        {
            return "ExpressTree";
        }
    }


}
