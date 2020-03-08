using System;
using FolderExplorer.Filesystem;
using System.IO;
using System.Collections.ObjectModel;
using FolderExplorer;
using Microsoft.Win32;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Drawing;
using System.Management;
using System.Collections.Generic;

namespace UnitTestFolder
{
    [TestClass]
    public class TestFolder
    {
        [TestMethod]
        public void TestSingleFolder()
        {
            DriveInfo[] drives = DriveInfo.GetDrives();
            FileSys folder = new Folder();
            folder.FullPath = "C:\\";
            Console.Write("文件名");
            Console.WriteLine(folder.Name);
            Console.Write("文件创建时间：");
        }
        [TestMethod]
        public void TestMultiFolder()
        {
            DriveInfo[] drives = DriveInfo.GetDrives();
            Folder folder = new Folder();
            folder.FullPath = "C:\\";
            ObservableCollection<Folder> subFolders = new ObservableCollection<Folder>();
            subFolders = folder.SubFolders;
            foreach (var subFolder in subFolders)
            {
                Console.Write("文件 {0} : ", subFolder.Name);
                Console.WriteLine("创建时间 {0}", subFolder.CreationTime);
            }
        }
        [TestMethod]
        public void TestDrive()
        {
            DriveInfo[] drives = DriveInfo.GetDrives();
            //输出驱动器信息
            foreach (var drive in drives)
            {
                Console.WriteLine("驱动器:{0}", drive.Name);
                Console.WriteLine("类型:{0}", drive.DriveType);
                // 检查驱动器是否已经准备好
                if (drive.IsReady)
                {
                    Console.WriteLine("空闲空间:{0}", drive.TotalFreeSpace);
                    Console.WriteLine("格式:{0}", drive.DriveFormat);
                    Console.WriteLine("卷标:{0}", drive.VolumeLabel);
                }
                Console.WriteLine("-----------------------------");
            }
        }
        [TestMethod]
        public void TestFolderAndFiles()
        {
            //ExplorerFolder folder = new ExplorerFolder();
            //folder.FullPath = "C:\\";
            //var folderandfiles = new ObservableCollection<object>();
            //folderandfiles = folder.FolderAndFiles;
            //foreach (var folderandfile in folderandfiles)
            //{
            //    Console.WriteLine(folderandfile);
            //}
        }
        [TestMethod]
        public void UseFileInfo()
        {
            var file = new FileInfo(@"D:\a.txt");
            Console.WriteLine(file.FullName);
            Console.WriteLine("文件名 {0}", file.Name);
            Console.WriteLine("父文件夹名 {0}", file.DirectoryName);
            Console.WriteLine("文件属性 {0}", file.Attributes);
            Console.WriteLine("文件大小 {0}", file.Length);
        }

        [TestMethod]
        public void ShowIcon()
        {
            Icon ico = Icon.ExtractAssociatedIcon(@"C:\WINDOWS\system32\notepad.exe");
            Console.WriteLine(ico);
        }

