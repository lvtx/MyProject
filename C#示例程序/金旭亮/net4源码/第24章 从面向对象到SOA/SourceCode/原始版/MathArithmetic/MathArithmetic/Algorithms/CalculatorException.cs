using System;
namespace MathArithmetic
{
    /// <summary>
    /// �Զ����쳣 
    /// </summary>
    public class CalculatorException : ApplicationException
    {
        public CalculatorException(string Info)
            : base(Info)
        {
        }
    }
}
