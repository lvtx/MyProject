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
            UseLinq();

            //UseExtensionMethods();

            Console.ReadKey();
        }

        private static void UseLinq()
        {
            //  1. 指定数据源
            var numbers = new int[] { 0, 1, 2, 3, 4, 5, 6 };

            // 2. 创建查询表达式
            var numQuery =
                from num in numbers
                where (num % 2) == 0
                select num;

            // 3. 执行查询
            foreach (int num in numQuery)
            {
                Console.Write("{0} ", num);
            }
        }

        static void UseExtensionMethods()
        {
            IEnumerable<int> numQuery =
                new int[] { 0, 1, 2, 3, 4, 5, 6 }
                    .Where<int>(
                        delegate(int num)
                        {
                            return (num % 2) == 0;
                        }
                    );

            foreach (int num in numQuery)
            {
                Console.Write("{0} ", num);
            }

        }
    }
}
