using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FuncAsFuncArgu
{
    class Program
    {
        static void Main(string[] args)
        {
            int[] numbers = new int[7] { 0, 1, 2, 3, 4, 5, 6 };
            Console.WriteLine(Process(Add, numbers, 0, 5));
            Console.WriteLine(Process(Multiply, numbers, 1, 5));
            Console.ReadKey();
        }

        static int Process(Func<int, int, int> op, int[] numbers, int from, int to)
        {
            int result = numbers[from];
            for (int i = from + 1; i <= to; i++)
                result = op(result, numbers[i]);
            return result;
        }

        static int Add(int i, int j)
        {
            return i + j;
        }

        static int Multiply(int i, int j)
        {
            return i * j;
        }
    }
}
