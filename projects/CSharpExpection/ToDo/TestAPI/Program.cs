using System;
using System.IO;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestAPI
{
    class Program
    {
        static void Main(string[] args)
        {
            DriveInfo[] allDrives = DriveInfo.GetDrives();
            foreach (var drive in allDrives)
            {
                Console.WriteLine("name:{0}", drive.Name);
            }
            DirectoryInfo directoryInfo = new DirectoryInfo(@"C:\");

            Console.WriteLine("Origin:{0}", directoryInfo);
            Console.WriteLine("FullName:{0}", directoryInfo.FullName);
            Folder fl = new Folder();
            fl.folder = new DirectoryInfo(@"E:\");
            foreach (var item in fl.Files)
            {
                Console.WriteLine("folder.files:{0}",item);
            }
            Number num = new Number();
            
            Console.ReadLine();

        }
    }
    class Folder
    {
        public DirectoryInfo folder { get; set; }
        public List<FileInfo> files { get; set; }
        public List<FileInfo> Files
        {
            get
            {
                if (this.files == null)
                {
                    this.files = new List<FileInfo>();
                    FileInfo[] fi = this.folder.GetFiles();
                    for (int i = 0; i < fi.Length - 1; i++)
                    {
                        this.files.Add(fi[i]);
                    }
                }
                return this.files;
            }
        }
    }
    class Number
    {
        public int a { get; set; }
        private int add1;
        public int Add1
        {
            
            get 
            {
                if (a == 0)
                {
                    add1 += 1;
                }
                return add1;
            }
        }
    }
}

