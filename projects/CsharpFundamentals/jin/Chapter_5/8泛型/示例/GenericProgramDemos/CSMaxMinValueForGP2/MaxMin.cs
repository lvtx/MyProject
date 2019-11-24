using System;
using System.Collections.Generic;
using System.Text;

namespace MaxMinValueForGP2
{
    /// <summary>
    /// ʹ�÷������װ�㷨
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MaxMin<T> where T : IComparable<T>
    {

        //�������ݣ���ȡ���ֵ��Сֵ
        public static Pair<T> GetMaxMinVauleFromArray(T[] arr)
        {

            Pair<T> ret;
            ret.Max = arr[0];
            ret.Min = arr[0];
            for (int i = 1; i <= arr.GetUpperBound(0); i++)
            {
                if (ret.Max.CompareTo(arr[i]) < 0)
                    ret.Max = arr[i];

                if (ret.Min.CompareTo(arr[i]) > 0)
                    ret.Min = arr[i];
            }

            return ret;
        }

    }
    /// <summary>
    /// ʹ�÷���struct��װ���ݴ�����
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public struct Pair<T>
    {
        public T Max;
        public T Min;
    }
}
