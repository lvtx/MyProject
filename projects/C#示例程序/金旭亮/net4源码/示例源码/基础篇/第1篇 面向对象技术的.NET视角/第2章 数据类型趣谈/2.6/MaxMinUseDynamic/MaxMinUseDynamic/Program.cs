using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MaxMinUseDynamic
{
    class Program
    {
        static void GetMaxMinVauleFromArray<T>(T[] datas, ref T Max, ref T Min)
        {
            dynamic max, min;
            max = datas[0];
            min = datas[0];
            for (int i = 1; i <= datas.GetUpperBound(0); i++)
            {
                if (max < datas[i]) max = datas[i];
                if (min > datas[i]) min = datas[i];
            }
            Max = max;
            Min = min;
        }
        static void Main(string[] args)
        {
            //以int数组为例，请读者自行试验其他类型的数组
            int[] arr = new int[10];
            Random ran = new Random();
            for (int i = 0; i < 10; i++)
            {
                arr[i] = ran.Next(1, 100);
                Console.Write("{0},", arr[i]);
            }
            Console.WriteLine();
            int Max = 0, Min = 0;
            GetMaxMinVauleFromArray<int>(arr, ref Max, ref Min);
            Console.WriteLine("Max:{0} Min:{1}", Max, Min);
            Console.ReadKey();

        }
    }
}
