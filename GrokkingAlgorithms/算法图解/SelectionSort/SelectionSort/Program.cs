using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelectionSort
{
    class Program
    {
        static void Main(string[] args)
        {
            int[] array = new int[] { 5, 3, 6, 2, 10 };
            Sort sort = new Sort(array);
            array = sort.SelectionSort();
            for (int i = 0; i < array.Length; i++)
            {
                Console.WriteLine(array[i]);
            }
        }
    }

    class Sort
    {
        private int[] Array { get; set; }
        public Sort(int[] array)
        {
            Array = array;
            //smallest = array[0];
        }

        public int[] SelectionSort()
        {
            int smallest = 0;
            int t = 0;
            for (int i = 0; i < Array.Length; i++)
            {
                smallest = Array[i];
                for (int j = i; j < Array.Length; j++)
                {
                    if (smallest > Array[j])
                    {
                        t = smallest;
                        smallest = Array[j];
                        Array[j] = t;
                    }
                }
                Array[i] = smallest;
            }
            return Array;
        }
    }
}
