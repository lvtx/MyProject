using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BubbleSort
{
    class Program
    {
        static void Main(string[] args)
        {
            int[] array = new int[10] { 1, 3, 5, 7, 9, 6, 2, 4, 8, 10 };
            int[] array2 = new int[10];
            array2 = Bubblesort(array);
            for (int i = 0; i < array.Length; i++)
            {
                Console.Write("{0}, ",array2[i]);
            }
            Console.ReadKey();
        }
        private static int[] Bubblesort(int[] array)
        {
            int temp = 0;
            bool sorted = false;//排序是否完成的标志
            while (!sorted)
            {
                sorted = true;
                for (int i = 1; i < array.Length; i++)
                {
                    if (array[i - 1] > array[i])
                    {
                        temp = array[i - 1];
                        array[i - 1] = array[i];
                        array[i] = temp;
                        sorted = false;
                    }
                }
            }
            return array;
        }
    }
}
