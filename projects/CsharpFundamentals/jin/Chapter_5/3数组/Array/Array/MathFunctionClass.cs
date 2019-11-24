using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Array
{
    class MathFunctionClass
    {
        internal static double Avg(int[] intArray)
        {
            int sum = 0;
            double ret = 0;
            foreach (var item in intArray)
            {
                sum += item;
            }
            ret = sum / intArray.Length;
            return ret;
        }
        internal static int Max(int[] intArray)
        {
            int max = intArray[0];
            foreach (var item in intArray)
            {
                if(max < item)
                {
                    max = item;
                }
            }
            return max;
        }
        internal static int Min(int[] intArray)
        {
            int min = intArray[0];
            foreach (var item in intArray)
            {
                if (min > item)
                {
                    min = item;
                }
            }
            return min;
        }
        internal static double Median(int[] intArray)
        {
            int mid = intArray.Length / 2 - 1;
            double med;//中位数
            int[] tempArray = Sequence(intArray);
            med = (tempArray[mid] + tempArray[mid + 1]) / 2;
            return med;
        }
        private static int[] Sequence(int[] intArray)
        {
            int[] tempArray = new int[intArray.Length];
            //将intArray的值赋给tempArray
            for (int i = 0; i < intArray.Length; i++)
            {
                tempArray[i] = intArray[i];
            }
            //排序
            for (int i = 0; i < tempArray.Length; i++)
            {
                int count = 0;
                int temp = tempArray[i];
                for (int j = i; j < tempArray.Length; j++)
                {
                    if (temp >= tempArray[j])
                    {
                        temp = tempArray[j];
                        count = j;
                    }
                }
                tempArray[count] = tempArray[i];
                tempArray[i] = temp;
            }
            Console.Write("排序之后:");
            for (int i = 0; i < tempArray.Length; i++)
            {
                Console.Write("{0} ,",tempArray[i]);
            }
            return tempArray;
        }
    }
}
