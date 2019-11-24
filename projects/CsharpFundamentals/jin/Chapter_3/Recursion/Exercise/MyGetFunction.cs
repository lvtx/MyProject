using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Exercise
{
    class MyGetFunction
    {
        #region "打印某文件夹下的文件、子文件夹名字"
        public static string[] MyGetFiles(string path)
        {
            //Directory
            string[] a = { "a", "b" };
            return a;
        }
        #endregion

        #region "看一下官方所给函数的效果"
        public static void Test()
        {
            string path = @"D:\windows\files\package";
            string[] printFiles = Directory.GetFiles(path);
            Console.WriteLine("所有文件名");
            for (int i = 0; i < printFiles.Length; i++)
            {
                Console.WriteLine(printFiles[i]); 
            }
            string[] printDirectories = Directory.GetDirectories(path);
            Console.WriteLine("所有文件夹名");
            for (int i = 0; i < printDirectories.Length; i++)
            {
                Console.WriteLine(printDirectories[i]);
            }
        }
        #endregion
    }
}
