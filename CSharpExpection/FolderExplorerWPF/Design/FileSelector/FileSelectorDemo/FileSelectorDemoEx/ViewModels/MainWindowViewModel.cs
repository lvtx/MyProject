using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSelectorDemo.ViewModels
{
   public class MainWindowViewModel:CodeBase.NotifyObject
    {
        #region ViewModels

        private ViewModels.FileListViewModel _FileListViewModelInstance = new FileListViewModel();
        public ViewModels.FileListViewModel FileListViewModelInstance
        {
            get { return _FileListViewModelInstance; }
            set
            {
                if (value != _FileListViewModelInstance)
                {
                    _FileListViewModelInstance = value;
                    OnPropertyChanged("FileListViewModelInstance");
                }
            }
        }       

        #endregion
    }
}
