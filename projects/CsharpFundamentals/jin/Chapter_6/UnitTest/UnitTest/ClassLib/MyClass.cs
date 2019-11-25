using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLib
{
    public class MyClass
    {
        public int DoubleValue(int value)
        {
            return 2 * value;
        }

        public int Sum(int start,int end)
        {
            int value = 0;
            if(start > end)
            {
                throw new ArgumentException("起始值不能大于终值");
            }
            else
            {
                for (int i = start; i <= end; i++)
                {
                    value += i;
                }
            }
            return value;
        }
        /// <summary>
        /// 计算岁数
        /// </summary>
        /// <param name="birthday"></param>
        /// <returns></returns>
        public const string AgeErrorMessage = "生日必须小于当前日期"; 
        public int CalculateAge(DateTime birthday)
        {
            if (DateTime.Now < birthday)
            {
                throw new ArgumentOutOfRangeException(AgeErrorMessage);
            }
            else
            {
                return DateTime.Now.Year - birthday.Year;
            }
        }
    }
}
