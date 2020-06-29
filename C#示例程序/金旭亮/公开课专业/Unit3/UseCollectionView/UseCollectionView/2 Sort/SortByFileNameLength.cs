using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.IO;

namespace UseCollectionView
{
    /// <summary>
    /// 自定义排序规则：按文件名的长度排序
    /// </summary>
    class SortByFileNameLength:IComparer
    {
        public int Compare(object x, object y)
        {
            FileInfo file1 = x as FileInfo;
            FileInfo file2 = y as FileInfo;
            if (file1 == null || file2 == null)
                throw new InvalidCastException("请传入两个FileInfo对象");
            return file1.Name.Length.CompareTo(file2.Name.Length);
        }

        
    }
}
