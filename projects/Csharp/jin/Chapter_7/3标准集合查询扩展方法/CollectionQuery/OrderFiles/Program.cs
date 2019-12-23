using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderFiles
{
    class Program
    {
        static void Main(string[] args)
        {
            var files = Directory.GetFiles("D:\\windows\\files\\package");
            var infos = files.Select(file => new FileInfo(file))
                .OrderBy(info => info.Length)
                .ThenBy(info => info.Name);
            foreach (var item in infos)
            {
                Console.WriteLine("{0}:为{1}字节",item.Name,item.Length);
            }
            //var infos = files.Select(file => new FileInfo(file));
            Console.ReadLine();
        }
    }
}
