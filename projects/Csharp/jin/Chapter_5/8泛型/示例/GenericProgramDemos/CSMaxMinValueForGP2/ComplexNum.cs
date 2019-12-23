using System;
using System.Collections.Generic;
using System.Text;

namespace MaxMinValueForGP2
{
    class ComplexNum:IComparable<ComplexNum>
    {
        public double a;//ʵ��
        public double b ;//�鲿

        public ComplexNum(double  aValue , double  bValue )
        {
            a = aValue;
            b = bValue;
        }
        //��������׼��ʽ����a+bi��ʽ���ִ�
        public override String ToString()
        {
            return (a.ToString()  + "+" + b.ToString() + "i");
        }
        //������ģ
        public double  GetMod() 
        {
            return Math.Sqrt(a * a + b * b);
        }
        //��ģ�Ƚ����������Ĵ�С
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
