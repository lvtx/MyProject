using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections;
using System.Windows.Forms;

namespace UseWhereFunc
{
    class FileBrowserHelper
    {
        /// <summary>
        /// 获取指定文件夹下的所有文件(按大小升序排列)
        /// 有些文件夹要访问可能需要有特殊的权限,当不能拥有此权限时,简单地返回一个NULL指针
        /// </summary>
        /// <param name="FolderName"></param>
        /// <returns></returns>
        public static List<FileInfo> GetAllFilesInFolder(string FolderName)
        {
            try
            {
                List<FileInfo> files = null;
                if (Directory.Exists(FolderName))
                {
                    DirectoryInfo dir = new DirectoryInfo(FolderName);
                    files = dir.GetFiles().ToList<FileInfo>();
                }
                return files;
            }
            catch
            {

                return null;
            }

        }

        /// <summary>
        /// 获取指定子文件夹下的所有子文件夹
        /// </summary>
        /// <param name="FolderName"></param>
        /// <returns></returns>
        public static List<DirectoryInfo> GetAllSubDirectories(string FolderName)
        {
            try
            {
                List<DirectoryInfo> dirs = null;
                if (Directory.Exists(FolderName))
                {
                    DirectoryInfo dir = new DirectoryInfo(FolderName);
                    dirs = dir.GetDirectories().ToList<DirectoryInfo>();

                }
                return dirs;
            }
            catch (Exception)
            {

                return null;
            }

        }


        /// <summary>
        /// 在一个列表框中显示集合中的所有文件名
        /// </summary>
        /// <param name="files"></param>
        /// <param name="list"></param>
        public static void ShowFileListInListBox(IEnumerable<FileInfo> files, ListBox list)
        {
            list.Items.Clear();
            foreach (FileInfo file in files)
                list.Items.Add(file.Name);
        }
    }
}
