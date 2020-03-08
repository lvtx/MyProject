using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UseNativeMethod
{
    class Program
    {
        static void Main(string[] args)
        {
            // 数据源.
            int[] numbers = { 5, 4, 1, 3, 9, 8, 6, 7, 2, 0 };

          
            //查找所有偶数
            var queryEvenNums =
                from num in numbers
                where IsEven(num)
                select num;

            // 执行查询
            foreach (var s in queryEvenNums)
            {
                Console.Write(s.ToString() + " ");
            }
            Console.ReadKey();

        }
        /// <summary>
        /// 判断一个数是否为偶数
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        private static bool IsEven(int num)
        {
            if (num % 2 == 0)
                return true;
            return false;
        }

    }
}
