using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LINQExamples
{
    class Program
    {
        static void Main(string[] args)
        {
            //TestLogicalOperator();
            //TestDistinct();
            TestOrderBy();
            //TestAnonymousObject();
            //TestLetStatement();
            //TestNativeMethod();

            Console.ReadKey();
        }

        #region "使用C#的逻辑运算符"

        static void TestLogicalOperator()
        {
            IEnumerable<FileInfo> files =
                from fileName in Directory.GetFiles("D:\\windows\\files\\document\\ebook\\教材\\大学\\软件\\操作系统")
                where File.GetLastWriteTime(fileName) < DateTime.Now.AddDays(-1)
                  && Path.GetExtension(fileName).ToUpper() == ".PDF"
                select new FileInfo(fileName);

            foreach (var file in files)
            {
                Console.WriteLine(file);
            }
        }
        #endregion

        #region "Distinct示例：消除重复项"

        static void TestDistinct()
        {
            Console.WriteLine("Enumerable类的所有公有成员：");
            ListMemberNamesOfEnumerable();
        }
        /// <summary>
        /// 列出Enumerable类的所有公有成员（除去重复的成员）
        /// </summary>
        static void ListMemberNamesOfEnumerable()
        {

            MemberInfo[] members = typeof(Enumerable).GetMembers(
                BindingFlags.Static | BindingFlags.Public);

            //var methods = (from method in members
            //               select method.Name);
            var methods = (from method in members
                           select method.Name).Distinct();
            int count = 0;

            foreach (string method in methods)
            {
                count++;
                Console.WriteLine(" {0}: {1},", count, method);
            }
        }
        #endregion

        #region "排序"
        static void TestOrderBy()
        {
            IEnumerable<FileInfo> files =
                  from fileName in Directory.GetFiles("D:\\windows\\files\\document\\ebook\\教材\\大学\\软件\\操作系统")
                  orderby (new FileInfo(fileName)).Length,
                    fileName ascending
                  select new FileInfo(fileName);

            foreach (var file in files)
            {
                Console.WriteLine("{0}  {1}字节", file.Name, file.Length);
            }

        }
        #endregion

        #region "动态创建匿名对象的示例"
        static void TestAnonymousObject()
        {
            var files = from fileName in Directory.GetFiles("C:\\")
                        select new
                        {
                            Name = fileName,
                            Length = new FileInfo(fileName).Length
                        };

            foreach (var file in files)
            {
                Console.WriteLine("{0}:{1}字节", file.Name, file.Length);
            }

        }
        #endregion


        #region "引入新的范围变量暂存查询结果"

        static void TestLetStatement()
        {
            IEnumerable<FileInfo> files =
                from fileName in Directory.GetFiles("C:\\")
                let file = new FileInfo(fileName)
                orderby file.Length, fileName
                select file;

            foreach (var file in files)
            {
                Console.WriteLine("{0}:{1}字节", file.Name, file.Length);
            }
        }

        #endregion


        #region"使用本地方法"

        static void TestNativeMethod()
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
        #endregion
    }
}
