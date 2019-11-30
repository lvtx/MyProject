using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvolutionToLINQ
{
    class Program
    {
        static void Main(string[] args)
        {
            //FindOddNumbers();
            FindOddNumbers2();
            //FindOddOrEvenNumbers();
            //GenericFindOddOrEvenNumbers();
            //GenericFindOddOrEvenNumbers2();
            //FindOddOrEvenNumbersUseLambda();
            //FindOddOrEvenNumbersUseExtensionMethod();
            //FindOddOrEvenNumbersUseExtensionMethod2();
            //FindOddNumbersUseWhere();
            //FindOddNumbersUseLINQ();
            Console.ReadKey();
        }
        #region "原始版本"
        /// <summary>
        /// 查找所有的奇数
        /// </summary>
        static void FindOddNumbers()
        {
            //生成从1到10的整数集合
            var nums = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            //找出所有的奇数，放到List<int>集合中
            var result = new List<int>();
            foreach (var num in nums)
            {
                if (IsOdd(num))
                {
                    result.Add(num);
                }
            }
            //输出结果
            foreach (var num in result)
            {
                Console.WriteLine(num);
            }

        }
        /// <summary>
        /// 判断某数是否为奇数
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        static bool IsOdd(int num)
        {
            return num % 2 != 0;
        }

        #endregion

        #region "数据处理功能独立"
        /// <summary>
        /// 将数据处理功能外置为独立的方法
        /// </summary>
        static void FindOddNumbers2()
        {
            //生成从1到10的整数集合
            var nums = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            //找出所有满足条件的数，放到集合中
            var result = FilterIntegers(nums);
            //输出结果
            foreach (var num in result)
            {
                Console.WriteLine(num);
            }

        }
        static IEnumerable<int> FilterIntegers(IEnumerable<int> list)
        {
            //找出所有的奇数，放到List<int>集合中
            var result = new List<int>();
            foreach (var num in list)
            {
                if (IsOdd(num))
                {
                    result.Add(num);
                }
            }
            return result;
        }
        #endregion

        #region "引入委托"

        //定义一个新委托
        public delegate bool PredicateDelegate(int value);

        /// <summary>
        /// 让外界指定数据筛选的方法
        /// </summary>
        /// <param name="list"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        static IEnumerable<int> FilterIntegers(IEnumerable<int> list,
            PredicateDelegate predicate)
        {
            //找出所有的偶数，放到List<int>集合中
            var result = new List<int>();
            foreach (var num in list)
            {
                if (predicate(num))
                {
                    result.Add(num);
                }
            }
            return result;
        }
        /// <summary>
        /// 新增一个“偶数”判断方法
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        static bool IsEven(int num)
        {
            return num % 2 == 0;
        }

        static void FindOddOrEvenNumbers()
        {
            //生成从1到10的整数集合
            var nums = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            //现在在外部就可以“临时”指定到底是需要偶数还是奇数
            var result = FilterIntegers(nums, IsEven);
            //result = FilterIntegers(nums, IsOdd);
            //输出结果
            foreach (var num in result)
            {
                Console.WriteLine(num);
            }

        }
        #endregion

        #region "引入泛型"

        //定义一个新的泛型委托
        public delegate bool PredicateGenericDelegate<T>(T value);

        //修改数据过滤方法
        static IEnumerable<T> Filter<T>(IEnumerable<T> list,
            PredicateGenericDelegate<T> predicate)
        {
            //找出所有的偶数，放到List<int>集合中
            var result = new List<T>();
            foreach (var num in list)
            {
                if (predicate(num))
                {
                    result.Add(num);
                }
            }
            return result;
        }

        static void GenericFindOddOrEvenNumbers()
        {
            //生成从1到10的整数集合
            var nums = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            //现在在外部就可以“临时”指定到底是需要偶数还是奇数
            var result = Filter(nums, IsEven);
            //result = Filter(nums, IsOdd);
            //输出结果
            foreach (var num in result)
            {
                Console.WriteLine(num);
            }

        }
        #endregion

        #region "使用.NET预定义委托"
        static IEnumerable<T> Filter2<T>(IEnumerable<T> list,
           Func<T, bool> predicate)
        {
            //找出所有的偶数，放到List<int>集合中
            var result = new List<T>();
            foreach (var num in list)
            {
                if (predicate(num))
                {
                    result.Add(num);
                }
            }
            return result;
        }

        static void GenericFindOddOrEvenNumbers2()
        {
            //生成从1到10的整数集合
            var nums = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            //现在在外部就可以“临时”指定到底是需要偶数还是奇数
            var result = Filter2(nums, IsEven);
            result = Filter2(nums, IsOdd);
            //输出结果
            foreach (var num in result)
            {
                Console.WriteLine(num);
            }

        }
        #endregion

        #region "使用Lambda表达式"

        static void FindOddOrEvenNumbersUseLambda()
        {
            //生成从1到10的整数集合
            var nums = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            //现在在外部就可以“临时”指定到底是需要偶数还是奇数
            var result = Filter2(nums, num => num % 2 == 0);
            //result = Filter2(nums, num => num % 2 != 0);
            //输出结果
            foreach (var num in result)
            {
                Console.WriteLine(num);
            }

        }
        #endregion

        #region "引入扩展方法特性"
        static void FindOddOrEvenNumbersUseExtensionMethod()
        {
            //生成从1到10的整数集合
            var nums = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            var result = nums.Filter(num => num % 2 == 0);
            //result = nums.Filter(nums, num => num % 2 != 0);
            //输出结果
            foreach (var num in result)
            {
                Console.WriteLine(num);
            }

        }
        #endregion
        #region "优化后的扩展方法"
        static void FindOddOrEvenNumbersUseExtensionMethod2()
        {

            var nums = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            var result = nums.AdvancedFilter(num => num % 2 != 0)
                .AdvancedFilter(num => num % 3 != 0);


            foreach (var num in result)
            {
                Console.WriteLine(num);
            }

        }
        #endregion

        #region ".NET基类库中的Where扩展方法"

        static void FindOddNumbersUseWhere()
        {

            var nums = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            //Where方法可以级联，完成筛选出所有不是3的倍数的奇数的功能
            var result = nums.Where(num => num % 2 != 0)
                .Where(num => num % 3 != 0);


            foreach (var num in result)
            {
                Console.WriteLine(num);
            }

        }

        #endregion

        #region "使用LINQ让代码更易读"
        static void FindOddNumbersUseLINQ()
        {

            var nums = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            //LINQ查询就象数据库的SQL命令
            var result = from num in nums
                         where num % 2 != 0 && num % 3 != 0
                         select num;

            foreach (var num in result)
            {
                Console.WriteLine(num);
            }

        }
        #endregion
    }

    static class MyExtensions
    {
        public static IEnumerable<T> Filter<T>(this IEnumerable<T> list,
           Func<T, bool> predicate)
        {
            //找出所有的符合条件的数，放到List<int>集合中
            var result = new List<T>();
            foreach (var num in list)
            {
                if (predicate(num))
                {
                    result.Add(num);
                }
            }
            return result;
        }
        public static IEnumerable<T> AdvancedFilter<T>(this IEnumerable<T> list,
         Func<T, bool> predicate)
        {
            //找出所有的符合条件的数，返回给外界

            foreach (var num in list)
            {
                if (predicate(num))
                {
                    yield return num;
                }
            }

        }

        /// <summary>
        /// 自己实现的Where扩展方法，其功能与.NET基类库中所提供的类似
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static IEnumerable<T> Where<T>(this IEnumerable<T> list,
              Func<T, bool> predicate)
        {
            //找出所有的符合条件的数，返回给外界

            foreach (var num in list)
            {
                if (predicate(num))
                {
                    yield return num;
                }
            }

        }

    }
}
