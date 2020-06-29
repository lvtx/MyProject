using System;
using System.Collections.Generic;
using System.Text;

namespace OverloadOperator
{
    class MyIntArray
    {
        public int[] arr;   //被操作的数组对象

        //重载加法运算符
        public static int[] operator +(MyIntArray x, MyIntArray y)
        {
            if ((x.arr == null) || (y.arr == null))
                throw new System.Exception("未指定要操作的数组");
            if (x.arr.Length != y.arr.Length)
                throw new System.Exception("数组大小不一致");

            //创建用于存放结果的数组对象
            int[] ret = new int[x.arr.Length];
            //数组对应元素相加
            for (int i = 0; i < x.arr.Length; i++)
                ret[i] = x.arr[i] + y.arr[i];

            return ret;
        }
    }
}
