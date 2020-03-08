using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MathArithmetic
{

    /// <summary>
    /// 此类用于存放在算法中用到的一些公用函数
    /// </summary>
    public class AlgorithmHelper
    {

        /// <summary>
        /// 判断某字符是否是运算符,是则返回true，其余非法字符返回false
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static bool IsOperator(char c)
        {
            bool result = false;
            switch (c)
            {
                case '+':

                case '-':

                case '*':

                case '/':

                case '(':

                case ')':
                    result = true;
                    break;
                default:
                    result = false;
                    break;
            }
            return result;
        }

        //判断当前字符是否可以取出一个操作数
        public static bool CanExtractAnOperand(MathExpression expr)
        {
            char curChar = expr.GetCurChar();

            bool result = false;
            //如果当前字符为数字，
            if (char.IsDigit(curChar))
                result = true;

            //或者第一个字符为减号，或者减号前为左括号
            if (curChar == '-')
                if (expr.CurIndex == 0)
                    result = true;
                else
                    if (expr.GetChar(expr.CurIndex - 1) == '(')
                        result = true;


            return result;
        }


        /// <summary>
        /// 将一个数学表达式反转,主要用于中缀生成前缀表达式,所有操作数均用大括号分隔
        /// </summary>
        /// <param name="expr"></param>
        /// <returns></returns>
        public static string ReverseExpr(string expr)
        {
            MathExpression obj = new MathExpression(expr);
            OperandGetter OperandObj = new OperandGetter(obj);
            char curChar;
            List<string> result = new List<string>();

            while (true)
            {
                curChar = obj.GetCurChar();
                //是操作数
                if (CanExtractAnOperand(obj))
                {
                    OperandObj.Run();
                    result.Add("{" + OperandObj.result + "}");
                    if ((obj.CurIndex == obj.Expression.Length - 1) && (char.IsDigit(obj.GetCurChar())))
                        break;

                }
                else
                {
                    result.Add(curChar.ToString());
                    if (obj.CurIndex < obj.Expression.Length - 1)
                        obj.CurIndex += 1;
                    else
                        break;
                }

            }


            StringBuilder buff = new StringBuilder();
            for (int i = result.Count - 1; i >= 0; i--)
                buff.Append(result[i]);
            return buff.ToString();

        }


        private static Dictionary<char, int> operands = new Dictionary<char, int>();

        /// <summary>
        /// 判断两运算符A和B的优先级谁高谁低，A优先于B，返回true，其余情况（优先级相等或低），返回false
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        public static bool IsAHeigherThanB(char A, char B)
        {
            if (AlgorithmHelper.IsOperator(A) == false || AlgorithmHelper.IsOperator(B) == false)
                throw new Exception("传入的字符不是合法的运算符。");

            InitializeOperands();

            return operands[A] > operands[B];

        }
        /// <summary>
        /// 初始化运算符优先级集合
        /// </summary>
        private static void InitializeOperands()
        {
            if (operands.Count == 0)
            {
                operands.Add('+', 0);
                operands.Add('-', 0);
                operands.Add('*', 1);
                operands.Add('/', 1);
                operands.Add('(', 2);
                operands.Add(')', 2);
            }
        }

        /// <summary>
        /// 判断两运算符A和B的优先级谁高谁低，A优先于B或与B相等，返回true
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        public static bool IsASameOrHeigherThanB(char A, char B)
        {
            if (AlgorithmHelper.IsOperator(A) == false || AlgorithmHelper.IsOperator(B) == false)
                throw new Exception("传入的字符不是合法的运算符。");
            InitializeOperands();
            return operands[A] >= operands[B];
        }


       
        /// <summary>
        /// 根据两个操作数和运算符,计算其结果
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="Operators"></param>
        /// <returns></returns>
        public static double Evaluate(double x, double y, string Operators)
        {
            double result=0;
            switch (Operators)
            {
                case "+":
                        result = x + y;
                        break;
                case "-":
                        result = x - y;
                        break;
                case "*":
                        result = x * y;
                        break;
                case "/":
                        result = x / y;
                        break;
                
            }
         
            return result;
        }

    }
}
