using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArrayExistsEvenNumer
{
    class Program
    {
        static void Main(string[] args)
        {
            Predicate<int> ExistEvenNumber=delegate(int value)
            {
                return value%2==0?true:false;
            };

            int[] IntArr = new int[]
            {
                0,1,2,3,4,5,6,7,8,9
            };

            bool HasEvenNumbers = Array.Exists<int>(IntArr, ExistEvenNumber);

            Console.WriteLine(HasEvenNumbers);
            Console.ReadKey();
        }
    }
}
