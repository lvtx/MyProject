using System;
using System.IO;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.AccessControl;
using System.Windows.Forms;

namespace CalculateFolderSize
{
    public static class Files
    {
        /// <summary>
        /// 当前文件夹及其子文件夹所有文件集合
        /// </summary>
        private static ObservableCollection<FileInfo> files = new ObservableCollection<FileInfo>();
        public static ObservableCollection<FileInfo> AllFile
        {
            get { return files; }
        }
        private static long size;

        public static long Size
        {
            get { return size / 1024 / 1024 / 1024; }
        }

        /// <summary>
        /// 获取当前文件夹及其子文件夹所有文件
        /// </summary>
        /// <param name="path"></param>
        public static void GetAllFile(string path)
        {
            DirectoryInfo folder = new DirectoryInfo(path);
            DirectorySecurity s = new DirectorySecurity(path, AccessControlSections.Access);
            DirectoryInfo[] subFolders = folder.GetDirectories();
            if (subFolders == null)
            {
                return;
            }
            if (!s.AreAccessRulesProtected)
            {
                try
                {
                    //得到当前文件夹下的文件
                    FileInfo[] fils = folder.GetFiles();
                    foreach (var fil in fils)
                    {
                        files.Add(fil);
                        size += fil.Length;
                    }
                    //获取当前文件夹下的子文件夹
                    foreach (var fol in subFolders)
                    {
                        GetAllFile(fol.FullName);
                    }
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}
