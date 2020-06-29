using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace FileSelectorDemo.Models
{
   public class NavigationHistoryModel:CodeBase.NotifyObject
    {
        private string _HistoryName;
        public string HistoryName
        {
            get { return _HistoryName; }
            set
            {
                if (value != _HistoryName)
                {
                    _HistoryName = value;
                    OnPropertyChanged("HistoryName");
                }
            }
        }

        private ImageSource _icon;
        public ImageSource Icon
        {
            get { return _icon; }
            set
            {
                if (_icon != value)
                {
                    _icon = value;
                    OnPropertyChanged("Icon");
                }
            }
        }
    }
}
