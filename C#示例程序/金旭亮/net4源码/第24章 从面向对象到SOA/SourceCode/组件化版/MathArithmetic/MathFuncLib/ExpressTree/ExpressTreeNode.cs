using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MathArithmetic
{
    //表达式语法树节点
    public class ExpressTreeNode
    {
        //左子树
        public ExpressTreeNode LeftChild = null;
        //右子树
        public ExpressTreeNode RightChild = null;
    }

    //操作数节点
    public class OperandNode : ExpressTreeNode
    {
        //操作数
        public double operand;
        public  OperandNode(double OperandValue)
        {
            operand = OperandValue;
        }

    }

    public class OperatorNode : ExpressTreeNode
    {

        //运算符
        public string operators;

        public  OperatorNode(string OperatorValue)
        {
            operators = OperatorValue;
        }
    }

}
