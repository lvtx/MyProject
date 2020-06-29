using System;
using System.IO;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Media.Imaging;

namespace FolderExplorer.Filesystem
{
    public abstract class FileSys
    {
        public virtual string Name { get; set; }
        public virtual string FullPath { get; set; }
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
                if (name == null)
                {
                    name = folder.Name;
                }
                return name;
            }
            set
            {
                name = value;
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

        private string creationTime;
        /// <summary>
        /// 获取文件的创建时间
        /// </summary>
        public string CreationTime
        {
            get
            {
                creationTime = folder.LastWriteTime.ToString();
                return creationTime;
            }
        }
        /// <summary>
        /// 获取文件夹的图标
        /// </summary>
        private Icon icon;

        public Icon iIcon
        {
            get
            {
                if(icon == null)
                    icon = IconInfo.GetFolderIcon(false);
                return icon;
            }
            set
            {
                icon = value;
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
                if ((folder.Attributes & FileAttributes.Directory) == FileAttributes.Directory)
                {
                    fType = "文件夹";
                }
                
                return fType;
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
        /// 获取文件修改时间
        /// </summary>
        private string creationTime;
        public string CreationTime
        {
            get
            {
                creationTime = file.LastWriteTime.ToString();
                return creationTime;
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
                fType = FileTypeInfo.GetFileType(file.FullName);
                return fType;
            }
        }

        /// <summary>
        /// 获取文件图标
        /// </summary>
        private Icon icon;

        public Icon iIcon
        {
            get
            {
                if (icon == null)
                {
                    var path = file.FullName;
                    icon = IconInfo.GetFileIcon(path,false);
                }
                return icon;
            }
        }

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

