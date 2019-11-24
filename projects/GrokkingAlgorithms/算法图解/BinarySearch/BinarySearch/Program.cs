using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace binary_search
{
    class Program
    {
        static void Main(string[] args)
        {
            int[] array = new int[5]{1, 3, 5, 7, 9};
            binary_search_ binary_Search = new binary_search_(array,16);
            Console.WriteLine(binary_Search.Search());
        }
    }

    class binary_search_
    {
        private int mid { get; set; }
        private int low { get; set; }
        private int high { get; set; }
        private int[] Array { get; set; }
        private int SearchOfNumber { get; set; }
        public binary_search_(int[] array ,int searchofnumber)
        {
            Array = array;
            SearchOfNumber = searchofnumber;
            low = 0;
            high = Array.Length - 1;
            mid = (high + low) / 2;
        }
        
        public bool Search()
        {
            while (low <= high)
            {
                mid = (high + low) / 2;
                if (SearchOfNumber < Array[mid])
                {
                    high = mid - 1;
                }
                if(SearchOfNumber > Array[mid])
                {
                    low = mid + 1;
                }
                else
                {
                    return true;
                }
            }
            return false;
        }
    }
}
