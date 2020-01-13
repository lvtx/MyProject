using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Encapsulation
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.Write("请输入数字A:");
                string strNumberA = Console.ReadLine();
                Console.Write("请输入数字B:");
                string strNumberB = Console.ReadLine();
                Console.Write("请选择运算符号(+、-、*、/)");
                string strOperate = Console.ReadLine();

                string strResult = "";
                strResult = Convert.ToString(
                    Operation.GetResult(
                        Convert.ToDouble(strNumberA), 
                        Convert.ToDouble(strNumberB), 
                        strOperate));
                Console.Write("最终结果为:{0}", strResult);
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.Write("您输入的结果有误{0}",ex.Message);
            }
        }
    }
}
