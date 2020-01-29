using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFileSearcherForWPF.Search
{
    /// <summary>
    /// 文件搜索信息对象
    /// </summary>
    public class SearchInfo
    {

        /// <summary>
        /// 要搜索的文件
        /// </summary>
        private String searchFile;

        public String SearchFile
        {
            get { return searchFile; }
            set
            {
                searchFile = value;
            }
        }

        /// <summary>
        /// 起始文件夹
        /// </summary>
        private string beginDirectory;

        public string BeginDirectory
        {
            get { return beginDirectory; }
            set 
            { 
                beginDirectory = value;
            }
        }
    }
}

