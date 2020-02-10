using System;
using System.IO;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Media.Imaging;

namespace TestFolder
{
    public abstract class FileSys
    {
        public virtual string Name { get; set; }
        public virtual string FullPath { get; set; }
    }

    public class AllDrives
    {
        //public Drives()
        //{
        //    DriveInfo[] drives = DriveInfo.GetDrives();
        //    foreach (var drive in drives)
        //    {
        //        Drive rootFolder = new Drive();
        //        rootFolder.FullPath = drive.Name;
        //        Add(rootFolder);
        //    }
        //}
        private ObservableCollection<Drive> drives;

        public ObservableCollection<Drive> Drives
        {
            get 
            {
                drives = new ObservableCollection<Drive>();
                var ds = DriveInfo.GetDrives();
                foreach (var drive in ds)
                {
                    Drive rootFolder = new Drive();
                    rootFolder.FullPath = drive.Name;
                    drives.Add(rootFolder);
                }
                return drives;
            }
        }
    }

    /// <summary>
    /// 自定义磁盘
    /// </summary>
    public class Drive : FileSys
    {
        private string name;
        /// <summary>
        /// 获取文件夹名称
        /// </summary>
        public override string Name
        {
            get
            {
                name = drive.Name;
                return name;
            }
        }
        private DriveInfo drive;
        public override string FullPath
        {
            get 
            {
                return drive.Name;
            }
            set 
            {
                drive = new DriveInfo(value);
            }
        }

        private Folder folder;

        public Folder iFolder
        {
            get 
            {
                if (folder == null)
                {
                    folder = new Folder();
                    folder.FullPath = drive.Name;                   
                }
                return folder;
            }
        }
    }

    /// <summary>
    /// 自定义文件夹类型
    /// </summary>
    public class Folder:FileSys
    {
        private string name;
        /// <summary>
        /// 获取文件夹名称
        /// </summary>
        public override string Name
        {
            get
            {
                name = folder.Name;
                return name;
            }
        }
        /// <summary>
        /// 用于在内部传递外部传入的文件夹参数
        /// </summary>
        private DirectoryInfo folder;
        /// <summary>
        /// 自定义一个初始路径
        /// </summary>
        public override string FullPath
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
                    throw new ArgumentException("must exist", value);
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

        //private string icon;

        //public string iIcon
        //{
        //    get
        //    {
        //        //icon = FolderExplorer.HomePage.
        //        return icon;
        //    }
        //}



        private ObservableCollection<FileSys> subFolderAndfiles;
        /// <summary>
        /// 获得子文件和文件夹的名字
        /// </summary>
        public ObservableCollection<FileSys> FolderAndFiles
        {
            get
            {
                //这里
                if (subFolderAndfiles == null)
                {
                    try
                    {
                        //这里要研究一下
                        subFolderAndfiles = new ObservableCollection<FileSys>();
                        var folderOrfile = new Folder();
                        folderOrfile.FullPath = folder.FullName;
                        foreach (var subfolder in SubFolders)
                        {
                            subFolderAndfiles.Add(subfolder);
                        }
                        foreach (var file in folderOrfile.Files)
                        {
                            subFolderAndfiles.Add(file);
                        }
                        return subFolderAndfiles;
                    }
                    catch (Exception)
                    {
                        throw new ArgumentException("must exist", "path");
                    }
                }
                return subFolderAndfiles;
            }
        }
    }

    /// <summary>
    /// 自定义文件类型
    /// </summary>
    public class File : FileSys
    {
        /// <summary>
        /// 获取文件名
        /// </summary>
        public override string Name
        {
            get { return file.Name; }
        }

        private FileInfo file;
        /// <summary>
        /// 获取文件路径
        /// </summary>
        public override string FullPath
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

        //private BitmapImage icon;

        //public Bitmap iIcon
        //{
        //    get 
        //    {
        //        var path = this.file.FullName;
        //        BitmapSource bitmap;
        //        if (icon == null && System.IO.File.Exists(path))
        //        {
        //            using (System.Drawing.Icon sysicon = System.Drawing.Icon.ExtractAssociatedIcon(path))
        //            {
        //                bitmap = System.Windows.Interop.Imaging.CreateBitmapSourceFromHIcon(
        //                          sysicon.Handle,
        //                          System.Windows.Int32Rect.Empty,
        //                          System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());
        //            }
                    
        //        }

        //        return icon;
        //    }
        //}

    }
}

