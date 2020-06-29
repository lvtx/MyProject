using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MathArithmetic;

namespace MathArithmetic
{
    //   '表达式类
    public class ExpressTree
    {
        //    '根据前序表达式创建表达式语法树,返回根节点
        //    '正则表达式以$间隔开各组成元素
        //    '如果表达式参数为空,返回Nothing
        public static ExpressTreeNode CreateTree(string prefixExpr)
        {
            ExpressTreeNode Root = null;
            ExpressTreeNode TempNode = null;

            string[] tokens = prefixExpr.Split('$');//将表达式分割,存入数组中

            //        '将数组元素反转(因为前序表达式要求从右到左读取,所以先反转,这样就可以从左到右读取表达式,比较合乎习惯
            Array.Reverse(tokens);


            Stack<ExpressTreeNode> OperandStack = new Stack<ExpressTreeNode>();

            //        '处理前序表达式
            foreach (string s in tokens)
            {
                if (char.IsDigit(s, 0) || s.StartsWith("-") && s.Length > 1)
                {
                    //                '是操作数,创建一个操作数节点,压入堆栈中
                    TempNode = new OperandNode(Convert.ToDouble(s));
                    OperandStack.Push(TempNode);
                }
                else
                {
                    //是运算符,创建一个运算符节点
                    TempNode = new OperatorNode(s);
                    //出栈两个操作数,作为运算符节点的子树
                    TempNode.LeftChild = OperandStack.Pop();
                    TempNode.RightChild = OperandStack.Pop();
                    //将运算符节点存回堆栈
                    OperandStack.Push(TempNode);
                }
            }
            //正常情况下,这时堆栈中只有一个元素
            if (OperandStack.Count > 1 || OperandStack.Count == 0)
                throw new CalculatorException("非法的前序表达式。");

            //取出树的根节点返回
            Root = OperandStack.Pop();
            return Root;
        }

        //    '根据语法树,中序遍历生成中序表达式
        public static string GetInfixExpr(ExpressTreeNode root)
        {
            if (root == null)
                throw new CalculatorException("语法树对象为空引用。");


            string result = "";

            OperatorNode LeftOperator, RightOperator;


            //        '左子树不为空
            if (!(root.LeftChild == null))
            {
                //如果左子树也是运算符节点
                if (root.LeftChild.GetType().Name == "OperatorNode")
                {
                    LeftOperator = root.LeftChild as OperatorNode;
                    //如果当前节点运算符优先级比子树节点优先级高,则应加上括号 
                    if (AlgorithmHelper.IsAHeigherThanB((root as OperatorNode).operators[0], LeftOperator.operators[0]))
                    {
                        result += "(";
                        result += GetInfixExpr(root.LeftChild); //递归访问左子树
                        result += ")";
                    }
                    else
                        //优先级等于或低于子树运算符优先级,不加括号
                        result += GetInfixExpr(root.LeftChild); //递归访问左子树

                }
                else
                    //左子树为操作数节点
                    result += GetInfixExpr(root.LeftChild); //递归访问左子树


                result += (root as OperatorNode).operators; //输出根节点的运算符

                //如果右子树也是运算符节点
                if (root.RightChild.GetType().Name == "OperatorNode")
                {
                    RightOperator = root.RightChild as OperatorNode;
                    //如果当前节点运算符优先级比子树节点优先级高,则应加上括号 
                    if (AlgorithmHelper.IsAHeigherThanB((root as OperatorNode).operators[0], RightOperator.operators[0]))
                    {
                        result += "(";
                        result += GetInfixExpr(root.RightChild); //递归访问右子树
                        result += ")";
                    }
                    else
                        //优先级等于或低于子树运算符优先级,不加括号
                        result += GetInfixExpr(root.RightChild);//递归访问右子树

                }
                else
                    //右子树为操作数节点
                    result += GetInfixExpr(root.RightChild); //递归访问右子树

            }
            else
            {

                //左子树为空,由于语法树的节点要不为叶子节点,要不为有两个子树的子节点
                //所以可以判断出此节点为操作数节点
                result += (root as OperandNode).operand;
            }

            return result;
        }

        //    '根据语法树生成前序表达式
        public static string GetPrefixExpr(ExpressTreeNode root)
        {
            if (root == null)
                throw new CalculatorException("语法树对象为空引用。");
            string result = "";

            //输出当前节点
            if (root.GetType().Name == "OperatorNode")
                result += (root as OperatorNode).operators;
            else
                result += (root as OperandNode).operand;


            if (!(root.LeftChild == null))
            {
                //递归处理左子树
                result += "$" + GetPrefixExpr(root.LeftChild);
                //递归处理右子树
                result += "$" + GetPrefixExpr(root.RightChild);
            }

            return result;
        }

        //    '由语法树创建后序表达式
        public static string GetPostfixExpr(ExpressTreeNode root)
        {
            if (root == null)
                throw new CalculatorException("语法树对象为空引用。");
            string result = "";

            if (!(root.LeftChild == null))
            {
                //递归处理左子树
                result += GetPostfixExpr(root.LeftChild);
                //递归处理右子树
                result += GetPostfixExpr(root.RightChild);
            }

            //输出当前节点
            if (root.GetType().Name == "OperatorNode")
                result += "$" + (root as OperatorNode).operators;
            else
                result += "$" + (root as OperandNode).operand;


            return result;
        }

        //    '表达式语法树求值
        public static double EvaluateExprTree(ExpressTreeNode root)
        {
            if (root == null)
                throw new CalculatorException("语法树对象为空引用。");

            //是操作数节点,肯定是叶子节点
            if (root.GetType().Name == "OperandNode")
                return (root as OperandNode).operand;


            //是操作符节点
            double x, y;
            x = EvaluateExprTree(root.LeftChild);
            y = EvaluateExprTree(root.RightChild);

            string curOperator;
            double result = 0;
            curOperator = (root as OperatorNode).operators;
            result = AlgorithmHelper.Evaluate(x, y, curOperator);

            return result;

        }

    }
}
