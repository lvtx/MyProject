using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace CascadingStatement
{
    class Program
    {
        static void Main(string[] args)
        {
            int[] sourceArray = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            int[] destinationArray = sourceArray.Slice(2, 5).Reverse();
            int[] destinationArray2 = MyExtensions.Reverse(MyExtensions.Slice(sourceArray, 2, 5));
            foreach (int num in destinationArray)
                Console.Write("{0} ", num);
            Console.ReadKey();
        }
    }

    public static class MyExtensions
    {
        //将一个数组中的部分抽取为一个新的数组
        public static T[] Slice<T>(this T[] source, int index, int count)
        {
            T[] result = new T[count];
            Array.Copy(source, index, result, 0, count);
            return result;
        }
        //反转一个数组元素的排列顺序
        public static T[] Reverse<T>(this T[] source)
        {
            Array.Reverse(source);
            return source;
        }
    }
}