        [TestMethod]
        public void TestSystemDirectory()
        {
            var Dirs = Environment.GetLogicalDrives();
            foreach (var dir in Dirs)
            {
                Console.WriteLine(dir);
            }
            var d = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            Console.WriteLine(d);
        }
        [TestMethod]
        public void TestFileType()
        {
            FileInfo file = new FileInfo(@"C:\OneDriveTemp");
            if ((file.Attributes & FileAttributes.Hidden) == FileAttributes.Hidden)
            {
                Console.WriteLine(file.Attributes);
            }
        }
        [TestMethod]
        public void DisplayTypedPaths()
        {
            //定义注册表顶级结点 命名空间Microsoft.Win32
            Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey
            ("Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\TypedPaths", true);
            int num = 0;
            //判断键是否存在
            if (key != null)
            {
                //检索包含此项关联的所有值名称 即url1 url2 url3
                string[] names = key.GetValueNames();
                foreach (string str in names)
                {
                    //获取url中相关联的值
                    Console.WriteLine(key.GetValue(str).ToString());
                    num++;
                }
                //显示获取文件总数              
            }
            Console.WriteLine("总文件个数为{0}", num);
        }
        [TestMethod]
        public void DisplaySubKey()
        {
            Microsoft.Win32.RegistryKey rk = Microsoft.Win32.Registry.Users;
            string[] names = rk.GetSubKeyNames();
            int icount = 0;
            Console.WriteLine("子键为" + rk.Name);
            Console.WriteLine("---------------------------------");
            foreach (String s in names)
            {
                Console.WriteLine(s);
                icount++;
                if (icount >= 10)
                {
                    break;
                }
            }
        }
        [TestMethod]
        public void HowToSetValue()
        {
            // The name of the key must include a valid root.
            const string userRoot = "HKEY_CURRENT_USER";
            const string subkey = "RegistrySetValueExample";
            const string keyName = userRoot + "\\" + subkey;

            // An int value can be stored without specifying the
            // registry data type, but long values will be stored
            // as strings unless you specify the type. Note that
            // the int is stored in the default name/value
            // pair.
            Registry.SetValue(keyName, "", 5280);
            Registry.SetValue(keyName, "TestLong", 12345678901234,
                RegistryValueKind.QWord);

            // Strings with expandable environment variables are
            // stored as ordinary strings unless you specify the
            // data type.
            Registry.SetValue(keyName, "TestExpand", "My path: %path%");
            Registry.SetValue(keyName, "TestExpand2", "My path: %path%",
                RegistryValueKind.ExpandString);

            // Arrays of strings are stored automatically as 
            // MultiString. Similarly, arrays of Byte are stored
            // automatically as Binary.
            string[] strings = { "One", "Two", "Three" };
            Registry.SetValue(keyName, "TestArray", strings);

            // Your default value is returned if the name/value pair
            // does not exist.
            string noSuch = (string)Registry.GetValue(keyName,
                "NoSuchName",
                "Return this default if NoSuchName does not exist.");
            Console.WriteLine("\r\nNoSuchName: {0}", noSuch);

            // Retrieve the int and long values, specifying 
            // numeric default values in case the name/value pairs
            // do not exist. The int value is retrieved from the
            // default (nameless) name/value pair for the key.
            int tInteger = (int)Registry.GetValue(keyName, "", -1);
            Console.WriteLine("(Default): {0}", tInteger);
            long tLong = (long)Registry.GetValue(keyName, "TestLong",
                long.MinValue);
            Console.WriteLine("TestLong: {0}", tLong);

            // When retrieving a MultiString value, you can specify
            // an array for the default return value. 
            string[] tArray = (string[])Registry.GetValue(keyName,
                "TestArray",
                new string[] { "Default if TestArray does not exist." });
            for (int i = 0; i < tArray.Length; i++)
            {
                Console.WriteLine("TestArray({0}): {1}", i, tArray[i]);
            }

            // A string with embedded environment variables is not
            // expanded if it was stored as an ordinary string.
            string tExpand = (string)Registry.GetValue(keyName,
                 "TestExpand",
                 "Default if TestExpand does not exist.");
            Console.WriteLine("TestExpand: {0}", tExpand);

            // A string stored as ExpandString is expanded.
            string tExpand2 = (string)Registry.GetValue(keyName,
                "TestExpand2",
                "Default if TestExpand2 does not exist.");
            Console.WriteLine("TestExpand2: {0}...",
                tExpand2.Substring(0, 40));

            Console.WriteLine("\r\nUse the registry editor to examine the key.");
            Console.WriteLine("Press the Enter key to delete the key.");
            Console.ReadLine();
            Registry.CurrentUser.DeleteSubKey(subkey);
            //
            // This code example produces output similar to the following:
            //
            //NoSuchName: Return this default if NoSuchName does not exist.
            //(Default): 5280
            //TestLong: 12345678901234
            //TestArray(0): One
            //TestArray(1): Two
            //TestArray(2): Three
            //TestExpand: My path: %path%
            //TestExpand2: My path: D:\Program Files\Microsoft.NET\...
            //
            //Use the registry editor to examine the key.
            //Press the Enter key to delete the key.
        }
        [TestMethod]
        public void DisplayLocalMachine()
        {
            Microsoft.Win32.RegistryKey rk = Microsoft.Win32.Registry.LocalMachine;
            string[] names = rk.GetSubKeyNames();
            int icount = 0;
            Console.WriteLine("子键为" + rk.Name);
            Console.WriteLine("---------------------------------");
            foreach (String s in names)
            {
                Console.WriteLine(s);
                icount++;
                if (icount >= 10)
                {
                    break;
                }
            }
        }
        [TestMethod]
        public void WhatIsMyComputer()
        {
            string paths = Environment.GetFolderPath(Environment.SpecialFolder.MyComputer);
            foreach (var path in paths)
            {
                Console.WriteLine(path);
            }
        }
    }
}


