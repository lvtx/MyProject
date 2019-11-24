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
            MyDate d1, d2;//日期1，日期2
            d1.Year = 1999;
            d1.Month = 5;
            d1.Day = 10;
            d2.Year = 2006;
            d2.Month = 3;
            d2.Day = 8;
            CalculateDate obj = new CalculateDate();
            int days = obj.CalculateDaysBetweenTwoDate(d1,d2);
            string str = "{0}年{1}月{2}日到{3}年{4}月{5}日共有天数：";
            str = String.Format(str, d1.Year, d1.Month, d1.Day, d2.Year, d2.Month, d2.Day);
            Console.WriteLine(str + days);
            Console.ReadKey();
        }
    }
}
