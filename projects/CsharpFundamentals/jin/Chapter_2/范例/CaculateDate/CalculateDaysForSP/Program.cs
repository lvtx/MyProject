using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculateDaysForSP
{
    class Program
    {
        //存放每月天数，第一个元素为0是因为数组下标从0起，而我们希望按月份直接获取天数
        static int[] months = { 0, 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };

        static void Main(string[] args)
        {
            MyDate d1, d2;     //起始日期和结束日期
            //1999年5月10日
            d1.Year = 1999;
            d1.Month = 5;
            d1.Day = 10;
            //2006年3月8日
            d2.Year = 2006;
            d2.Month = 3;
            d2.Day = 8;
            //计算结果
            int days = CalculateDaysOfTwoDate(d1, d2);

            string str = "{0}年{1}月{2}日到{3}年{4}月{5}日共有天数：";
            str = String.Format(str, d1.Year, d1.Month, d1.Day, d2.Year, d2.Month, d2.Day);
            Console.WriteLine(str + days);

            //暂停,敲任意键结束
            Console.ReadKey();
        }


        //计算两个日期中的整天数
        static int CalculateDaysOfTwoDate(MyDate beginDate, MyDate endDate)
        {
            int days = 0;
            days = CalculateDaysOfTwoYear(beginDate.Year, endDate.Year);
            if (beginDate.Year == endDate.Year)

                days += CalculateDaysOfTwoMonth(beginDate, endDate, true);
            else
                days += CalculateDaysOfTwoMonth(beginDate, endDate, false);

            return days;
        }

        //计算两年之间的整年天数，不足一年的去掉
        static int CalculateDaysOfTwoYear(int beginYear, int endYear)
        {
            int days = 0;
            for (int i = beginYear + 1; i <= endYear - 1; i++)
            {
                if (IsLeapYear(i))
                    days += 366;
                else
                    days += 365;
            }
            return days;
        }


        //根据两个日期，计算出这两个日期之间的天数
        static int CalculateDaysOfTwoMonth(MyDate beginDate, MyDate endDate, bool IsInOneYear)
        {
            int days = 0;
            //对于同一月，天数直接相减
            if (beginDate.Month == endDate.Month)
                if (IsInOneYear)
                    return endDate.Day - beginDate.Day;
                else
                    if (IsLeapYear(beginDate.Year))
                    return 366 + (endDate.Day - beginDate.Day);
                else
                    return 365 + (endDate.Day - beginDate.Day);

            //不同月
            int i = 0;
            if (IsInOneYear)
            {
                //同一年
                for (i = beginDate.Month; i <= endDate.Month; i++)
                {
                    days += months[i];
                    //处理闰二月
                    if ((IsLeapYear(beginDate.Year) && (i == 2)))
                        days += 1;
                }

                //减去月初到起始日的天数
                days -= beginDate.Day;
                //减去结束日到月底的天数
                days -= months[endDate.Month] - endDate.Day;
            }
            else
            {
                //不同年
                //计算到年底的天数
                for (i = beginDate.Month; i <= 12; i++)
                    days += months[i];

                //减去月初到起始日的天数
                days -= beginDate.Day;
                //从年初到结束月的天数
                for (i = 1; i <= endDate.Month; i++)
                    days += months[i];

                //减去结束日到月底的天数
                days -= months[endDate.Month] - endDate.Day;
            }
            return days;
        }

        //根据年数判断其是否为闰年
        static bool IsLeapYear(int year)
        {
            //如果年数能被400整除，是闰年
            if (year % 400 == 0)
            {
                return true;
            }
            //能被4整除，但不能被100整除，是闰年
            if (year % 4 == 0 && year % 100 != 0)
            {
                return true;
            }
            //其他情况，是平年
            return false;
        }
    }
}
