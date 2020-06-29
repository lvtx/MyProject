using System;
using System.Collections.Generic;
using System.Management;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject1
{
    [TestClass]
    public class TestDrive
    {
        [TestMethod]
        private void ShowDriveDetail()
        {
            SelectQuery sq = new SelectQuery("select * from win32_logicaldisk");
            System.Management.ManagementObjectSearcher mos = new ManagementObjectSearcher(sq);
            List<string> diskList = new List<string>();
            foreach (System.Management.ManagementObject disk in mos.Get())
            {
                diskList.Add(disk["Name"].ToString());
                //Name表示设备的名称
                //各属性的标识见联机的MSDN里，Win32 and COM Development下的WMI。
                //如http://msdn.microsoft.com/en-us/library/aa394173(VS.85).aspx
                try
                {
                    string strType = disk["DriveType"].ToString();
                    switch (strType) //类型 
                    {
                        case "0":
                            diskList.Add("未知设备");
                            break;
                        case "1":
                            diskList.Add("未分区");
                            break;
                        case "2":
                            diskList.Add("可移动磁盘");
                            break;
                        case "3":
                            diskList.Add("硬盘");
                            break;
                        case "4":
                            diskList.Add("网络驱动器");
                            break;
                        case "5":
                            diskList.Add("光驱");
                            break;
                        case "6":
                            diskList.Add("内存磁盘");
                            break;
                    }
                }
                catch
                {
                    diskList.Add("设备未准备好");
                }
                try
                {
                    diskList.Add(GetSizeUseUnit(disk["Size"].ToString()));
                    //未用GetSizeUseUnit函数处理的Size属性以字节为单位 
                }
                catch
                {
                }
                try //可移动设备如光驱在未插入光盘时处于不可用状态，需要捕捉异常。 
                {
                    diskList.Add(GetSizeUseUnit(disk["FreeSpace"].ToString()));
                }
                catch
                {
                }
                try
                {
                    diskList.Add(disk["VolumeSerialNumber"].ToString());
                }
                catch
                {
                }
            }
            foreach (var disk in diskList)
            {
                Console.WriteLine(disk);
            }
        }
        [TestMethod]
        private string GetSizeUseUnit(string size)
        {
            double dSpace = Convert.ToDouble(size);
            string sSpace = dSpace.ToString("N");
            string[] tmp;
            string rtnSize = "0";
            tmp = sSpace.Split(',');
            switch (tmp.GetUpperBound(0))
            {
                case 0:
                    rtnSize = tmp[0] + " 字节";
                    break;
                case 1:
                    rtnSize = tmp[0] + "." + tmp[1].Substring(0, 2) + " K";
                    break;
                case 2:
                    rtnSize = tmp[0] + "." + tmp[1].Substring(0, 2) + " M";
                    break;
                case 3:
                    rtnSize = tmp[0] + "." + tmp[1].Substring(0, 2) + " G";
                    break;
                case 4:
                    rtnSize = tmp[0] + "." + tmp[1].Substring(0, 2) + " T";
                    break;
            }
            return rtnSize;
        }
    }
}

