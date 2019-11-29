using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace UseQueryMethods
{

    class Program
    {
        static void Main(string[] args)
        {
            //UseWhere();

            //UseSelect();

            //UseOrderBy();

            UseSetAndAggregateMethods();

            Console.ReadKey();

        }
        /// <summary>
        /// 使用Where扩展方法
        /// </summary>
        private static void UseWhere()
        {
            //创建一个包容[1,100]内整数的数组
            IEnumerable<int> nums = Enumerable.Range(1, 100);
            //使用Where扩展方法筛选出5的倍数，构成一个集合返回
            var numList = nums.Where((num) => num % 5 == 0);
            Print("集合中5的倍数有：{0}", numList);
        }

        /// <summary>
        /// Select方法示例
        /// </summary>
        private static void UseSelect()
        {
            //fileList的类型为：string[]
            var fileList = Directory.GetFiles("D:\\windows\\files\\package", "*.*");
            //files的类型为：IEnumerable<FileInfo> 
            var files = fileList.Select(
                file => new FileInfo(file));

            //infos是匿名对象的集合，此匿名对象包容两个属性：文件名和文件大小
            var infos = files.Select(info => new 
            {
                FileName = info.Name,
                FileLength = info.Length
            });

            //输出结果
            foreach (var info in infos)
            {
                Console.WriteLine("{0}:{1}字节", info.FileName,
                    info.FileLength);
            }
        }

        /// <summary>
        /// 数据排序示例
        /// </summary>
        private static void UseOrderBy()
        {
            //先把String对象转换为FileInfo对象，之后再按文件大小降序排列
            var fileInfos = Directory.GetFiles("C:\\")
                .Select(file => new FileInfo(file))
                .OrderByDescending(fileInfo => fileInfo.Length);

            //输出结果
            foreach (var info in fileInfos)
            {
                Console.WriteLine("{0}:{1}字节", info.Name,
                    info.Length);
            }
        }
       

        /// <summary>
        /// 集合与聚合操作
        /// </summary>
        private static void UseSetAndAggregateMethods()
        {
            //创建一个多态对象集合
            IEnumerable<object> stuff = new object[]
            { new object(),
                1, 3, 5, 7, 9,
                "\"thing\"",
                Guid.NewGuid() };
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

            Print("整数集合与偶数集合的差集：{0}", numbers.Except(even));

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
        }

        #region "辅助方法"
        /// <summary>
        /// 输出集合的内容
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="format"></param>
        /// <param name="items"></param>
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
        /// <summary>
        /// 按指定格式输出参数值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="format"></param>
        /// <param name="item"></param>
        private static void Print<T>(string format, T item)
        {
            Console.WriteLine(format, item);
        }

        #endregion

    }
}
