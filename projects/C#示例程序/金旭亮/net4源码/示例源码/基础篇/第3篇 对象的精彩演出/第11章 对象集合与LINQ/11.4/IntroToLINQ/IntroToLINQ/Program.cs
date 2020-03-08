using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntroToLINQ
{
    class Program
    {
        static void Main(string[] args)
        {
            //  1. 指定数据源
            int[] numbers = new int[] { 0, 1, 2, 3, 4, 5, 6 };

            // 2. 创建查询表达式
            var numQuery =
                from num in numbers
                where (num % 2) == 0
                select num;
           
            // 3. 执行查询
            foreach (int num in numQuery)
                 Console.Write("{0,1} ", num);
       
            Console.ReadKey();
        }
    }
}
