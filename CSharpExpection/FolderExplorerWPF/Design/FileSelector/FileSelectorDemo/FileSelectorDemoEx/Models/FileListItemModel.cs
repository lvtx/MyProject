using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace FileSelectorDemo.Models
{
   public class FileListItemModel:CodeBase.NotifyObject
    {
        private string _Name;
        public string Name
        {
            get { return _Name; }
            set
            {
                if(value!= _Name)
                {
                    _Name = value;
                    OnPropertyChanged("Name");
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

        private string _CreateTime;
        public string CreateTime
        {
            get { return _CreateTime; }
            set
            {
                if (value != _CreateTime)
                {
                    _CreateTime = value;
                    OnPropertyChanged("CreateTime");
                }
            }
        }

        /// <summary>
        /// 当前文件的大小
        /// </summary>
        private string _Size;
        public string Size
        {
            get { return _Size; }
            set
            {
                if (value != _Size)
                {
                    _Size = value;
                    OnPropertyChanged("Size");
                }
            }
        }

        /// <summary>
        /// 当前类型
        /// </summary>
        private Defines.CurrentType _CurrentType;
        public Defines.CurrentType CurrentType
        {
            get { return _CurrentType; }
            set
            {
                if (value != _CurrentType)
                {
                    _CurrentType = value;
                    OnPropertyChanged("CurrentType");
                }
            }
        }

        /// <summary>
        /// 当前文件的拓展名(文件夹用.来表示)
        /// </summary>
        private string _FileExtension;
        public string FileExtension
        {
            get { return _FileExtension; }
            set
            {
                if (value != _FileExtension)
                {
                    _FileExtension = value;
                    OnPropertyChanged("FileExtension");
                }
            }
        }

        /// <summary>
        /// 当前项是否处于选中状态
        /// </summary>
        private bool _IsSelected = false;
        public bool IsSelected
        {
            get { return _IsSelected; }
            set
            {
                if (value != _IsSelected)
                {
                    _IsSelected = value;
                    OnPropertyChanged("IsSelected");
                }
            }
        }

        /// <summary>
        /// 当前项所处的状态，这个状态指的是在Navigation中，当前项
        /// 是可以向前还是可以向后或者正好被选中
        /// </summary>
        private Defines.Direction _CurrentDirection;
        public Defines.Direction CurrentDirection
        {
            get { return _CurrentDirection; }
            set
            {
                if (value != _CurrentDirection)
                {
                    _CurrentDirection = value;
                    OnPropertyChanged("CurrentDirection");
                }
            }
        }


        /// <summary>
        /// 当前文件的父节点的文件路径
        /// </summary>
        private string _ParentDirectory;
        public string ParentDirectory
        {
            get { return _ParentDirectory; }
            set
            {
                if (value != _ParentDirectory)
                {
                    _ParentDirectory = value;
                    OnPropertyChanged("ParentDirectory");
                }
            }
        }

        /// <summary>
        /// 当前文件的文件路径
        /// </summary>
        private string _CurrentDirectory;
        public string CurrentDirectory
        {
            get { return _CurrentDirectory; }
            set
            {
                if (value != _CurrentDirectory)
                {
                    _CurrentDirectory = value;
                    OnPropertyChanged("CurrentDirectory");
                }
            }
        }

        /// <summary>
        /// 当前项的子项
        /// </summary>
        private ObservableCollection<Models.FileListItemModel> _Children = new ObservableCollection<Models.FileListItemModel>();
        public ObservableCollection<Models.FileListItemModel> Children
        {
            get { return _Children; }
            set
            {
                if (value != _Children)
                {
                    _Children = value;
                    OnPropertyChanged("Children");
                }
            }
        }
    }
}
