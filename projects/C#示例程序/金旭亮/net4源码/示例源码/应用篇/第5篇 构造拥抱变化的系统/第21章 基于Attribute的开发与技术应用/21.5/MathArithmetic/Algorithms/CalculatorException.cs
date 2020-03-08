using System;
namespace MathArithmetic
{
    /// <summary>
    /// 自定义异常 
    /// </summary>
    public class CalculatorException : ApplicationException
    {
        public CalculatorException(string Info)
            : base(Info)
        {
        }
    }
}
