using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculatorDaysUseDotNet
{
    class Program
    {
        static void Main(string[] args)
        {
            DateTime d1 = new DateTime(1999, 5, 10);
            DateTime d2 = new DateTime(2006, 3, 8);
            //计算结果
            double days = (d2 - d1).TotalDays; 

            string str = "{0}年{1}月{2}日到{3}年{4}月{5}日共有天数：";
            str = String.Format(str, d1.Year, d1.Month, d1.Day, d2.Year, d2.Month, d2.Day);
            Console.WriteLine(str + days);

            //暂停,敲任意键结束
            Console.ReadKey();
        }
    }
}
