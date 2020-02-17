using System;
using System.IO;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FolderExplorer.Filesystem
{
    public class Folder
    {
        internal DriveInfo drive = null;
        public Folder() { }
        public Folder(DriveInfo drive)
        {
            this.drive = drive;
            //去除磁盘名的'\\'
            string str = drive.Name;
            int len = str.Length;
            int count = 0;
            char[] cstr = new char[len];
            for (int i = 0; i < len; i++)
            {
                if (str[i] != '\\')
                {
                    cstr[count++] = str[i];
                }
            }
            str = new string(cstr, 0, count);
            name = string.Format(drive.VolumeLabel + ' ' + '(' + str + ')');
            drive = null;
        }
        private string name;
        /// <summary>
        /// 获取文件夹名称
        /// </summary>
        public virtual string Name
        {
            get
            {
                if (drive == null)
                {
                    name = folder.Name;
                }
                return name;
            }
        }
        /// <summary>
        /// 用于在内部传递外部传入的文件夹参数
        /// </summary>
        internal DirectoryInfo folder;
        /// <summary>
        /// 自定义一个初始路径
        /// </summary>
        public string FullPath
        {
            get
            {
                return folder.FullName;
            }
            set
            {
                if (Directory.Exists(value))
                {
                    //一个标记(重点)
                    this.folder = new DirectoryInfo(value);
                }
                else
                {
                    throw new ArgumentException("must exist", "fullPath");
                }
            }
        }

        private ObservableCollection<Folder> subFolders;
        /// <summary>
        /// 当前文件夹下的子文件夹
        /// </summary>
        public ObservableCollection<Folder> SubFolders
        {
            get
            {
                if (subFolders == null)
                {
                    try
                    {
                        subFolders = new ObservableCollection<Folder>();
                        var subFolderArray = folder.GetDirectories();
                        foreach (var subFolder in subFolderArray)
                        {
                            var newFolder = new Folder();
                            newFolder.FullPath = subFolder.FullName;
                            subFolders.Add(newFolder);
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
                return subFolders;
            }
        }

        //private ObservableCollection<FileInfo> files;
        ///// <summary>
        ///// 当前文件夹下的文件
        ///// </summary>
        //public ObservableCollection<FileInfo> Files
        //{
        //    get
        //    {
        //        if (files == null)
        //        {
        //            try
        //            {
        //                files = new ObservableCollection<FileInfo>();
        //                FileInfo[] fileArray = folder.GetFiles();
        //                foreach (var file in fileArray)
        //                {
        //                    files.Add(file);
        //                }
        //            }
        //            catch (Exception e)
        //            {
        //                Console.WriteLine(e.Message);
        //            }
        //        }
        //        return files;
        //    }
        //}

        private ObservableCollection<File> files;
        /// <summary>
        /// 当前文件夹下的文件
        /// </summary>
        public ObservableCollection<File> Files
        {
            get
            {
                if (files == null)
                {
                    try
                    {
                        files = new ObservableCollection<File>();
                        FileInfo[] fileArray = folder.GetFiles();
                        foreach (var file in fileArray)
                        {
                            var newFile = new File();
                            newFile.FullPath = file.FullName;
                            files.Add(newFile);
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
                return files;
            }
        }

        private string creationTime;
        /// <summary>
        /// 获取文件的创建时间
        /// </summary>
        public string CreationTime
        {
            get
            {
                creationTime = folder.CreationTime.ToString();
                return creationTime;
            }
        }

    }
    /// <summary>
    /// 实用的文件
    /// </summary>
    public class ExplorerFolder : Folder
    {
        public ExplorerFolder():base()
        {          
        }
        public ExplorerFolder(DriveInfo drive) : base(drive)
        {
            base.drive = drive;
        }

        //private ObservableCollection<string> subFolderAndfiles = new ObservableCollection<string>();
        ///// <summary>
        ///// 获得子文件和文件夹的名字
        ///// </summary>
        //public ObservableCollection<string> FolderAndFiles
        //{
        //    get
        //    {
        //        var fol = new Folder();
        //        fol.FullPath = folder.FullName;

        //    }

        //}
        ///// <summary>
        ///// 获取文件或文件夹创建时间
        ///// </summary>
        //private string creationTime;
        //public string CreationTime
        //{
        //    get
        //    {
        //        creationTime = CreationTime.ToString();
        //        return creationTime;
        //    }
        //}
    }
    /// <summary>
    /// 自定义文件
    /// </summary>
    public class File
    {
        /// <summary>
        /// 获取文件名
        /// </summary>
        public string Name
        {
            get { return file.Name; }
        }

        private FileInfo file;
        /// <summary>
        /// 获取文件路径
        /// </summary>
        public string FullPath
        {
            get
            {
                return file.FullName;
            }
            set
            {
                if (System.IO.File.Exists(value))
                {
                    this.file = new FileInfo(value);
                }
                else
                {
                    throw new ArgumentException("must exit", value);
                }
            }
        }
        /// <summary>
        /// 获取文件创建时间
        /// </summary>
        private string creationTime;
        public string CreationTime
        {
            get
            {
                creationTime = file.CreationTime.ToString();
                return creationTime;
            }
        }
    }
}

