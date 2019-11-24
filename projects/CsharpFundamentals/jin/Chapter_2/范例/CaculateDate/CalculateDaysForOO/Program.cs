using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculateDaysForOO
{
    class Program
    {
        static void Main(string[] args)
        {
            MyDate d1, d2;  //起始日期和结束日期

            //1999年5月10日
            d1.Year = 1999;
            d1.Month = 5;
            d1.Day = 10;
            //2006年3月8日
            d2.Year = 2006;
            d2.Month = 3;
            d2.Day = 8;

            string str = "{0}年{1}月{2}日到{3}年{4}月{5}日共有天数：";
            str = String.Format(str, d1.Year, d1.Month, d1.Day, d2.Year, d2.Month, d2.Day);
            
            //创建类CalculateDate的对象，让变量obj引用它
            DateCalculator obj = new DateCalculator(); 
            //调用对象obj的CalculateDaysOfTwoDate方法计算
            int days = obj.CalculateDaysOfTwoDate(d1, d2);

            Console.WriteLine(str + days);

            //暂停,敲任意键结束
            Console.ReadKey();
        }
    }
}
