using System;
using System.Collections.Generic;
using System.Text;

namespace MaxMinValueForGP2
{
    class ComplexNum:IComparable<ComplexNum>
    {
        public double a;//实部
        public double b ;//虚部

        public ComplexNum(double  aValue , double  bValue )
        {
            a = aValue;
            b = bValue;
        }
        //按复数标准形式返回a+bi形式的字串
        public override String ToString()
        {
            return (a.ToString()  + "+" + b.ToString() + "i");
        }
        //求复数的模
        public double  GetMod() 
        {
            return Math.Sqrt(a * a + b * b);
        }
        //按模比较两个复数的大小
        int IComparable<ComplexNum>.CompareTo(ComplexNum other  ) 
        {
            if (Math.Abs(GetMod() - other.GetMod()) < 0.000001 )
                return 0;
            if( GetMod() > other.GetMod() )
                return 1;
            else
                return -1;
        }   
    }
}
