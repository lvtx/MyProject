using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculateDaysForOO
{
    class CalculateDate
    {
        private int[] months = { 0, 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };
        public int CalculateDaysBetweenTwoDate(MyDate beginDate, MyDate endDate)
        {
            int days = 0;
            days = CalculateDaysBetweenTwoYear(beginDate.Year, endDate.Year);
            days += CalculateDaysBetweenTwoMonth(beginDate, endDate);
            return days;
        }
        private int CalculateDaysBetweenTwoMonth(MyDate beginDate, MyDate endDate)
        {
            int days = 0;
            bool a = IsLeapYear(beginDate.Year);
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
        private int CalculateDaysBetweenTwoYear(int beginYear, int endYear)
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
        private bool IsLeapYear(int year)
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
