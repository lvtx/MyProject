using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using System.IO;
using WinForm = System.Windows.Forms;

namespace UseCollectionView
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {

        /// <summary>
        /// 获取指定文件夹下的所有文件
        /// </summary>
        /// <param name="DirectoryName"></param>
        /// <returns></returns>
        public static List<FileInfo> GetFiles(string DirectoryName)
        {
            try
            {
                DirectoryInfo dir = new DirectoryInfo(DirectoryName);
                FileInfo[] files = dir.GetFiles("*.pdf",SearchOption.AllDirectories);
                return files.ToList<FileInfo>();
            }
            catch
            {

                return null;
            }
        }

        /// <summary>
        /// 获取用户选择的文件夹
        /// </summary>
        /// <returns></returns>
        public static string ChooseDirectory()
        {
            WinForm.FolderBrowserDialog fb = new WinForm.FolderBrowserDialog();
            if (fb.ShowDialog() == WinForm.DialogResult.OK)
                return fb.SelectedPath;
            else
                return "";
        }
    }
}
