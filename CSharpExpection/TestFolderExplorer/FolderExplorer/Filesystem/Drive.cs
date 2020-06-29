using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace FolderExplorer.Filesystem
{
    class Drive
    {
        /// <summary>
        /// 获取磁盘名
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public string GetDriveName(DriveInfo d)
        {
            string driveName = d.Name;
            DriveInfo drive = d;
            switch (drive.DriveType)
            {

                //固定磁盘
                case DriveType.Fixed:

                    //显示的名称
                    if (drive.VolumeLabel == "新加卷")
                    {
                        driveName = string.Format("新加卷 (" + drive.Name.Split('\\')[0] + ")");
                    }
                    else
                        driveName = string.Format("本地磁盘 (" + drive.Name.Split('\\')[0] + ")");
                    break;

                //光驱
                case DriveType.CDRom:

                    //显示的名称
                    driveName = string.Format("光驱 (" + drive.Name.Split('\\')[0] + ")");
                    break;

                //可移动磁盘
                case DriveType.Removable:

                    //显示的名称
                    driveName = string.Format("可移动磁盘 (" + drive.Name.Split('\\')[0] + ")");
                    break;
            }
            return driveName;
        }

        /// <summary>
        /// 获取磁盘名另一种实现方式
        /// </summary>
        //public string GetDriveName(DriveInfo d)
        //{
        //    SelectQuery sq = new SelectQuery("select * from win32_logicaldisk");
        //    System.Management.ManagementObjectSearcher mos = new ManagementObjectSearcher(sq);
        //    string driveName = d.Name;
        //    DriveInfo drive = d;
        //    var diskList = mos.Get();
        //    //Name表示设备的名称
        //    //各属性的标识见联机的MSDN里，Win32 and COM Development下的WMI。
        //    //如http://msdn.microsoft.com/en-us/library/aa394173(VS.85).aspx
        //    foreach (System.Management.ManagementObject disk in mos.Get())
        //    {
        //        if (disk["Name"].ToString() == drive.Name.Split('\\')[0])
        //        {
        //            try
        //            {
        //                string strType = disk["DriveType"].ToString();
        //                switch (strType) //类型 
        //                {
        //                    case "0":
        //                        driveName = string.Format("未知设备 (" + drive.Name.Split('\\')[0] + ")");
        //                        break;
        //                    case "1":
        //                        driveName = string.Format("未分区 (" + drive.Name.Split('\\')[0] + ")");
        //                        break;
        //                    case "2":
        //                        driveName = string.Format("可移动磁盘 (" + drive.Name.Split('\\')[0] + ")");
        //                        break;
        //                    case "3":
        //                        if(disk["VolumeName"].ToString() != "新加卷")
        //                        driveName = string.Format("本地磁盘 (" + drive.Name.Split('\\')[0] + ")");
        //                        else
        //                        driveName = string.Format(disk["VolumeName"] + " (" + drive.Name.Split('\\')[0] + ")");
        //                        break;
        //                    case "4":
        //                        driveName = string.Format("网络驱动器 (" + drive.Name.Split('\\')[0] + ")");
        //                        break;
        //                    case "5":
        //                        driveName = string.Format("光驱 (" + drive.Name.Split('\\')[0] + ")");
        //                        break;
        //                    case "6":
        //                        driveName = string.Format("内存磁盘 (" + drive.Name.Split('\\')[0] + ")");
        //                        break;
        //                }
        //            }
        //            catch
        //            {
        //            }
        //        }
        //    }
        //    return driveName;
        //}

    }
}
