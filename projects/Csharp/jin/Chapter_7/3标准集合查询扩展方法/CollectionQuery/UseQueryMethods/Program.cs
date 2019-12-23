using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;


namespace UseQueryMethods
{
    delegate int AddDelegate(int x, int y);
    class Program
    {
        static void Main(string[] args)
        {
            //UseWhere();
            //UseSelect();
            //UseOrderBy();
            UseSetAndAggregateMethods();
            Console.ReadLine();
        }
        private static void UseWhere()
        {
            IEnumerable<int> nums = Enumerable.Range(1, 100);
            var a = nums.Where<int>((num) => num % 5 == 0);
            //Print(a);
            //AddDelegate del = delegate (int x, int y)
            //{
            //    return x + y;
            //};
            //del(5, 10);
            //AddDelegate del2 = (x, y) => x + y;
            //del2(5, 10);
        }
        private static void UseSelect()
        {
            var fileList = Directory.GetFiles("D:\\windows\\files\\package");
            var files = fileList.Select(file => new FileInfo(file));
            var infos = files.Select(info => new
            {
                FileName = info.Name,
                FileLength = info.Length
            });
            foreach (var info in infos)
            {
                Console.WriteLine(info.FileName + "的大小" + info.FileLength);
            }
        }
        private static void UseOrderBy()
        {
            var infos = Directory.GetFiles("C:\\")
                .Select((file) => new FileInfo(file))
                .OrderByDescending((fileinfo) => fileinfo.Length);
            foreach (var info in infos)
            {
                Console.WriteLine("{0}:{1}字节",info.Name,info.Length);
            }
        }
        private static void UseSetAndAggregateMethods()
        {
            //创建一个多态对象集合
            IEnumerable<object> stuff = new object[]
            {
                new object(),
                1,2,3,4,5,
                "this is a very string",
                Guid.NewGuid()
            };
            Print("stuff的内容:{0}",stuff);
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
        private static void Print<T>(string format,IEnumerable<T> items)
        {
            StringBuilder text = new StringBuilder();
            foreach (var item in items)
            {
                text.Append(item + ", ");
            }
            Console.WriteLine(format,text);
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
