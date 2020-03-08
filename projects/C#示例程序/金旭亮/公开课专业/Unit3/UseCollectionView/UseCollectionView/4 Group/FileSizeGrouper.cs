using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace UseCollectionView
{
    /// <summary>
    /// 按文件大小进行分组
    /// </summary>
    class FileSizeGrouper:IValueConverter
    {

      /// <summary>
      /// 按文件大小分类
      /// </summary>
      /// <param name="value"></param>
      /// <param name="targetType"></param>
      /// <param name="parameter"></param>
      /// <param name="culture"></param>
      /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            long filesize = (long)value;
            int K = 1024;
            int M = 1024 * 1024;
            int G = 1024 * 1024 * 1024;
            if (filesize > 0 && filesize < K)
                return "1K以下";
            if (filesize >= K && filesize < M)
                return "1K~1M之间";
            if (filesize >= M && filesize < G)
                return "1M~1G之间";
            return "大于1G";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

       
    }
}
