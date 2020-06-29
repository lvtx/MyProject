using System;
using System.IO;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Media.Imaging;
using System.ComponentModel;

namespace FolderExplorer.Filesystem
{
    public abstract class FileSys : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }
        /// <summary>
        /// 获取一个文件或文件夹
        /// </summary>
        protected FileSystemInfo folderOrfile;
        /// <summary>
        /// 获取文件实例
        /// </summary>
        protected DirectoryInfo folder;
        /// <summary>
        /// 获取文件实例 
        /// </summary>
        protected FileInfo file;
        protected string name;
        /// <summary>
        /// 获取文件夹或文件名称
        /// </summary>
        public string Name
        {
            get
            {
                if (name == null)
                {
                    name = folderOrfile.Name;
                }
                return name;
            }
            set
            {
                name = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Name"));
            }
        }
        /// <summary>
        /// 传入路径创建一个文件或文件夹对象
        /// </summary>
        public string FullPath
        {
            get
            {
                return folderOrfile.FullName;
            }
            set
            {
                if (Directory.Exists(value))
                {
                    //一个标记(重点)
                    this.folder = new DirectoryInfo(value);
                    folderOrfile = this.folder;
                }
                else if (System.IO.File.Exists(value))
                {
                    this.file = new FileInfo(value);
                    folderOrfile = this.file;
                }
                else
                {
                    throw new ArgumentException("must exist", value);
                }
                OnPropertyChanged(new PropertyChangedEventArgs("FullPath"));
            }
        }

        /// <summary>
        /// 获取文件或文件夹修改时间
        /// </summary>
        private string changeTime;
        public string ChangeTime
        {
            get
            {
                changeTime = folderOrfile.LastWriteTime.ToString();
                return changeTime;
            }
        }
        /// <summary>
        /// 获取文件夹或文件的图标
        /// </summary>
        private Icon icon;

        public Icon iIcon
        {
            get
            {
                if (icon == null)
                {
                    switch (folderOrfile is DirectoryInfo)
                    {
                        case true:
                            icon = IconInfo.GetFolderIcon(false);
                            break;
                        case false:
                            {
                                var path = file.FullName;
                                icon = IconInfo.GetFileIcon(path, true);
                            }
                            break;
                        default:
                            break;
                    }
                }
                return icon;
            }
            set
            {
                icon = value;
                OnPropertyChanged(new PropertyChangedEventArgs("iIcon"));
            }
        }

        /// <summary>
        /// 获取文件类型
        /// </summary>
        private string fType;

        public string FType
        {
            get
            {
                switch (folderOrfile is DirectoryInfo)
                {
                    case true:
                        {
                            if ((folder.Attributes & FileAttributes.Directory) == FileAttributes.Directory)
                            {
                                fType = "文件夹";
                            }
                        }
                        break;
                    case false:
                        {
                            fType = FileTypeInfo.GetFileType(file.FullName);
                        }
                        break;
                    default:
                        break;
                }
                return fType;
            }
        }
        /// <summary>
        /// 获取文件大小
        /// </summary>
        private long? size;

        public long? Size
        {
            get 
            {
                if (file != null)
                    //空格是用来保持与类型cell之间的距离
                    size = (file.Length / 1024);
                //文件夹的大小为空
                else size = null;
                return size;
            }
        }

    }

    /// <summary>
    /// 自定义文件夹类型
    /// </summary>
    public class Folder : FileSys
    {
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
                            if ((subFolder.Attributes & FileAttributes.System) != FileAttributes.System)
                            {
                                var newFolder = new Folder();
                                newFolder.FullPath = subFolder.FullName;
                                subFolders.Add(newFolder);
                            }
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
                            if ((file.Attributes & FileAttributes.System) != FileAttributes.System)
                            {
                                var newFile = new File();
                                newFile.FullPath = file.FullName;
                                files.Add(newFile);
                            }
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
                        foreach (var subfolder in folderOrfile.SubFolders)
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
    /*======================================================================*/
    /// <summary>
    /// 自定义文件类型
    /// </summary>
    public class File : FileSys
    {
        //private Icon icon;
        ///// <summary>
        ///// 获取文件图标的另一种方法
        ///// </summary>
        //public Icon iIcon
        //{
        //    get
        //    {
        //        var path = this.file.FullName;
        //        icon = System.Drawing.Icon.ExtractAssociatedIcon(path);
        //        return icon;
        //    }
        //}
    }
}

