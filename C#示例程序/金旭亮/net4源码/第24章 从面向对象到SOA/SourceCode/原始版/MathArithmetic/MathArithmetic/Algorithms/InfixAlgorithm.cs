using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MathArithmetic
{
    /// <summary>
    /// 中缀算法
    /// </summary>
    public class InfixAlgorithm : IAlgorithm
    {


        //运算符堆栈
        private Stack<string> OperatorStack = new Stack<string>();


        //操作数堆栈
        private Stack<string> OperandStack = new Stack<string>();


        private OperandGetter OperandObj; //提取操作数对象

        /// <summary>
        /// 表达式求值
        /// </summary>
        /// <param name="expr"></param>
        /// <returns></returns>
        public double Calculate(string expr)
        {
            //对表达式字串进行预处理
            expr = PreProcess.ClearAllSpace(expr);

            //创建数学表达式对象
            MathExpression exprobj = new MathExpression();
            exprobj.Expression = expr;
            exprobj.CurIndex = 0;

            //清空堆栈
            OperandStack.Clear();
            OperatorStack.Clear();

            //创建取操作数对象
            OperandObj = new OperandGetter(exprobj);

            char curChar;//当前正在处理的字符
            char tempOperator; //保存运算符
          

            //开始扫描字串
            while (exprobj.CurIndex < exprobj.Expression.Length)
            {
                curChar = exprobj.GetCurChar();

                if (AlgorithmHelper.CanExtractAnOperand(exprobj))  //是操作数
                {
                    OperandObj.Run(); //提取操作数
                    //将操作数入栈
                    OperandStack.Push(OperandObj.result);
                    //当字符索引是最后一个字符,且为数字时,表示这是表达式的最后一个数字,可以
                    //结束扫描了,否则,表示后面还有有效字符
                    if (exprobj.CurIndex == exprobj.Expression.Length - 1 && char.IsDigit(exprobj.GetCurChar()))
                        break; //已到字串末尾
                }
                else
                {
                    //是运算符
                    if (AlgorithmHelper.IsOperator(curChar))
                    {
                        //栈空，则直接入栈
                        if (OperatorStack.Count == 0)
                            OperatorStack.Push(curChar.ToString());
                        else //栈不空
                        {
                            tempOperator = OperatorStack.Peek()[0];
                            if (curChar == ')')
                                //出栈并计算,直到遇见"("
                                DoWithRightParentheses();
                            else
                                //不是")"
                                if (AlgorithmHelper.IsAHeigherThanB(curChar, tempOperator) || tempOperator == '(')
                                    //当前运算符比栈顶的高,进栈
                                    OperatorStack.Push(curChar.ToString());
                                else
                                {
                                    //只要当前运算符比栈顶的优先级低或相等，就不停地出栈并计算
                                    do
                                    {
                                        DoCalculate(); //使用两个堆栈进行计算
                                        if (OperatorStack.Count == 0)  //运算符堆栈为空？
                                            break;
                                        tempOperator = OperatorStack.Peek()[0];//取栈顶运算符
                                    } while (!AlgorithmHelper.IsAHeigherThanB(curChar, tempOperator));
                                    //当前运算符进栈
                                    OperatorStack.Push(curChar.ToString());
                                }
                        }
                    }

                    //增加字符索引,获取新的字符
                    if (exprobj.CurIndex < exprobj.Expression.Length - 1)
                        exprobj.CurIndex += 1;
                    else
                        break; //已到字串末尾


                }

            }

            //        '字符串扫描完成,向外界返回表达式最终计算结果
            return GetResult();

        }

        /// <summary>
        /// 向外界报告本算法的名字
        /// </summary>
        /// <returns></returns>
        public string GetAlgorithmName()
        {
            return "Infix";
        }



        //获取表达式最终计算结果
        private double GetResult()
        {
            //字符串扫描完成
            if (OperatorStack.Count == 0)
            {
                //            如果运算符栈空,表明计算结束,输出最终结果
                if (OperandStack.Count == 0)
                    throw new CalculatorException("运算数栈为空");

                return Convert.ToDouble(OperandStack.Pop());
            }
            else
            {
                //            '如果运算符栈不空,开始计算最终结果
                while (OperatorStack.Count > 0)
                {
                    //使用两个堆栈进行计算
                    DoCalculate();
                }
                //          运算符栈空,表明计算结束,输出最终结果
                return Convert.ToDouble(OperandStack.Pop());
            }
        }

        // 处理右圆括号,需要出栈并计算部分表达式内容
        private void DoWithRightParentheses()
        {
            char tempOperator;
            //获取栈顶运算符
            tempOperator = OperatorStack.Peek()[0];
            //出栈并计算,直到遇见"("
            while (tempOperator != '(')
            {
                //使用两个堆栈进行计算
                DoCalculate();
                //获取新栈顶运算符,
                tempOperator = OperatorStack.Peek()[0];
            }
            //找到了"("
            OperatorStack.Pop();
        }

        // 从运算符栈中弹出一个运算符,从操作数栈中弹出两个操作数进行计算,
        // 结果入操作数栈
        private void DoCalculate()
        {
            char tempOperator;
            double Operand1, Operand2;

            //获取出栈顶运算符
            tempOperator = OperatorStack.Pop()[0];

            //取出两个操作数
            Operand2 = Convert.ToDouble(OperandStack.Pop());
            Operand1 = Convert.ToDouble(OperandStack.Pop());
            //运算结果入栈
            OperandStack.Push(AlgorithmHelper.Evaluate(Operand1, Operand2, tempOperator.ToString()).ToString());

        }




    }

}
