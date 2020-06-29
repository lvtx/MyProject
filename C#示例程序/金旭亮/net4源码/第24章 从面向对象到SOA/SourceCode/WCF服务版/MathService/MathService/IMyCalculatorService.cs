using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace MathService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IMyCalculatorService" in both code and config file together.
    [ServiceContract]
    public interface IMyCalculatorService
    {
        [OperationContract]
        double Calculator(string Expr);
    }
}
