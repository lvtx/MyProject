using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MathArithmetic
{
    /// <summary>
    ///预处理类:用于对表达式字串进行初步处理，使其符合一定的规范
    /// </summary>
    public class PreProcess
    {

        /// <summary>
        /// 清除字串中所有的空格
        /// </summary>
        /// <param name="expr"></param>
        /// <returns></returns>
        public static string ClearAllSpace(string expr)
        {
            return expr.Replace(" ", String.Empty);
        }

        /// <summary>
        /// 检查表达式是否是正确的四则运算表达式
        /// </summary>
        /// <param name="expr"></param>
        public static void CheckExprValidate(string expr)
        {
            if (expr.Trim() == "")
                throw new CalculatorException("空表达式。");

            //清除字串中所有的空格
            PreProcess.ClearAllSpace(expr);

            //创建表达式解析对象
            MathExpression obj = new MathExpression() { Expression = expr, CurIndex = 0 };
          
            //用于读取数字的对象
            OperandGetter processor = new OperandGetter(obj);

            bool IsOperator = false; //上一字符是否为运算符
            bool IsLeftParenttheses = false; //上一字符是否为左括号
            bool IsRightParenttheses = false; //上一字符是否为右括号
            bool IsOperand = false; //上一字符是否为数字
            bool hasOperand = false; //是否全由运算符组成
            int ParenthesesCount = 0; //表达式中的括号数

            //不允许出现两个小数点
            int DotIndex = -1;
            DotIndex = obj.Expression.IndexOf("..");
            if (DotIndex >= 0)
                throw new CalculatorException("不能出现连续两个小数点,出错位置：" + (DotIndex + 1).ToString());

            char curChar;
            while (true)
            {
                curChar = obj.GetCurChar();

                //处理小数点,正常情况下，小数点被提取操作数的自动机读取，不应出现当前字符为小数点的情况
                if (curChar == '.')
                    throw new CalculatorException("小数点前必须为数字,出错位置：" + (obj.CurIndex + 1).ToString());
                
                //数字
                if (char.IsDigit(curChar))
                {
                    processor.Run();
                    if (processor.result[processor.result.Length - 1] == '.')
                        throw new CalculatorException("小数点后必须为数字,出错位置：" + (obj.CurIndex + 1).ToString());
                    hasOperand = true;
                    if (IsRightParenttheses == true)
                       throw new CalculatorException("右括号后不能直接跟数字,出错位置：" + (obj.CurIndex + 1).ToString());
                    IsOperand = true;
                    IsOperator = false;//重置运算符标志，以便检测重复的运算符
                    //重置括号标志
                    IsLeftParenttheses = false;
                    IsRightParenttheses = false;
                    //表达式最后一个字符为操作数，应结束循环
                    if (obj.CurIndex == obj.Expression.Length - 1 && char.IsDigit(obj.GetCurChar()))
                        break;
                }
                else     //运算符
                {
                    if (AlgorithmHelper.IsOperator(curChar))
                    {
                        if (obj.CurIndex == 0 && curChar != '(' && curChar != '-')
                            throw new CalculatorException("表达式必须以左括号,减号或数字开头,出错位置：" + (obj.CurIndex + 1).ToString());

                        //处理括号
                        if (curChar == '(')
                        {
                            if (IsOperand)
                                throw new CalculatorException("操作数之后不能直接跟左括号,出错位置：" + (obj.CurIndex + 1).ToString());

                            ParenthesesCount += 1;//括号数加一
                            IsLeftParenttheses = true;
                            IsOperator = false;
                        }
                        if (curChar == ')')
                        {
                            if (IsOperator)
                                throw new CalculatorException("右括号前必须是操作数,出错位置：" + (obj.CurIndex + 1).ToString());

                            ParenthesesCount -= 1; //括号数减一
                            IsRightParenttheses = true;

                        }
                        if (ParenthesesCount < 0)
                            throw new CalculatorException("括号不配对，右括号比左括号多,出错位置：" + (obj.CurIndex + 1).ToString());


                        //处理连续出现两个以上的运算符
                        if (curChar != '(' && curChar != ')' && AlgorithmHelper.IsOperator(curChar))
                        {
                            if (IsLeftParenttheses && curChar != '-')
                                throw new CalculatorException("左括号后不能直接跟减号以外的运算符,出错位置：" + (obj.CurIndex + 1).ToString());

                            if (IsOperator)
                                throw new CalculatorException("不允许出现连续两个运算符。出错位置：" + (obj.CurIndex + 1).ToString());

                            IsLeftParenttheses = false;
                            IsRightParenttheses = false;

                            IsOperator = true;
                        }
                        IsOperand = false;
                    }
                    else
                    {
                        //既不是运算符，又不是数字，非法字符

                        throw new CalculatorException("非法字符。表达式只能由数字和运算符组成,出错位置：" + (obj.CurIndex + 1).ToString());

                    }

                    //增加索引
                    if (obj.CurIndex < obj.Expression.Length - 1)
                        obj.CurIndex += 1;
                    else
                        break;


                }
            }

            if (ParenthesesCount > 0)
                throw new CalculatorException("括号不配对，左括号比右括号多,出错位置：" + (obj.CurIndex + 1).ToString());

            if (hasOperand == false)
                throw new CalculatorException("表达式中至少要有一个操作数,出错位置：" + (obj.CurIndex + 1).ToString());


            char lastChar = obj.Expression[obj.Expression.Length - 1];
            if (AlgorithmHelper.IsOperator(lastChar) && lastChar != ')')
                throw new CalculatorException("表达式不能以运算符结尾,出错位置：" + (obj.CurIndex + 1).ToString());



        }



    }

}
