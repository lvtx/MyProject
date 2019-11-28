using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;


namespace UseQueryMethods
{
    delegate int AddDelegate(int x, int y);
    class Program
    {
        static void Main(string[] args)
        {
            UseWhere();
        }
        private static void UseWhere()
        {
            IEnumerable<int> nums = Enumerable.Range(1, 100);
            var a = nums.Where<int>((num) => num % 5 == 0);
            var b = nums.Select<int, int>((num) =>  num % 5 == 0 ? num : 0);
            Print(a);
            Print(b);
            //AddDelegate del = delegate (int x, int y)
            //{
            //    return x + y;
            //};
            //del(5, 10);
            //AddDelegate del2 = (x, y) => x + y;
            //del2(5, 10);
            Console.ReadLine();
        }
        static void Print(IEnumerable<int> nums)
        {
            int counts = nums.Count();
            int Max = counts;
            foreach (var item in nums)
            {               
                if (counts % 10 == 0 && counts != Max)
                {
                    Console.WriteLine();
                }
                --counts;
                Console.Write(item + ", ");
            }
        }
    }
}
