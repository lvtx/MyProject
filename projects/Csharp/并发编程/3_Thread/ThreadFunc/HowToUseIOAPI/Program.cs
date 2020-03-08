using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HowToUseIOAPI
{
    class Program
    {
        static void Main(string[] args)
        {
            string searchPath = @"D:\windows\files\document\ebook\教材\大学\软件\软件工程";
            //获得一个文件夹
            DirectoryInfo directory = new DirectoryInfo(searchPath);
            //获取当前文件夹下所有文件
            FileInfo[] files = directory.GetFiles("*.pdf",SearchOption.TopDirectoryOnly);
            int i = 0;
            FileInfo[] tfiles = files;
            //Console.WriteLine(files[2].Name);
            foreach (var file in files)
            {
                if (file.Length > 1000)
                {
                    tfiles[i++] = file;
                    Console.WriteLine("{0},{1}",file,file.Length);
                }
            }
            Console.WriteLine("-------------------------------------");
            foreach (var file in tfiles)
            {
                Console.WriteLine("{0},{1}", file, file.Length);
            }
            //foreach (var file in files)
            //{
            //    Console.WriteLine(file);
            //}
            Console.ReadLine();
        }
    }
}
