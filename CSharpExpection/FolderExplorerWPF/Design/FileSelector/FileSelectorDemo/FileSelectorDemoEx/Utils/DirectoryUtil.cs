using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace FileSelectorDemo.Utils
{
   public class DirectoryUtil
    {
        public static List<Models.FileListItemModel> InitCurrentFileList()
        {          
            List<Models.FileListItemModel> ret = new List<Models.FileListItemModel>();
            var drivers = System.IO.DriveInfo.GetDrives();
            string createTime;
            Icon smallIcon;

            foreach (var driver in drivers)
             {
                string flagLabel = driver.VolumeLabel;
                if (driver.DriveType == DriveType.Network)
                {
                    flagLabel = "网络";
                }
                string driverName= driver.RootDirectory.FullName.Split(new char[] { '\\', ':' }, StringSplitOptions.RemoveEmptyEntries)[0];
                Models.FileListItemModel currentModel = new Models.FileListItemModel();
                currentModel.Size = "未知";
                currentModel.CurrentType = Defines.CurrentType.驱动器;
                currentModel.Name = flagLabel+ "(" + driverName + ")";
                currentModel.CurrentDirectory = driver.RootDirectory.FullName;

                #region 获取系统盘图标  
                smallIcon = Utils.SystemIconUtilEx.GetDriverIcon(driverName.ToCharArray()[0], false);
                currentModel.Icon = Utils.SystemIconUtilEx.ConvertFromIcon(smallIcon);
                #endregion
                try
                {
                    createTime = Directory.GetLastWriteTime(driver.RootDirectory.FullName).ToString();
                }
                catch (Exception ex)
                {
                    createTime = "未知的类型";
                }
                currentModel.CreateTime = createTime;
                ret.Add(currentModel);
            }

            ///添加桌面           
            string path = Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop);          
            Models.FileListItemModel desktopModel = GetSpecialFolderItem(path, "桌面");     
            ret.Add(desktopModel);

            ///最近
            string recentPath = Environment.GetFolderPath(System.Environment.SpecialFolder.Recent);
            Models.FileListItemModel recentModel = GetSpecialFolderItem(recentPath, "最近");
            ret.Add(recentModel);
            return ret;
        }

        private static Models.FileListItemModel GetSpecialFolderItem(string path,string chineseName)
        {
            string createTime;      
            Models.FileListItemModel specialModel = new Models.FileListItemModel();
            specialModel.Size = "未知";
            specialModel.CurrentDirectory = path;
            specialModel.Name = chineseName;
            specialModel.CurrentType = Defines.CurrentType.文件夹;
            try
            {
                createTime = Directory.GetLastWriteTime(path).ToString();
            }
            catch (Exception ex)
            {
                createTime = "未知的类型";
            }
            specialModel.CreateTime = createTime;  
            specialModel.Icon = GetDefaultFolderIcon(); 
            return specialModel;
        }

        /// <summary>
        /// 获取当前的所有的子目录里面的内容
        /// </summary>
        /// <param name="currentDirectory">当前目录</param>
        /// <returns></returns>
        public static List<Models.FileListItemModel> GetCurrentFileList(string currentDirectory)
        { 
            List<Models.FileListItemModel> ret = new List<Models.FileListItemModel>();
            if (Directory.Exists(currentDirectory))
            {
                GetSonFoldersAndFiles(currentDirectory, ret);
            }
            else if (currentDirectory.StartsWith("此电脑"))
            {
                ret.AddRange(InitCurrentFileList());
            }
            return ret;
        }

        /// <summary>
        /// 获取当前路径的所有子文件夹(不包含文件类型)
        /// </summary>
        /// <param name="currentDirectory"></param>
        /// <returns></returns>
        public static List<Models.FileListItemModel> GetCurrentFileListEx(string currentDirectory)
        {
            List<Models.FileListItemModel> ret = new List<Models.FileListItemModel>();            
            GetSonFolders(currentDirectory, ret);           
            return ret;
        }

        /// <summary>
        /// 获取当前的父目录的所有文件及文件夹
        /// </summary>
        /// <param name="currentDirectory">当前目录</param>
        /// <returns></returns>
        public static List<Models.FileListItemModel> GetParentFileList(string currentDirectory)
        {
            List<Models.FileListItemModel> ret = new List<Models.FileListItemModel>();
            if (Directory.Exists(currentDirectory))
            {
                DirectoryInfo parentDirectoryInfo= Directory.GetParent(currentDirectory);
                if (null != parentDirectoryInfo && parentDirectoryInfo.Exists)
                {
                    GetSonFoldersAndFiles(parentDirectoryInfo.FullName, ret);
                }
                else
                {
                   var defaultItems= InitCurrentFileList();
                   ret.AddRange(defaultItems);
                }
            }
            return ret;
        }

        public static Models.FileListItemModel GetParentDirectory(string currentDirectory)
        {            
            if (Directory.Exists(currentDirectory))
            {
                string fullPath=string.Empty;
                string fullName = string.Empty;
                ImageSource bitmap = GetDefaultFolderIcon();
                DirectoryInfo parentDirectoryInfo = Directory.GetParent(currentDirectory);
                if (null != parentDirectoryInfo && parentDirectoryInfo.Exists)
                {
                    fullPath = parentDirectoryInfo.FullName;
                    //判断当前目录是否是根目录
                    string str = Directory.GetDirectoryRoot(fullPath);
                    if(str==fullPath)
                    {
                        string driverName = fullPath.Split(new char[] { '\\', ':' }, StringSplitOptions.RemoveEmptyEntries)[0];
                        fullName = "新加卷"+"("+ driverName + ")";
                        Icon smallIcon = Utils.SystemIconUtilEx.GetDriverIcon(driverName.ToCharArray()[0], false);
                        bitmap =Utils.SystemIconUtilEx.ConvertFromIcon(smallIcon);
                    }
                    else
                    {
                        fullName = parentDirectoryInfo.Name;
                    }                                
                }
                else
                {
                    fullName=fullPath = "此电脑";
                }
                Models.FileListItemModel ret = new Models.FileListItemModel()
                {
                    CurrentDirectory = fullPath,
                    Name = fullName,
                    Icon=bitmap                   
                };
                return ret;
            }
            return null;
        }

        private static void GetSonFoldersAndFiles(string directory, List<Models.FileListItemModel> collections)
        {
            List<Models.FileListItemModel> directoryList = new List<Models.FileListItemModel>();
            List<Models.FileListItemModel> fileList = new List<Models.FileListItemModel>();
            string[] resourceArray = Directory.GetFileSystemEntries(directory);
            foreach (var item in resourceArray)
            {
                try
                {
                    double size = 0;
                    string currentName;
                    string sizeWithUnit = "未知";
                    string createTime;
                    string extension = ".";
                    string parentDirectory = "";
                    Defines.CurrentType type;
                    BitmapSource iconSource = null;
                    if (Directory.Exists(item))
                    {
                        //首先过滤掉隐藏文件夹
                        DirectoryInfo directoryInfo = new DirectoryInfo(item);
                        if (directoryInfo.Attributes.ToString().Contains(FileAttributes.Hidden.ToString()))
                        {
                            continue;
                        }
                        //Icon directoryIcon = Utils.SystemIconUtil.GetDirectoryIcon();
                        //iconSource = Utils.SystemIconUtil.ConvertFromIcon(directoryIcon);         
                        iconSource = GetDefaultFolderIcon();
                        currentName = directoryInfo.Name;
                        parentDirectory = directoryInfo.Parent.FullName;
                        try
                        {
                            createTime = Directory.GetLastWriteTime(item).ToString();
                        }
                        catch (Exception ex)
                        {
                            createTime = "未知的类型";
                        }                        
                        //通过递归的方式来计算文件夹大小，耗用的时间太长，影响用户体验
                        //size = GetDirectoryLength(item);
                        //sizeWithUnit = GetCurrentLengthWithUnit(size);
                        type = Defines.CurrentType.文件夹;
                        extension = ".";
                        Models.FileListItemModel tempModel = new Models.FileListItemModel()
                        {
                            Name = currentName,
                            CreateTime = createTime,
                            Size = sizeWithUnit,
                            CurrentType = type,
                            CurrentDirectory = item,
                            Icon = iconSource,
                            FileExtension = extension,
                            ParentDirectory = parentDirectory
                        };
                        directoryList.Add(tempModel);
                    }
                    else
                    {
                        //首先过滤掉隐藏文件
                        FileInfo fileInfo = new FileInfo(item);
                        if (fileInfo.Attributes.ToString().Contains(FileAttributes.Hidden.ToString()))
                        {
                            continue;
                        }
                        parentDirectory = fileInfo.Directory.FullName;
                        extension = System.IO.Path.GetExtension(item);
                        #region 获取系统文件图标
                        Icon large, small;
                        string des;
                        Utils.SystemIconUtil.GetExtsIconAndDescription(extension, out large, out small, out des);
                        iconSource = Utils.SystemIconUtil.ConvertFromIcon(large);
                        #endregion
                        currentName = fileInfo.Name;                       
                        try
                        {
                            createTime = File.GetLastWriteTime(item).ToString();
                        }
                        catch (Exception ex)
                        {
                            createTime = "未知的类型";
                        }
                        size = fileInfo.Length;
                        sizeWithUnit = GetCurrentLengthWithUnit(size);
                        type = Defines.CurrentType.文件;

                        Models.FileListItemModel tempModel = new Models.FileListItemModel()
                        {
                            Name = currentName,
                            CreateTime = createTime,
                            Size = sizeWithUnit,
                            CurrentType = type,
                            CurrentDirectory = item,
                            Icon = iconSource,
                            FileExtension = extension,
                            ParentDirectory = parentDirectory
                        };
                        fileList.Add(tempModel);
                    }              
                }
                catch (Exception ex)
                {
                    continue;
                }
            }
            //添加时按照文件夹在上，文件在下的方式来添加到集合中
            collections.AddRange(directoryList);
            collections.AddRange(fileList);
        }

        /// <summary>
        /// 获取当前路径下的所有子文件夹
        /// </summary>
        /// <param name="directory">当前路径</param>
        /// <param name="collections"></param>
        private static void GetSonFolders(string directory, List<Models.FileListItemModel> collections)
        {           
            if (directory.StartsWith("此电脑"))
            {
                collections.AddRange(InitCurrentFileList());
                return;
            }
            if (!Directory.Exists(directory))
            {
                return;
            }
            string[] resourceArray = Directory.GetDirectories(directory);
            foreach (var item in resourceArray)
            {
                try
                {
                    string currentName;
                    string sizeWithUnit = "未知";
                    string createTime;
                    string extension = ".";
                    string parentDirectory = "";
                    Defines.CurrentType type;
                    BitmapSource iconSource = null;
                    if (Directory.Exists(item))
                    {
                        //首先过滤掉隐藏文件夹
                        DirectoryInfo directoryInfo = new DirectoryInfo(item);
                        if (directoryInfo.Attributes.ToString().Contains(FileAttributes.Hidden.ToString()))
                        {
                            continue;
                        }
                        //Icon directoryIcon = Utils.SystemIconUtil.GetDirectoryIcon();
                        //iconSource = Utils.SystemIconUtil.ConvertFromIcon(directoryIcon);
                        iconSource = GetDefaultFolderIcon();
                        currentName = directoryInfo.Name;
                        parentDirectory = directoryInfo.Parent.FullName;                       
                        try
                        {
                            createTime = Directory.GetLastWriteTime(item).ToString();
                        }
                        catch (Exception ex)
                        {
                            createTime = "未知的类型";
                        }
                        //通过递归的方式来计算文件夹大小，耗用的时间太长，影响用户体验
                        //size = GetDirectoryLength(item);
                        //sizeWithUnit = GetCurrentLengthWithUnit(size);
                        type = Defines.CurrentType.文件夹;
                        extension = ".";

                        Models.FileListItemModel tempModel = new Models.FileListItemModel()
                        {
                            Name = currentName,
                            CreateTime = createTime,
                            Size = sizeWithUnit,
                            CurrentType = type,
                            CurrentDirectory = item,
                            Icon = iconSource,
                            FileExtension = extension,
                            ParentDirectory = parentDirectory
                        };
                        collections.Add(tempModel);
                    }
                }
                catch (Exception ex)
                {
                   continue ;
                }                
            }
        }

        public static BitmapImage GetDefaultFolderIcon()
        {
            try
            {
                string source = "/FileSelectorDemo;component/Resources/Images/文件夹.png";
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(source, UriKind.RelativeOrAbsolute);
                bitmap.EndInit();
                return bitmap;
            }
            catch (Exception ex)
            {
                ;
            }
            return null;
        }


        //获取当前路径下的文件及文件夹大小
        public static double GetDirectoryLength(string dirPath)
        {
            if (!Directory.Exists(dirPath))
            {
                return 0;
            }
            double len=0;
            //定义一个DirectoryInfo对象
            DirectoryInfo directoryInfo = new DirectoryInfo(dirPath);
            //循环遍历每一个文件的大小
            foreach (var file in directoryInfo.GetFiles())
            {
                len += file.Length;
            }
            //获取di中所有的文件夹,并存到一个新的对象数组中,以进行递归
            DirectoryInfo[] dis = directoryInfo.GetDirectories();
            if (dis.Length > 0)
            {
                for (int i = 0; i < dis.Length; i++)
                {
                    len += GetDirectoryLength(dis[i].FullName);
                }
            }
            return len;
        }

        //给当前的按照字节Byte计数的单位按照KB,MB,GB等更大的单位来计数
        public static string GetCurrentLengthWithUnit(double lengtn)
        {
            string lengthWithUnit = string.Empty;
            StringBuilder sb = new StringBuilder();
            int depth = 0;
            double afterDivideBy1024= lengtn;           
            while (afterDivideBy1024 > 1024|| afterDivideBy1024 == 1024)
            {
                depth++;
                afterDivideBy1024 /= 1024;              
            }           
            switch (depth)
            {
                case 0:
                    lengthWithUnit = sb.Append(afterDivideBy1024.ToString("f2")).Append(" ").Append("B").ToString();
                    break;
                case 1:               
                    lengthWithUnit = sb.Append(afterDivideBy1024.ToString("f2")).Append(" ").Append("KB").ToString();
                    break;
                case 2:
                    lengthWithUnit = sb.Append(afterDivideBy1024.ToString("f2")).Append(" ").Append("MB").ToString();
                    break;
                case 3:
                    lengthWithUnit = sb.Append(afterDivideBy1024.ToString("f2")).Append(" ").Append("GB").ToString();
                    break;
                case 4:
                    lengthWithUnit = sb.Append(afterDivideBy1024.ToString("f2")).Append(" ").Append("TB").ToString();
                    break;
                case 5:
                    lengthWithUnit = sb.Append(afterDivideBy1024.ToString("f2")).Append(" ").Append("PB").ToString();
                    break;
                case 6:
                    lengthWithUnit = sb.Append(afterDivideBy1024.ToString("f2")).Append(" ").Append("EB").ToString();
                    break;
            }
            return lengthWithUnit;
        }
        
    }
}
