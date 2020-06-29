using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace FileSelectorDemo.Utils
{
    public class SystemIconUtil
    {
        /// <summary>  
        /// 通过扩展名得到图标和描述  
        /// </summary>  
        /// <param name="ext">扩展名(如“.txt”)</param>  
        /// <param name="largeIcon">得到大图标</param>  
        /// <param name="smallIcon">得到小图标</param>  
        /// <param name="description">得到类型描述或者空字符串</param>  
        public static void GetExtsIconAndDescription(string ext, out Icon largeIcon, out Icon smallIcon, out string description)
        {
            try
            {
                GetDefaultIcon(out largeIcon, out smallIcon);   //得到缺省图标  
                description = "";                               //缺省类型描述  
                RegistryKey extsubkey = Registry.ClassesRoot.OpenSubKey(ext);   //从注册表中读取扩展名相应的子键  
                if (extsubkey == null) return;

                string extdefaultvalue = extsubkey.GetValue(null) as string;     //取出扩展名对应的文件类型名称  
                if (extdefaultvalue == null) return;

                if (extdefaultvalue.Equals("exefile", StringComparison.OrdinalIgnoreCase))  //扩展名类型是可执行文件  
                {
                    RegistryKey exefilesubkey = Registry.ClassesRoot.OpenSubKey(extdefaultvalue);  //从注册表中读取文件类型名称的相应子键  
                    if (exefilesubkey != null)
                    {
                        string exefiledescription = exefilesubkey.GetValue(null) as string;   //得到exefile描述字符串  
                        if (exefiledescription != null) description = exefiledescription;
                    }
                    System.IntPtr exefilePhiconLarge = new IntPtr();
                    System.IntPtr exefilePhiconSmall = new IntPtr();
                    ExtractIconExW(Path.Combine(Environment.SystemDirectory, "shell32.dll"), 2, ref exefilePhiconLarge, ref exefilePhiconSmall, 1);
                    if (exefilePhiconLarge.ToInt32() > 0) largeIcon = Icon.FromHandle(exefilePhiconLarge);
                    if (exefilePhiconSmall.ToInt32() > 0) smallIcon = Icon.FromHandle(exefilePhiconSmall);
                    return;
                }

                RegistryKey typesubkey = Registry.ClassesRoot.OpenSubKey(extdefaultvalue);  //从注册表中读取文件类型名称的相应子键  
                if (typesubkey == null) return;

                string typedescription = typesubkey.GetValue(null) as string;   //得到类型描述字符串  
                if (typedescription != null) description = typedescription;

                RegistryKey defaulticonsubkey = typesubkey.OpenSubKey("DefaultIcon");  //取默认图标子键  
                if (defaulticonsubkey == null) return;

                //得到图标来源字符串  
                string defaulticon = defaulticonsubkey.GetValue(null) as string; //取出默认图标来源字符串  
                if (defaulticon == null) return;
                string[] iconstringArray = defaulticon.Split(',');
                int nIconIndex = 0; //声明并初始化图标索引  
                if (iconstringArray.Length > 1)
                {
                    if (!int.TryParse(iconstringArray[1], out nIconIndex))
                    {
                        nIconIndex = 0;     //int.TryParse转换失败，返回0  
                    }                        
                }                  

                //得到图标  
                System.IntPtr phiconLarge = new IntPtr();
                System.IntPtr phiconSmall = new IntPtr();
                ExtractIconExW(iconstringArray[0].Trim('"'), nIconIndex, ref phiconLarge, ref phiconSmall, 1);
                if (phiconLarge.ToInt32() > 0) largeIcon = Icon.FromHandle(phiconLarge);
                if (phiconSmall.ToInt32() > 0) smallIcon = Icon.FromHandle(phiconSmall);
            }
            catch (Exception ex)
            {
                largeIcon = null;
                smallIcon = null;
                description = ex.Message;
            }            
        }

        /// <summary>  
        /// 获取缺省图标  
        /// </summary>  
        /// <param name="largeIcon"></param>  
        /// <param name="smallIcon"></param>  
        private static void GetDefaultIcon(out Icon largeIcon, out Icon smallIcon)
        {
            largeIcon = smallIcon = null;
            System.IntPtr phiconLarge = new IntPtr();
            System.IntPtr phiconSmall = new IntPtr();
            ExtractIconExW(Path.Combine(Environment.SystemDirectory, "shell32.dll"), 0, ref phiconLarge, ref phiconSmall, 1);
            if (phiconLarge.ToInt32() > 0) largeIcon = Icon.FromHandle(phiconLarge);
            if (phiconSmall.ToInt32() > 0) smallIcon = Icon.FromHandle(phiconSmall);
        } 

        /// Return Type: UINT->unsigned int
        ///lpszFile: LPCWSTR->WCHAR*
        ///nIconIndex: int
        ///phiconLarge: HICON*
        ///phiconSmall: HICON*
        ///nIcons: UINT->unsigned int
        [System.Runtime.InteropServices.DllImportAttribute("shell32.dll", EntryPoint = "ExtractIconExW", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        private static extern uint ExtractIconExW([System.Runtime.InteropServices.InAttribute()] [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.LPWStr)] string lpszFile, int nIconIndex, ref System.IntPtr phiconLarge, ref System.IntPtr phiconSmall, uint nIcons);

        /// <summary>  
        /// 获取文件夹图标
        /// </summary>  
        /// <returns>图标</returns>  
        public static Icon GetDirectoryIcon()
        {
            try
            {
                SHFILEINFO _SHFILEINFO = new SHFILEINFO();
                IntPtr _IconIntPtr = SHGetFileInfo(@"", 0, ref _SHFILEINFO, (uint)Marshal.SizeOf(_SHFILEINFO), (uint)(SHGFI.SHGFI_ICON | SHGFI.SHGFI_LARGEICON));
                if (_IconIntPtr.Equals(IntPtr.Zero)) return null;
                Icon _Icon = System.Drawing.Icon.FromHandle(_SHFILEINFO.hIcon);
                return _Icon;
            }
            catch (Exception ex)
            {
                
            }
            return null;         
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct SHFILEINFO
        {
            public IntPtr hIcon;
            public IntPtr iIcon;
            public uint dwAttributes;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string szDisplayName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
            public string szTypeName;
        }

        /// <summary>  
        /// 返回系统设置的图标  
        /// </summary>  
        /// <param name="pszPath">文件路径 如果为""  返回文件夹的</param>  
        /// <param name="dwFileAttributes">0</param>  
        /// <param name="psfi">结构体</param>  
        /// <param name="cbSizeFileInfo">结构体大小</param>  
        /// <param name="uFlags">枚举类型</param>  
        /// <returns>-1失败</returns>  
        [DllImport("shell32.dll")]
        private static extern IntPtr SHGetFileInfo(string pszPath, uint dwFileAttributes, ref SHFILEINFO psfi, uint cbSizeFileInfo, uint uFlags);

        private enum SHGFI
        {
            SHGFI_ICON = 0x100,
            SHGFI_LARGEICON = 0x0,
            SHGFI_USEFILEATTRIBUTES = 0x10
        }

        ///// <summary>  
        ///// 获取文件图标
        ///// </summary>  
        ///// <param name="p_Path">文件全路径</param>  
        ///// <returns>图标</returns>  
        //public static Icon GetFileIcon(string p_Path)
        //{
        //    SHFILEINFO _SHFILEINFO = new SHFILEINFO();
        //    IntPtr _IconIntPtr = SHGetFileInfo(p_Path, 0, ref _SHFILEINFO, (uint)Marshal.SizeOf(_SHFILEINFO), (uint)(SHGFI.SHGFI_ICON | SHGFI.SHGFI_LARGEICON | SHGFI.SHGFI_USEFILEATTRIBUTES));
        //    if (_IconIntPtr.Equals(IntPtr.Zero)) return null;
        //    Icon _Icon = System.Drawing.Icon.FromHandle(_SHFILEINFO.hIcon);
        //    return _Icon;
        //}

        [DllImport("gdi32")]
        static extern int DeleteObject(IntPtr o);
        public static BitmapSource ConvertFromIcon(Icon icon)
        {
            try
            {
                if (null == icon) return null;
                Bitmap m_Bitmap = System.Drawing.Image.FromHbitmap(icon.ToBitmap().GetHbitmap());
                IntPtr ip = m_Bitmap.GetHbitmap();
                BitmapSource bitmapSource = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                    ip, IntPtr.Zero, Int32Rect.Empty,
                    System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());
                DeleteObject(ip);
                return bitmapSource;
            }
            catch
            {
                return null;
            }
        }
    }
}
