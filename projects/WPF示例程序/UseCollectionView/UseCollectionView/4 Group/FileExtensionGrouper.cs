using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.IO;

namespace UseCollectionView
{
    /// <summary>
    /// 按文件扩展名进行分组
    /// </summary>
    class FileExtensionGrouper:IValueConverter
    {
       
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string FileName = value.ToString();
            string ext = Path.GetExtension(FileName).ToUpper();
            return "文件扩展名:" + ext;　　//作为分组依据的值
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

       
    }
}
