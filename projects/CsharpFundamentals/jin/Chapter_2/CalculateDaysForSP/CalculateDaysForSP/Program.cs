using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculateDaysForSP
{
    class Program
    {
        static int[] months = { 0, 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };
        static void Main(string[] args)
        {
            MyDate d1, d2;//日期1，日期2
            int days;
            d1.Year = 1999;
            d1.Month = 5;
            d1.Day = 10;
            d2.Year = 2006;
            d2.Month = 3;
            d2.Day = 8;
            days = CalculateDaysBetweenTwoDate(d1, d2);
            string str = "从{0}年{1}月{2}日到{3}年{4}月{5}日一共有{6}天";
            str = String.Format(str, d1.Year, d1.Month, d1.Day, d2.Year, d2.Month, d2.Day, days);
            Console.WriteLine(str);
            Console.WriteLine("Press any key to quit ...");
            Console.ReadKey();
        }
        static int CalculateDaysBetweenTwoDate(MyDate beginDate, MyDate endDate)
        {
            int days = 0;
            days = CalculateDaysBetweenTwoYear(beginDate.Year, endDate.Year);
            days += CalculateDaysBetweenTwoMonth(beginDate, endDate);
            return days;
        }
        static int CalculateDaysBetweenTwoMonth(MyDate beginDate, MyDate endDate)
        {
            int days = 0;
            //同月
            if (beginDate.Month == endDate.Month)
            {
                if (beginDate.Year == endDate.Year)//同年
                {
                    return days = endDate.Day - beginDate.Day;
                }
                else//相差一年
                {
                    if (IsLeapYear(beginDate.Year))//闰年
                    {
                        return days = 366 + (endDate.Day - beginDate.Day);
                    }
                    else//不是闰年
                    {
                        return days = 365 + (endDate.Day - beginDate.Day);
                    }
                }
            }
            //不同月
            else
            {
                if (beginDate.Year == endDate.Year)//同年
                {
                    for (int i = beginDate.Month; i < endDate.Month; i++)
                    {
                        days += months[i];
                    }
                    if (IsLeapYear(beginDate.Year) && beginDate.Month <= 2)
                    {
                        days += 1;
                    }
                    days += endDate.Day - beginDate.Day;
                }
                else//不同年
                {
                    for (int i = beginDate.Month; i <= 12; i++)
                    {
                        days += months[i];
                    }
                    days -= beginDate.Day;
                    for (int i = 1; i < endDate.Month; i++)
                    {
                        days += months[i];
                    }
                    days += endDate.Day;
                }
                return days;
            }
        }
        static int CalculateDaysBetweenTwoYear(int beginYear, int endYear)
        {
            int days = 0;
            //表达式判断过滤掉开头一年跟结尾的一年
            for (int i = beginYear + 1; i <= endYear - 1; i++)
            {
                if (IsLeapYear(i))
                {
                    days += 366;
                }
                else
                {
                    days += 365;
                }
            }
            return days;
        }
        static bool IsLeapYear(int year)
        {
            if (year % 400 == 0)//判断是否为世纪闰年
            {
                return true;
            }
            else if (year % 4 == 0 && year % 100 != 0)//判断是否为普通闰年
            {
                return true;
            }
            return false;//其他情况平年   
        }
    }
}
