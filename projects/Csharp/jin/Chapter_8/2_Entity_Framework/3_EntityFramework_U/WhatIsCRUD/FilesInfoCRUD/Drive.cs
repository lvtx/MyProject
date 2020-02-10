using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilesInfoCRUD
{
    public class Drive
    {
        /// <summary>
        /// 获取所有磁盘驱动器
        /// </summary>
        private DriveInfo[] drives;
        public DriveInfo[] Drives
        {
            get
            {
                drives = DriveInfo.GetDrives();
                return drives;
            }
        }
    }
}
