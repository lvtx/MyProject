using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLib
{
    public class MyClass
    {
        //求一个数的倍数
        public int DoubleValue(int i)
        {
            return i * 2;
        }
        //计算从[from,to]中所有整数的和
        public int Sum(int from, int to)
        {
            if (from > to)
            {
                throw new ArgumentException("参数from必须小于to");
            }
            int sum = 0;
            for (int i = from; i <= to; i++)
            {
                sum += i;
            }
            return sum;
        }

        public const String AgeErrorString = "生日必须小于当前日期";
        //给定一个人的生日，计算他的年纪
        public int CaculateAge(DateTime Birthday)
        {
            if (DateTime.Now < Birthday)
            {
                throw new ArgumentOutOfRangeException(AgeErrorString);
            }
            return DateTime.Now.Year - Birthday.Year;
        }
      
    }
}
