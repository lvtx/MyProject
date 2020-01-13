using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inheritance
{
    //先是一个父类然后多个子类
    //一个工厂类选择需要的子类
    public class Operation
    {
        private double _numberA;

        public double NumberA
        {
            get { return _numberA; }
            set { _numberA = value; }
        }
        private double _numberB;

        public double NumberB
        {
            get { return _numberB; }
            set { _numberB = value; }
        }

        public virtual double GetResult()
        {
            double result = 0d;            
            return result;
        }
    }
    class OperationAdd : Operation
    {
        public override double GetResult()
        {
            double result = 0;
            return result = NumberA + NumberB;
        }
    }
    class OperationSub : Operation
    {
        public override double GetResult()
        {
            double result = 0;
            return result = NumberA - NumberB;
        }
    }
    class OperationMul : Operation
    {
        public override double GetResult()
        {
            double result = 0;
            return result = NumberA * NumberB;
        }
    }
    class OperationDiv : Operation
    {
        public override double GetResult()
        {
            double result = 0;
            if (NumberB == 0)
            {
                throw new Exception("除数不能为0");
            }
            return result = NumberA / NumberB;
        }
    }
    public class OperationFactory
    {
        public static Operation createOperate(string operate)
        {
            Operation oper = null;
            switch (operate)
            {
                case "+": oper = new OperationAdd(); break;
                case "-": oper = new OperationSub(); break;
                case "*": oper = new OperationMul(); break;
                case "/": oper = new OperationDiv(); break;
            }
            return oper;
        }
    }
}
