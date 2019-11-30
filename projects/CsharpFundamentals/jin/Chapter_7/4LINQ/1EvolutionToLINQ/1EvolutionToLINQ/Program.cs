using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1EvolutionToLINQ
{
    //为扩展方法创建的类
    static class MyExtensions
    {
        public static IEnumerable<T> Filter<T>(this IEnumerable<T> items, Func<T, bool> predicate)
        {
            var ret = new List<T>();
            foreach (var item in items)
            {
                if (predicate(item))
                {
                    ret.Add(item);
                }
            }
            return ret;
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            //FindOddNumbers();
            //FindOddNumbers2();
            //FindOddOrEvenNumbers();
            //GenericFindOddOrEvenNumbers();
            //GenericFindOddOrEvenNumbers2();
            //FindOddOrEvenNumbersUseLambda();
            FindOddOrEvenNumbersUseExtensionMethod();
            Console.ReadLine();
        }

        //判断是否为基数
        static bool IsOdd(int num)
        {
            if(num % 2 == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        //判断是否为偶数
        static bool IsEven(int num)
        {
            if (num % 2 != 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }



        #region "1.原始版本"
        static void FindOddNumbers()
        {
            int[] nums = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            List<int> ret = new List<int>();
            foreach (var num in nums)
            {
                if (num % 2 != 0)
                {
                    ret.Add(num);
                }
            }
            foreach (var item in ret)
            {
                Console.Write(item + ", ");
            }
        }
        #endregion

        #region "2.将数据过滤功能提取出来"
        static void FindOddNumbers2()
        {
            int[] nums = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            IEnumerable<int> ret = FilterIntegers(nums);
            foreach (var item in ret)
            {
                Console.Write(item + ", ");
            }
        }
        static IEnumerable<int> FilterIntegers(IEnumerable<int> items)
        {
            var ret = new List<int>();
            foreach (var item in items)
            {
                if (IsOdd(item))
                {
                    ret.Add(item);
                }
            }
            return ret;
        }
        #endregion

        #region "3.引入委托"
        public delegate bool PredicateDelegate(int num);
        static void FindOddOrEvenNumbers()
        {
            int[] nums = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            IEnumerable<int> ret = FilterIntegers(nums,IsEven);
            foreach (var item in ret)
            {
                Console.Write(item + ", ");
            }
        }
        static IEnumerable<int> FilterIntegers(IEnumerable<int> items
            ,PredicateDelegate predicate)
        {
            var ret = new List<int>();
            foreach (var item in items)
            {
                if (predicate(item))
                {
                    ret.Add(item);
                }
            }
            return ret;
        }
        #endregion

        #region "4.引入泛型"
        public delegate bool PredicateGenericDelegate<T>(T num);
        static void GenericFindOddOrEvenNumbers()
        {
            int[] nums = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            var ret = Filter(nums, IsEven);
            foreach (var item in ret)
            {
                Console.Write(item + ", ");
            }
        }
        static IEnumerable<T> Filter<T>(IEnumerable<T> items
            , PredicateGenericDelegate<T> predicate)
        {
            var ret = new List<T>();
            foreach (var item in items)
            {
                if (predicate(item))
                {
                    ret.Add(item);
                }
            }
            return ret;
        }
        #endregion

        #region "5.dotNet预定义委托"
        static void GenericFindOddOrEvenNumbers2()
        {
            int[] nums = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            var ret = Filter2(nums, IsEven);
            foreach (var item in ret)
            {
                Console.Write(item + ", ");
            }
        }
        static IEnumerable<T> Filter2<T>(IEnumerable<T> items
            , Func<T,bool> predicate)
        {
            var ret = new List<T>();
            foreach (var item in items)
            {
                if (predicate(item))
                {
                    ret.Add(item);
                }
            }
            return ret;
        }
        #endregion

        #region "6.Lambda表达式"
        static void FindOddOrEvenNumbersUseLambda()
        {
            int[] nums = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            var ret = Filter(nums, num => num % 2 != 0);
            foreach (var item in ret)
            {
                Console.Write(item + ", ");
            }
        }
        #endregion

        #region "7.使用扩展方法"
        static void FindOddOrEvenNumbersUseExtensionMethod()
        {
            int[] nums = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            var ret = nums.Filter(num => num % 2 != 0);
            foreach (var item in ret)
            {
                Console.Write(item + ", ");
            }
        }
        #endregion
    }
}
