using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace FolderExplorer.Filesystem
{
    public class FileTypeInfo
    {

        [DllImport("shell32.dll")]
        private static extern IntPtr SHGetFileInfo(string pszPath, uint dwFileAttributes, ref SHFILEINFO psfi, uint cbSizeFileInfo, uint uFlags);

        [StructLayout(LayoutKind.Sequential)]
        private struct SHFILEINFO
        {
            public IntPtr hIcon;
            public int iIcon;
            public uint dwAttributes;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string szDisplayName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
            public string szTypeName;
        };

        private enum FILE_ATTRIBUTE
        {
            FILE_ATTRIBUTE_NORMAL = 0x80
        };

        private enum SHGFI
        {
            SHGFI_TYPENAME = 0x000000400,
            SHGFI_USEFILEATTRIBUTES = 0x000000010
        };

        public static string GetFileType(string fileName)
        {
            string filetype;
            SHFILEINFO info = new SHFILEINFO();
            uint dwFileAttributes = (uint)FILE_ATTRIBUTE.FILE_ATTRIBUTE_NORMAL;
            uint uFlags = (uint)(SHGFI.SHGFI_TYPENAME | SHGFI.SHGFI_USEFILEATTRIBUTES);
            SHGetFileInfo(fileName, dwFileAttributes, ref info, (uint)Marshal.SizeOf(info), uFlags);
            filetype = info.szTypeName;
            return filetype;
        }



        //class Program
        //{
        //    public static void Main(string[] args)
        //    {
        //        NativeMethods.SHFILEINFO info = new NativeMethods.SHFILEINFO();

        //        string fileName = @"C:\Some\Path\SomeFile.png";
        //        uint dwFileAttributes = NativeMethods.FILE_ATTRIBUTE.FILE_ATTRIBUTE_NORMAL;
        //        uint uFlags = (uint)(NativeMethods.SHGFI.SHGFI_TYPENAME | NativeMethods.SHGFI.SHGFI_USEFILEATTRIBUTES);

        //        NativeMethods.SHGetFileInfo(fileName, dwFileAttributes, ref info, (uint)Marshal.SizeOf(info), uFlags);

        //        Console.WriteLine(info.szTypeName);
        //    }
        //}
    }
}
