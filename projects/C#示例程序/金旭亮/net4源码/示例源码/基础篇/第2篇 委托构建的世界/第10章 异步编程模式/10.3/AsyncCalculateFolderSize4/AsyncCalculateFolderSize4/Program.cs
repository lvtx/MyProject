using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace AsyncCalculateFolderSize4
{
    class Program
    {
        //计算指定文件夹的总容量
        private static long CalculateFolderSize(string FolderName)
        {
            if (Directory.Exists(FolderName) == false)
            {
                throw new DirectoryNotFoundException("文件夹不存在");
            }

            DirectoryInfo RootDir = new DirectoryInfo(FolderName);
            //获取所有的子文件夹
            DirectoryInfo[] ChildDirs = RootDir.GetDirectories();
            //获取当前文件夹中的所有文件
            FileInfo[] files = RootDir.GetFiles();
            long totalSize = 0;
            //累加每个文件的大小
            foreach (FileInfo file in files)
            {
                totalSize += file.Length;
            }
            //对每个文件夹执行同样的计算过程：累加其下每个文件的大小
            //这是通过递归调用实现的
            foreach (DirectoryInfo dir in ChildDirs)
            {
                totalSize += CalculateFolderSize(dir.FullName);
            }
            //返回文件夹的总容量
            return totalSize;
        }

       
        public delegate long CalculateFolderSizeDelegate(string FolderName);

        private static CalculateFolderSizeDelegate d=CalculateFolderSize;
        //用于回调的函数
        public static void ShowFolderSize(IAsyncResult result)
        {
            long size = d.EndInvoke(result);
            Console.WriteLine("\n文件夹{0}的容量为:{1}字节\n", (String)result.AsyncState, size);
        }

        static void Main(string[] args)
        {
            string FolderName;

            while (true)
            {
                Console.WriteLine("请输入文件夹名称(例如:C:\\Windows),输入quit结束程序");
                FolderName = Console.ReadLine();
                if (FolderName == "quit")
                    break;
                d.BeginInvoke(FolderName, ShowFolderSize, FolderName);
            }

        }
    }
}
