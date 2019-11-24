using System;
using System.Collections.Generic;
using System.Text;

namespace MaxMinValueForGP2
{
    /// <summary>
    /// 使用泛型类封装算法
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MaxMin<T> where T : IComparable<T>
    {

        //处理数据，获取最大值最小值
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
    /// 使用泛型struct封装数据处理结果
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public struct Pair<T>
    {
        public T Max;
        public T Min;
    }
}
