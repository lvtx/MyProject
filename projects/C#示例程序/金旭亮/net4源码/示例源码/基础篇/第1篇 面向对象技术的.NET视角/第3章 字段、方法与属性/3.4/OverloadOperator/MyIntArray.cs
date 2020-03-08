using System;
using System.Collections.Generic;
using System.Text;

namespace OverloadOperator
{
    class MyIntArray
    {
        public int[] arr;   //���������������

        //���ؼӷ������
        public static int[] operator +(MyIntArray x, MyIntArray y)
        {
            if ((x.arr == null) || (y.arr == null))
                throw new System.Exception("δָ��Ҫ����������");
            if (x.arr.Length != y.arr.Length)
                throw new System.Exception("�����С��һ��");

            //�������ڴ�Ž�����������
            int[] ret = new int[x.arr.Length];
            //�����ӦԪ�����
            for (int i = 0; i < x.arr.Length; i++)
                ret[i] = x.arr[i] + y.arr[i];

            return ret;
        }
    }
}
