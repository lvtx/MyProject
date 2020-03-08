using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace LambdaExpressionTree
{
    class Program
    {
        static void Main(string[] args)
        {
            //构造(x, y) => x + y 的Lambda表达式树
            //创建两个参数节点对象
            ParameterExpression leftOperand = Expression.Parameter(typeof(int), "x");
            ParameterExpression rightOperand = Expression.Parameter(typeof(int), "y");
            //创建“+”节点对象，得到“x + y”所对应的子树
            BinaryExpression tree = Expression.Add(leftOperand, rightOperand);
            //组合构造完整的Lambda表达式树：(x, y) => x + y
            var expr = Expression.Lambda<Func<int, int, int>>(tree, leftOperand,
                    rightOperand);
            int sum = expr.Compile()(100, 200);  //输出：sum=300
            Console.WriteLine("Lambde表达式树：{0}\n值：{1}", expr, sum);
            Console.ReadKey();
        }
    }
}
