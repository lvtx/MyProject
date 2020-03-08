using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SortWithKeys
{
    class Program
    {
        static void Main(string[] args)
        {

            int[] Keys = new int[]
            {
                7,3,6,5,4,1,2
            };
            string[] Items = new string[]
            {
               "Sunday","Wednesday","Saturday","Friday","Thursday","Monday","Tuesday"
            };
            Array.Sort<int, string>(Keys, Items);
            Array.ForEach<string>(Items, (str) => { Console.WriteLine(str); });
            Console.ReadKey();
        }
    }
}
