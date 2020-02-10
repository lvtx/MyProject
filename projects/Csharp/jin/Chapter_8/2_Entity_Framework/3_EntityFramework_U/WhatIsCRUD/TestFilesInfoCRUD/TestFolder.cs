using System;
using System.IO;
using FilesInfoCRUD;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestFilesInfoCRUD
{
    [TestClass]
    public class TestFolder
    {
        [TestMethod]
        public void TestUseFolder()
        {
            Drive myDrive = new Drive();
            DriveInfo[] drives = myDrive.Drives;
            //StoreDBDrive storeDBDrive = new StoreDBDrive(drives);
            //storeDBDrive.StoreDrive();
            //storeDBDrive.GetResult();
            foreach (var d in drives)
            {
                Folder currentFolder = new Folder();
                currentFolder.FullPath = d.Name;
                Console.WriteLine($"------------目录{d.Name}-----------");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("------------子文件夹-------------");
                foreach (var subFolder in currentFolder.SubFolders)
                {
                    Console.WriteLine(subFolder.Name);
                }
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("------------文件-------------");
                foreach (var file in currentFolder.Files)
                {
                    Console.WriteLine(file.Name);
                }
            }
        }
    }
}
