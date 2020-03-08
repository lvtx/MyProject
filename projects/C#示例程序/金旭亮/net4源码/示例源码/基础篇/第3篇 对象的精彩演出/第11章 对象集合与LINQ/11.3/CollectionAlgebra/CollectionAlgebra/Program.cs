using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CollectionAlgebra
{
    class Program
    {
        static void Main(string[] args)
        {
            //创建一个多态对象集合
            IEnumerable<object> stuff = new object[] { new object(), 1, 3, 5, 7, 9, "\"thing\"", Guid.NewGuid() };
            //输出数组的内容
            Print("Stuff集合的内容: {0}", stuff);

            //偶数集合
            IEnumerable<int> even = new int[] { 0, 2, 4, 6, 8 };
            Print("偶数集合的内容: {0}", even);

            //从多态集合stuff中筛选中整数元素（全部为奇数）组成一个新集合
            IEnumerable<int> odd = stuff.OfType<int>();

            Print("奇数集合的内容: {0}", odd);

            //求两个集合的并集
            IEnumerable<int> numbers = even.Union(odd);
            Print("奇数与偶数集合的并集，成为一个整数集合: {0}", numbers);

            Print("整数集合与偶数集合的并集: {0}", numbers.Union(even));

            Print("整数集合与奇数集合相连接: {0}", numbers.Concat(odd));

            Print("整数集合与偶数集合的交集: {0}", numbers.Intersect(even));

            Print("整数集合与奇数集合连接，再删除重复值: {0}", numbers.Concat(odd).Distinct());


            if (!numbers.SequenceEqual(numbers.Concat(odd).Distinct()))
            {
                throw new Exception("Unexpectedly unequal");
            }
            else
            {

                Print("反转整数集合: {0}", numbers.Reverse());

                Print("求整数集合的平均值: {0}", numbers.Average());
                Print("求整数集合的总和: {0}", numbers.Sum());
                Print("求整数集合的最大值: {0}", numbers.Max());
                Print("求整数集合的最小值: {0}", numbers.Min());
            }

            Console.ReadKey();
        }

        private static void Print<T>(string format, IEnumerable<T> items)
        {
            StringBuilder text = new StringBuilder();

            foreach (T item in items.Take(items.Count() - 1))
            {
                text.Append(item + ", ");
            }


            text.Append(items.Last());
            Console.WriteLine(format, text);
        }

        private static void Print<T>(string format, T item)
        {
            Console.WriteLine(format, item);
        }
    }
}
