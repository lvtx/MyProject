using System;
using System.Collections.Generic;
using System.Text;

namespace DynamicInvokeMethodLibrary
{
    public class SumNumber
    {
        //两个重载的实例方法
        public int Sum(int i, int j)
        {
            return i + j;
        }
        public int Sum(int i, int j, int k)
        {
            return i + j + k;
        }
        //静态方法
        public static int Sum(int[] arr)
        {
            int ret=0;
            for (int i = 0; i < arr.Length; i++)
            {
                ret += arr[i];
            }
            return ret;
        }
    }
}
