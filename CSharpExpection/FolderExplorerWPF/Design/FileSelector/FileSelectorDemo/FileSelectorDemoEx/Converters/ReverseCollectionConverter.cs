using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace FileSelectorDemo.Converters
{
    public class ReverseCollectionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ObservableCollection<Models.FileListItemModel>)
            {
                var reverseCollection= value as ObservableCollection<Models.FileListItemModel>;
                return reverseCollection.Reverse();
            }
            return new ObservableCollection<Models.FileListItemModel>();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
