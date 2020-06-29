using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using MathArithmetic;

namespace MathService
{
    public class MyCalculatorService : IMyCalculatorService
    {
        public double Calculator(string expression)
        {

            PreProcess.CheckExprValidate(expression);
            InfixAlgorithm obj = new InfixAlgorithm();
            return obj.Calculate(expression);
      }

    }
}
