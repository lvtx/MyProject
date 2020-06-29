using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace AsyncCalculateFolderSize2
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

        //定义一个委托
        public delegate long CalculateFolderSizeDelegate(string FolderName);

        static void Main(string[] args)
        {
            //定义一个委托变量引用静态方法CalculateFolderSize
            CalculateFolderSizeDelegate d = CalculateFolderSize;

            Console.WriteLine("请输入文件夹名称(例如:C:\\Windows):");

            string FolderName = Console.ReadLine();

            //通过委托异步调用静态方法CalculateFolderSize
            IAsyncResult ret = d.BeginInvoke(FolderName, null, null);

            Console.Write ("正在计算中，请耐心等待");

            while (ret.IsCompleted == false)
            {
                Console.Write(".");
                //每隔2秒检查一次
                System.Threading.Thread.Sleep(2000);
            }

            //阻塞，等到调用完成，取出结果
            long size = d.EndInvoke(ret);

            Console.WriteLine("\n计算完成。文件夹{0}的容量为:{1}字节\n", FolderName, size);

            Console.ReadKey();
        }
    }
}
