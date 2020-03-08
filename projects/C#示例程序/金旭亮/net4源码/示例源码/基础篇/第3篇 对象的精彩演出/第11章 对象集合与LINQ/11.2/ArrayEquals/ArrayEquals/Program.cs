using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace ArrayEquals
{
    class Program
    {
        static void Main(string[] args)
        {
            int[] arr1 = new int[]
            {
                1,2,3,4
            };
            int[] arr2 = new int[]
            {
                1,2,3,4
            };
            Console.WriteLine((arr1 as IStructuralEquatable).Equals(arr2, StructuralComparisons.StructuralEqualityComparer));
            Console.WriteLine((arr1 as IStructuralEquatable).Equals(arr2, EqualityComparer<int>.Default));
            
            Console.WriteLine((arr1 as IStructuralComparable).CompareTo(arr2, StructuralComparisons.StructuralComparer));
            Console.WriteLine((arr1 as IStructuralComparable).CompareTo(arr2,Comparer.Default));
          
           
            Console.ReadKey();
        }
    }
}
