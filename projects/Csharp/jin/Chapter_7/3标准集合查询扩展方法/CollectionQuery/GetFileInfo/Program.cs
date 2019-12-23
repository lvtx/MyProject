using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetFileInfo
{
    class Program
    {
        static void Main(string[] args)
        {
            var fileList = Directory.GetFiles("D:\\windows\\files\\package");
            var files = fileList.Select((file) => new FileInfo(file));
            var infos = files.Select((info) => new
            {
                FileLength = info.Length,
                FileName = info.Name
            });
            foreach (var info in infos)
            {
                Console.WriteLine(info.FileName + ", " + info.FileLength);
            }
            Console.ReadLine();
        }
    }
}
