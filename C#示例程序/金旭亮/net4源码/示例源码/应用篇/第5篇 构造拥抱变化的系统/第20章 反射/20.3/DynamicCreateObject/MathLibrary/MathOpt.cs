using System;
using System.Collections.Generic;
using System.Text;
using InterfaceLibrary;

namespace MathLibrary
{

    public class AddClass : IMathOpt
    {
        double IMathOpt.GetIt(double x, double y)
        {
           return x+y;
        }
    }

    public class SubtractClass:IMathOpt
    {
        double IMathOpt.GetIt(double x, double y)
        {
            return x - y;
        }
    }

    public class MultiplyClass : InterfaceLibrary.IMathOpt
    {
        double IMathOpt.GetIt(double x, double y)
        {
            return x * y;
        }
    }

    public class DivideClass : InterfaceLibrary.IMathOpt
    {
        double IMathOpt.GetIt(double x, double y)
        {
            return x / y;
        }
    }
}
