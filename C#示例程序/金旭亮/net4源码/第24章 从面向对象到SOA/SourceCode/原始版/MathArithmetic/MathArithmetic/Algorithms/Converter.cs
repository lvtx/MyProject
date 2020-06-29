using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;



namespace MathArithmetic
{
    //    '此类完成中序，前序相互转换的算法
    public class Converter
    {
        //    '中序转前序,为区分开多位整数及小数,采用$将表达式各基本组成成份分开
        public static string InfixToPrefix(string expr)
        {
            if (expr.Trim() == "")
                return "";

            //反转输入字串

            string rExpr = AlgorithmHelper.ReverseExpr(expr);


            //结果字串
            List<string> result = new List<string>();
            //创建数学表达式对象
            MathExpression obj = new MathExpression(rExpr);

            OperandGetter OperandObj = new OperandGetter(obj); //创建提取操作数对象

            Stack<string> OperatorStack = new Stack<string>();//运算符堆栈

            char curChar, topChar;

            //循环访问字符
            while (true)
            {
                curChar = obj.GetCurChar();
                // 可以提取数字
                if (curChar == '{') //反转后的数字均以大括号间隔，所以此处使用左大括号表明即将取出数字
                {
                    //丢弃“{”
                    obj.CurIndex += 1;
                    OperandObj.Run();
                    //将操作数直接追加到输出缓冲区中
                    result.Add(OperandObj.result);
                    //扫描结束
                    if (obj.CurIndex == obj.Expression.Length - 1 && obj.GetCurChar() == '}')
                        break;

                    obj.CurIndex += 1;//丢弃右“}”
                }
                //是右括号,直接压入堆栈
                if (curChar == ')')
                {
                    OperatorStack.Push(curChar.ToString());
                    //增加索引
                    if (obj.CurIndex < obj.Expression.Length - 1)
                        obj.CurIndex += 1;

                }
                //是运算符但不是括号
                if (AlgorithmHelper.IsOperator(curChar) && curChar != ')' && curChar != '(')
                {
                    do
                    {
                        if (OperatorStack.Count == 0)
                        {
                            //堆栈为空,运算符压入堆栈
                            OperatorStack.Push(curChar.ToString());
                            break;
                        }
                        else
                        {
                            topChar = OperatorStack.Peek()[0]; //提取栈顶字符
                            if (topChar == ')')
                            {
                                //堆栈栈顶是右括号,运算符压入堆栈
                                OperatorStack.Push(curChar.ToString());
                                break;
                            }
                            else
                            {
                                //比对优先级
                                if (AlgorithmHelper.IsASameOrHeigherThanB(curChar, topChar))
                                {    //当前运算符比堆栈栈顶运算符优先级高或相等,运算符压入堆栈
                                    OperatorStack.Push(curChar.ToString());
                                    break;
                                }
                                else
                                    //栈顶运算符优先级高,出栈,追加到输出缓冲区
                                    result.Add(OperatorStack.Pop());
                            }

                        }
                    } while (true);


                    //增加索引
                    if (obj.CurIndex < obj.Expression.Length - 1)
                        obj.CurIndex += 1;

                }

                //是左括号,则运算符出栈直到遇见右括号
                if (curChar == '(')
                {
                    topChar = OperatorStack.Peek()[0];
                    while (topChar != ')')
                    {
                        //运算符出栈,追加到输出缓冲区
                        result.Add(OperatorStack.Pop());
                        topChar = OperatorStack.Peek()[0];
                    }
                    //丢弃右括号
                    OperatorStack.Pop();
                    //增加索引
                    if (obj.CurIndex < obj.Expression.Length - 1)
                        obj.CurIndex += 1;
                    else
                        break;

                }

            }

            //扫描完成,如果运算符栈中还有运算符
            while (OperatorStack.Count > 0)
            {
                //运算符出栈,追加到输出缓冲区
                result.Add(OperatorStack.Pop());
            }

            //反转结果并返回,使用$分隔

            StringBuilder buff = new StringBuilder();
            for (int i = result.Count - 1; i >= 0; i--)
            {
                if (i > 0)
                    buff.Append(result[i] + "$");
                else
                    buff.Append(result[i]);//最后一个字符不加$


            }

            return buff.ToString();

        }

        //中序转后序,为区分开多位整数及小数,采用$将表达式各基本组成成份分开
        public static string InfixToPostfix(string expr)
        {
            return ExpressTree.GetPostfixExpr(Converter.CreateExprTreeFromPrefix(Converter.InfixToPrefix(expr)));
        }

        //根据前序表达式创建语法树
        private static ExpressTreeNode CreateExprTreeFromPrefix(string prefixExpr)
        {
            return ExpressTree.CreateTree(prefixExpr);
        }

        //前序转后序
        public static string PrefixToPostfix(string expr)
        {
            return ExpressTree.GetPostfixExpr(Converter.CreateExprTreeFromPrefix(expr));
        }

        //前序转中序
        public static string PrefixToInfix(string expr)
        {
            return ExpressTree.GetInfixExpr(Converter.CreateExprTreeFromPrefix(expr));
        }

        //后序转中序
        public static string PostfixToInfix(string expr)
        {
            throw new CalculatorException("等待你的完成！");
        }

        //后序转前序
        public static string PostfixToPrefix(string expr)
        {
            throw new CalculatorException("等待你的完成！");
        }

    }
}
