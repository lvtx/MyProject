using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Runtime.Serialization;


namespace UseClipboard
{
    [Serializable]
    class MyPic
    {
        public Image pic;       //图片
        public string picInfo;  //图片信息说明

    }
}
