using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace DownLoadPageFromWeb
{
    public class DownLoadTask
    {
        //用于下载文件的WebRequest对象
        public WebRequest WebRequestObject { get; set; }
        //用于保存下载文件的文件名
        public string SaveToFileName { get; set; }
    }
}
