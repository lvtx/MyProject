using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UseDistinct
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enumerable类的所有公有成员：");
            ListMemberNamesOfEnumerable();
            Console.ReadKey();
        }
        /// <summary>
        /// 列出Enumerable类的所有公有成员（除去重复的成员）
        /// </summary>
        public static void ListMemberNamesOfEnumerable()
        {
            IEnumerable<string> enumerableMethodNames = (
            from method in typeof(Enumerable).GetMembers(
            System.Reflection.BindingFlags.Static |
            System.Reflection.BindingFlags.Public)
            select method.Name).Distinct();
            int count = 0;
            foreach (string method in enumerableMethodNames)
            {
                count++;
                Console.WriteLine(" {0}: {1},", count , method);
            }
        }
    }
}
