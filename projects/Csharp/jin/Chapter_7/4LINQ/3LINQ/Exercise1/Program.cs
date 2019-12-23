using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace Exercise1
{
    /*使用LINQ查询指定文件夹下的所有文件，按其类型进行分组，
     * 在每组中按文件大小进行排序，显示在屏幕上*/
    class Program
    {
        static void Main(string[] args)
        {
            var filesGroup =
                from file in
                Directory.GetFiles("D:\\windows\\files\\document\\ebook\\教材\\大学\\软件\\操作系统")
                let fileInfo = new FileInfo(file)
                orderby fileInfo.Length
                group fileInfo by Path.GetExtension(fileInfo.FullName);
            foreach (var files in filesGroup)
            {
                Console.WriteLine("==========={0}类文件==========",files.Key);
                foreach (var file in files)
                {
                    Console.WriteLine("{0}文件         :{1}字节",file.Name,file.Length);
                }
            }
            Console.ReadLine();
        }
    }
}
