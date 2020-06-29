using System;
using System.Collections.Generic;
using System.Text;

namespace UseMathOpt
{
    class MathOpt
    {
        //整数相加
        public int Add(int x, int y)
        {
            return x + y;
        }
        //浮点数相加
        public double Add(double x, double y)
        {
            return x + y;
        }



        //获取数组中的最大值
        public int GetMaxValue(int[] values)
        {
            int maxvalue = values[0];
            for (int i = 0; i < values.GetLength(0); i++)
            {
                if (maxvalue < values[i])
                    maxvalue = values[i];
            }
            return maxvalue;
        }
    }
}
