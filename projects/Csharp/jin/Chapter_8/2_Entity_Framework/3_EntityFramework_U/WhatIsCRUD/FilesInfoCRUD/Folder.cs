using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilesInfoCRUD
{
    public class Folder
    {
        /// <summary>
        /// 获取文件夹名称
        /// </summary>
        public string Name
        {
            get { return folder.Name; }
        }

        private DirectoryInfo folder;
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
                    this.folder = new DirectoryInfo(value);
                }
                else
                {
                    throw new ArgumentException("must exist", "fullPath");
                }
            }
        }

        private List<Folder> subFolders;
        /// <summary>
        /// 当前文件夹下的子文件夹
        /// </summary>
        public List<Folder> SubFolders
        {
            get
            {
                if (subFolders == null)
                {
                    try
                    {
                        subFolders = new List<Folder>();
                        var dirArray = folder.GetDirectories();
                        foreach (var dir in dirArray)
                        {
                            var newFolder = new Folder();
                            newFolder.FullPath = dir.FullName;
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

        private List<FileInfo> files;
        /// <summary>
        /// 当前文件夹下的文件
        /// </summary>
        public List<FileInfo> Files
        {
            get
            {
                if (files == null)
                {
                    try
                    {
                        files = new List<FileInfo>();
                        FileInfo[] fileArray = folder.GetFiles();
                        foreach (var fi in fileArray)
                        {
                            files.Add(fi);
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
    }
}
