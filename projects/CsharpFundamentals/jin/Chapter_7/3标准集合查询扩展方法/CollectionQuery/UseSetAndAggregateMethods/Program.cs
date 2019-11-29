using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseSetAndAggregateMethods
{
    class Program
    {
        static void Main(string[] args)
        {
            Random ran = new Random();
            int[] array = new int[10];
            for (int i = 0; i < 10; i++)
            {
                array[i] = ran.Next(1, 100);
                Console.Write(array[i]+", ");
            }
            Console.WriteLine();
            Console.WriteLine("总和{0}",array.Sum());
            Console.WriteLine("平均值{0}",array.Average());
            Console.WriteLine("最大值{0}",array.Max());
            Console.WriteLine("最小值{0}",array.Min());
            Console.ReadLine();
        }
    }
}
