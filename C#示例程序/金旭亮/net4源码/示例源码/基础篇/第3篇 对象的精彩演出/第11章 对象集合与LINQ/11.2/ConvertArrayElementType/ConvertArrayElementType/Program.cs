using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConvertArrayElementType
{
    class Program
    {
        //将int数值转换为double数值
        static Converter<int, double> IntToDouble = delegate(int value)
        {
            return (double)value;
        };

        static void Main(string[] args)
        {
            int[] intArr = new int[]
            {
                0,1,2,3,5,6,7,8,9
            };
            double[] doubleArr = Array.ConvertAll<int, double>(intArr, IntToDouble);
            foreach (double value in doubleArr)
                Console.WriteLine(value);

            Console.ReadKey();
        }
    }
}
