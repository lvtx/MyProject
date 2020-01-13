using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inheritance
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("请选择运算符号(+、-、*、/)");
            string strOperate = Console.ReadLine();
            Operation oper = OperationFactory.createOperate(strOperate);
            Console.Write("请输入数字A:");
            oper.NumberA = Convert.ToDouble(Console.ReadLine());
            Console.Write("请输入数字B:");
            oper.NumberB = Convert.ToDouble(Console.ReadLine());
            Console.WriteLine("最终的结果为:{0}", oper.GetResult());
            Console.ReadLine();
        }
    }
}
