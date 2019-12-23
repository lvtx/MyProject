using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace _2IntroToLINQ
{
    class Program
    {
        static void Main(string[] args)
        {
            //TestLogicalOperator();
            //ListMemberNamesOfEnumerable();
            //TestOrderBy();
            //TestAnonymousObject();
            //TestLetStatement();
            TestNativeMethod();
            Console.ReadLine();
        }

        #region "1.使用C#的逻辑运算符"
        static void TestLogicalOperator()
        {
            var files = from fileName in Directory.GetFiles
                        ("D:\\windows\\files\\document\\ebook\\教材\\大学\\软件\\操作系统")
                        where (File.GetLastWriteTime(fileName) < DateTime.Now.AddDays(-1))
                        && Path.GetExtension(fileName).ToUpper() == ".PDF"
                        select new FileInfo(fileName);
            foreach (var file in files)
            {
                Console.WriteLine(file.Name);
            }
            Console.ReadLine();
        }
        #endregion

        #region "2.Distinct示例：消除重复项"
        /// <summary>
        /// 列出Enumerable类的所有公有成员（除去重复的成员）
        /// </summary>
        static void ListMemberNamesOfEnumerable()
        {
            MemberInfo[] members = typeof(Enumerable).
                GetMembers(BindingFlags.Static | BindingFlags.Public);
            var methods = (from method in members
                           select method.Name).Distinct();
            int count = 0;
            foreach (var method in methods)
            {
                count++;
                Console.WriteLine("{0}:{1}", method, count);
            }
        }
        #endregion

        #region "3.按照文件名和大小排序"
        static void TestOrderBy()
        {
            var files = from file in Directory.GetFiles("D:\\windows\\files\\document\\ebook\\教材\\大学\\软件\\操作系统")
                        orderby (new FileInfo(file)).Name,
                        file descending
                        select (new FileInfo(file));
            foreach (var file in files)
            {
                Console.WriteLine("{0}:{1}字节", file.Name, file.Length);
            }
        }
        #endregion

        #region "4.动态创建匿名对象的示例"
        static void TestAnonymousObject()
        {
            var files =
                from file in Directory.GetFiles("D:\\windows\\files\\document\\ebook\\教材\\大学\\软件\\操作系统")
                select new
                {
                    FileName = new FileInfo(file).Name,
                    FileLength = new FileInfo(file).Length
                };
            foreach (var file in files)
            {
                Console.WriteLine("{0}:{1}字节", file.FileName, file.FileLength);
            }
        }
        #endregion

        #region "5.引入新的范围变量暂存查询结果"
        static void TestLetStatement()
        {
            var files =
                from file in
                Directory.GetFiles("D:\\windows\\files\\document\\ebook\\教材\\大学\\软件\\操作系统")
                let fileName = new FileInfo(file)
                orderby fileName.Length, fileName
                select fileName;
            foreach (var file in files)
            {
                Console.WriteLine("{0}:{1}字节", file.Name, file.Length);
            }
        }
        #endregion

        #region "6.使用本地方法"
        static void TestNativeMethod()
        {
            // 数据源.
            int[] numbers = { 5, 4, 1, 3, 9, 8, 6, 7, 2, 0 };
            var queryEvenNums =
                from num in numbers
                where IsEven(num)
                select num;
            foreach (var num in queryEvenNums)
            {
                Console.Write(num + ", ");
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
