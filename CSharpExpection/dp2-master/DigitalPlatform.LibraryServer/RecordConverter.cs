﻿using System;
using System.Collections.Generic;
using System.Text;

namespace DigitalPlatform.LibraryServer
{
    /// <summary>
    /// dp2library中调用C#脚本时, 用于转换一般记录信息xml->html的脚本类的基类
    /// </summary>
    public class RecordConverter
    {
        public LibraryApplication App = null;

        public string RecPath = ""; // 2009/10/18

        public RecordConverter()
        {

        }

        public virtual string Convert(string strXml)
        {

            return strXml;
        }
    }
}
