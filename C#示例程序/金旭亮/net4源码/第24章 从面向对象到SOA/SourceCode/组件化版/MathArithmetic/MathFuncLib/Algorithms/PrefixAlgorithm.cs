using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MathArithmetic
{
    //    '前序表达式计算法
    public class PrefixAlgorithm : IAlgorithm
    {
       private Stack<double> OperandStack = new Stack<double>(); //前序表达式字串只需要一个操作数堆栈

        //根据中序表达式计算出结果
        public double Calculate(string expr)
        {
            string prefixExpr = Converter.InfixToPrefix(expr);
            return CalculateByPrefixExpr(prefixExpr);
        }

        // 传入前序表达式(以$间隔),计算出结果
        public double CalculateByPrefixExpr(string prefixExpr)
        {



            double num1, num2, tempResult;

            string[] tokens = prefixExpr.Split('$'); //将表达式分割,存入数组中
            //将数组元素反转(因为前序表达式要求从右到左读取,所以先反转,这样就可以从左到右读取表达式,比较合乎习惯
            Array.Reverse(tokens);

            //处理前序表达式
            foreach (string s in tokens)
            {
                if (char.IsDigit(s[0]) || (s.StartsWith("-") && s.Length > 1))
                    //是操作数,则压入堆栈
                    OperandStack.Push(Convert.ToDouble(s));
                else
                {
                    //出栈两个操作数
                    num1 = OperandStack.Pop();
                    num2 = OperandStack.Pop();
                    tempResult = AlgorithmHelper.Evaluate(num1, num2, s);
                    //将计算结果存回堆栈
                    OperandStack.Push(tempResult);
                }
            }
            //计算完成
            if (OperandStack.Count > 1)
                throw new CalculatorException("无效的前序表达式");

            //返回计算结果
            return OperandStack.Pop();
        }




        /// <summary>
        /// 向外界报告本算法的名字
        /// </summary>
        /// <returns></returns>
        public string GetAlgorithmName()
        {
            return "Prefix";
        }

       
    }
}
