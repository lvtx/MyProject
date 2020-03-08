using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace UseLetStatement
{
    class Program
    {
        static void Main(string[] args)
        {


            IEnumerable<FileInfo> files =from fileName in Directory.GetFiles("C:\\")
                        let file = new FileInfo(fileName)
                        orderby file.Length, fileName
                        select file;

            //相当于
//            IEnumerable<FileInfo> files =
//from fileName in Directory.GetFiles("c:\\")
//orderby new FileInfo(fileName).Length, fileName
//select new FileInfo(fileName);

            foreach (var file in files)
            {
                Console.WriteLine(file.Name);
            }

            Console.ReadKey();
        }
    }
}
