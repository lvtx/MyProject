using System;
using System.Collections.Generic;
using System.Text;

namespace UseMathOpt
{
    class MathOpt
    {
        //�������
        public int Add(int x, int y)
        {
            return x + y;
        }
        //���������
        public double Add(double x, double y)
        {
            return x + y;
        }



        //��ȡ�����е����ֵ
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
