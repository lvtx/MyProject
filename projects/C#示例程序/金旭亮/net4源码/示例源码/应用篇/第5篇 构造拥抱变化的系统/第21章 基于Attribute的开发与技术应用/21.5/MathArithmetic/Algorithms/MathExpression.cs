//''算术表达式类
//''将表达式看成是一个字串数组，通过下标来访问特定的字符。
using System;
namespace MathArithmetic
{
    public class MathExpression
    {
        //四则运算表达式
        private string _expr;
        public string Expression
        {
            get
            {
                return _expr;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new Exception("空的算术表达式");
                }
                _expr = value;
            }
        }

        //字串中的当前字符索引
        private int _index = 0;
        public int CurIndex
        {
            get
            {
                return _index;
            }
            set
            {
                if (value < 0)
                    throw new Exception("字符索引数必须大于0");

                if (value >= _expr.Length)
                    throw new Exception("字符索引数必须小于字串长度");

                _index = value;
            }
        }


        /// <summary>
        /// 根据当前索引返回当前字符，不修改索引位置
        /// </summary>
        /// <returns></returns>
        public char GetCurChar()
        {
            if (CurIndex >= _expr.Length)
                throw new ArgumentOutOfRangeException("索引超过了表达式的字符串长度");

            return _expr[CurIndex];
        }


        /// <summary>
        /// 根据字符索引返回特定的字符
        /// </summary>
        /// <param name="index">字符索引</param>
        /// <returns></returns>
        public char GetChar(int index)
        {
            if (index < 0 || index > _expr.Length - 1)

                throw new CalculatorException("字符索引越限");

            if (_expr.Length == 0)
                throw new NullReferenceException("字串为空");

            return _expr[index];
        }

        public MathExpression(string expr)
        {
            _expr = expr;
            _index = 0;
        }

        public MathExpression()
        {
            _expr = "";
            _index = 0;
        }

    }
}