using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace FileSelectorDemo.ViewModels
{
   public class FileListViewModel:CodeBase.NotifyObject
    {
        //更新导航栏的IsEnable的状态
        public Action m_NavigationStateAction = null;
        public FileListViewModel()
        {            
            Init();
            OpenCurrentDirectory = new CodeBase.DelegateCommand<object>(DoOpenCurrentDirectory);
            SwitchDirectory= new CodeBase.DelegateCommand<object>(DoSwitchDirectory);
            SwitchToParentDirectory= new CodeBase.DelegateCommand(DoSwitchToParentDirectory);
            SelectCurrentFileListItem = new CodeBase.DelegateCommand<object>(DoSelectCurrentFileListItem);
        }        

        private void Init()
        {          
            var items= Utils.DirectoryUtil.InitCurrentFileList();
            if (items.Count > 0)
            {
                items.ForEach((item)=>
                {
                    CurrentFileList.Add(item);                    
                });
            }
            InitCurrentModelDetail();
        }

        private ObservableCollection<Models.FileListItemModel> _CurrentFileList
            =new ObservableCollection<Models.FileListItemModel>();
        public ObservableCollection<Models.FileListItemModel> CurrentFileList
        {
            get { return _CurrentFileList; }
            set
            {
                if(value!= _CurrentFileList)
                {
                    _CurrentFileList = value;
                    OnPropertyChanged("CurrentFileList");
                }                
            }
        }

        /// <summary>
        /// 当前的FileList对应的Model
        /// </summary>
        private Models.FileListItemModel _CurrentModel=new Models.FileListItemModel() {CurrentDirectory="此电脑" };
        public Models.FileListItemModel CurrentModel
        {
            get { return _CurrentModel; }
            set
            {
                if(value!= _CurrentModel)
                {
                    _CurrentModel = value;
                    OnPropertyChanged("CurrentModel");
                    InitCurrentModelDetail();
                }                
            }
        }
        
        private ObservableCollection<Models.FileListItemModel> _CurrentModelDetail = new ObservableCollection<Models.FileListItemModel>();
        public ObservableCollection<Models.FileListItemModel> CurrentModelDetail
        {
            get { return _CurrentModelDetail; }
            set
            {
                if (value != _CurrentModelDetail)
                {
                    _CurrentModelDetail = value;
                    OnPropertyChanged("CurrentModelDetail");                    
                }
            }
        }

        private ObservableCollection<Models.FileListItemModel> _CurrentModelChildren = new ObservableCollection<Models.FileListItemModel>();
        public ObservableCollection<Models.FileListItemModel> CurrentModelChildren
        {
            get { return _CurrentModelChildren; }
            set
            {
                if (value != _CurrentModelChildren)
                {
                    _CurrentModelChildren = value;
                    OnPropertyChanged("CurrentModelChildren");
                }
            }
        }

        private void InitCurrentModelDetail()
        {
            try
            {
                if (null != CurrentModel)
                {
                    string currentDirectory = CurrentModel.CurrentDirectory;
                    string[] detailList = CurrentModel.CurrentDirectory.Split(new char[] { '/', '\\' }, StringSplitOptions.RemoveEmptyEntries);
                    if (null != detailList && detailList.Length > 0)
                    {
                        if (CurrentModelDetail.Count > 0)
                        {
                            CurrentModelDetail.Clear();
                        }
                        for (int index = 0; index < detailList.Length; index++)
                        {
                            string name = detailList[index];
                            string currentItemDirectory = string.Empty;
                            StringBuilder sb = new StringBuilder();
                            for (int i = 0; i <= index; i++)
                            {
                                sb.Append(detailList[i] + "\\");
                            }
                            currentItemDirectory = sb.ToString();
                            if (!currentItemDirectory.EndsWith("\\"))
                            {
                                currentItemDirectory += "\\";
                            }

                            Models.FileListItemModel tempModel = new Models.FileListItemModel();
                            tempModel.Name = name;                           
                            tempModel.Icon = Utils.DirectoryUtil.GetDefaultFolderIcon();
                            tempModel.CurrentDirectory = currentItemDirectory;
                            var items = Utils.DirectoryUtil.GetCurrentFileListEx(currentItemDirectory);
                            if (items.Count > 0)
                            {
                                items.ForEach((item) =>
                                {
                                    tempModel.Children.Add(item);
                                });
                            }
                            CurrentModelDetail.Add(tempModel);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ;
            }           
        }

        //最近的十条访问记录
        private  ObservableCollection<Models.FileListItemModel> _DirectoryHistory=new ObservableCollection<Models.FileListItemModel>();
        public ObservableCollection<Models.FileListItemModel> DirectoryHistory
        {
            get { return _DirectoryHistory; }
            set
            {
                if(value!= _DirectoryHistory)
                {
                    _DirectoryHistory = value;
                    OnPropertyChanged("DirectoryHistory");
                }                
            }
        }

        /// <summary>
        /// 是否显示历史记录的Popup
        /// </summary>
        private bool _IsDirectoryHistoryShow=false;
        public bool IsDirectoryHistoryShow
        {
            get { return _IsDirectoryHistoryShow; }
            set
            {
                if(value!= _IsDirectoryHistoryShow)
                {
                    _IsDirectoryHistoryShow = value;
                    OnPropertyChanged("IsDirectoryHistoryShow");
                }
               
            }
        }

        /// <summary>
        /// 左侧的DataGrid的状态栏是否处于全部选中状态
        /// </summary>
        private bool _IsStateCheckAll = false;
        public bool IsStateCheckAll
        {
            get { return _IsStateCheckAll; }
            set
            {
                if (value != _IsStateCheckAll)
                {
                    _IsStateCheckAll = value;
                    OnPropertyChanged("IsStateCheckAll");
                    InitCurrentFileListIsSelectedState(value);
                    OnPropertyChanged("CurrentFileList");
                }
            }
        }

        private void InitCurrentFileListIsSelectedState(bool isChecked)
        {
            CurrentFileList.ToList().ForEach((item) =>
            {
                if (item.CurrentType == Defines.CurrentType.文件)
                {
                    item.IsSelected = isChecked;
                }
            });
        }
        #region 命令
        /// <summary>
        /// 打开当前的路径(这个指的是通过双击DataGrid中的DataGridRow来打开当前子目录的命令)
        /// </summary>
        private ICommand _OpenCurrentDirectory;
        public ICommand OpenCurrentDirectory
        {
            get { return _OpenCurrentDirectory; }
            set
            {
                if (value != _OpenCurrentDirectory)
                {
                    _OpenCurrentDirectory = value;
                    OnPropertyChanged("OpenCurrentDirectory");
                }                
            }
        }
        private void DoOpenCurrentDirectory(object obj)
        {
            Models.FileListItemModel current = obj as Models.FileListItemModel;
            if (null != current)
            {
                //如果双击的是文件则不做任何处理
                if (current.CurrentType == Defines.CurrentType.文件
                    ||string.IsNullOrEmpty(current.CurrentDirectory))
                {
                    return;
                }
                CurrentModel = current;

                if (CurrentFileList.Count > 0)
                {
                    CurrentFileList.Clear();
                }
                var items = Utils.DirectoryUtil.GetCurrentFileList(CurrentModel.CurrentDirectory);
                if (items.Count > 0)
                {                   
                    items.ForEach((item) =>
                    {
                        CurrentFileList.Add(item);
                    });
                    RecordHistoryDirectory(current);
                }
                //更新导航按钮的状态
                m_NavigationStateAction?.Invoke();               
            }
        }

        private ICommand _SwitchtDirectory;
        public ICommand SwitchDirectory
        {
            get { return _SwitchtDirectory; }
            set
            {
                if (value != _SwitchtDirectory)
                {
                    _SwitchtDirectory = value;
                    OnPropertyChanged("SwitchDirectory");
                }
            }
        }

        private void DoSwitchDirectory(object obj)
        {
            Models.FileListItemModel current = obj as Models.FileListItemModel;
            if (null != current)
            {
                CurrentModel = current;
                var items = Utils.DirectoryUtil.GetCurrentFileList(CurrentModel.CurrentDirectory);
                if (items.Count > 0)
                {
                    if (CurrentFileList.Count > 0)
                    {
                        CurrentFileList.Clear();
                    }
                    items.ForEach((item) =>
                    {
                        CurrentFileList.Add(item);
                    });
                }
                ChangeHistoryDirectoryItemDirction(CurrentModel);
            }
            //不再显示路径的历史记录(Popup关闭)
            IsDirectoryHistoryShow = false;
            //更新导航按钮的状态
            m_NavigationStateAction?.Invoke();
        }

        /// <summary>
        /// 查找当前路径的父类的内容
        /// </summary>
        private ICommand _SwitchToParentDirectory;
        public ICommand SwitchToParentDirectory
        {
            get { return _SwitchToParentDirectory; }
            set
            {
                if(value!= _SwitchToParentDirectory)
                {
                    _SwitchToParentDirectory = value;
                    OnPropertyChanged("SwitchToParentDirectory");
                }                
            }
        }

        private void DoSwitchToParentDirectory()
        {
            if (null == CurrentModel) return;    
            var items = Utils.DirectoryUtil.GetParentFileList(CurrentModel.CurrentDirectory);
            if (items.Count > 0)
            {
                if (CurrentFileList.Count > 0)
                {
                    CurrentFileList.Clear();
                }
                items.ForEach((item) =>
                {
                    CurrentFileList.Add(item);
                });
            }

            var ret = Utils.DirectoryUtil.GetParentDirectory(CurrentModel.CurrentDirectory);
            if(null!=ret)
            {
                ///添加到历史列表
                Models.FileListItemModel tempModel = new Models.FileListItemModel()
                {
                    Name = ret.Name,
                    Icon = ret.Icon,
                    CurrentDirectory = ret.CurrentDirectory,
                };
                RecordHistoryDirectory(tempModel);

                ///更新CurrentModel状态
                CurrentModel = tempModel;
            }
            //更新导航按钮的状态
            m_NavigationStateAction?.Invoke();
        }

        /// <summary>
        /// 选中当前左侧的文件列表中的某一项
        /// </summary>
        private ICommand _SelectCurrentFileListItem;
        public ICommand SelectCurrentFileListItem
        {
            get { return _SelectCurrentFileListItem; }
            set
            {
                if (value != _SelectCurrentFileListItem)
                {
                    _SelectCurrentFileListItem = value;
                    OnPropertyChanged("SelectCurrentFileListItem");
                }
            }
        }

        private void DoSelectCurrentFileListItem(object selectedItems)
        {
            if (null != selectedItems && selectedItems is IList)
            {
                IList list = selectedItems as IList;
                CurrentFileList.ToList().ForEach((item) =>
                {
                    item.IsSelected = false;
                });
                foreach (var current in list)
                {
                    var item = current as Models.FileListItemModel;
                    if (item.CurrentType == Defines.CurrentType.文件)
                    {
                        (current as Models.FileListItemModel).IsSelected = true;
                    }
                }
                OnPropertyChanged("CurrentFileList");
            }
        }
        #endregion
        private void RecordHistoryDirectory(Models.FileListItemModel current)
        {
            //状态初始化
            foreach (var record in DirectoryHistory)
            {
                record.CurrentDirection = Defines.Direction.向后;
            }

            //每次添加后都加入到历史记录中
            if (DirectoryHistory.Contains(current))
            {
                DirectoryHistory.Insert(0, new Models.FileListItemModel()
                {
                    Name = current.Name,
                    CurrentDirectory = current.CurrentDirectory,
                    Icon = current.Icon,
                    CurrentDirection = Defines.Direction.选中
                });
            }
            else
            {
                current.CurrentDirection = Defines.Direction.选中;
                DirectoryHistory.Insert(0, current);
            }
            KeepTenHistoryRecords();
        }

        /// <summary>
        /// 保持历史记录中不超过10条记录
        /// </summary>
        private void KeepTenHistoryRecords()
        {
            if (DirectoryHistory.Count > 10)
            {           
                for (int i = 0; i < DirectoryHistory.Count; i++)
                {
                    if(i>9)
                    {
                        DirectoryHistory.RemoveAt(i);
                        i--;
                    }                                   
                }   
            }
        }

        private void ChangeHistoryDirectoryItemDirction(Models.FileListItemModel model)
        {
            if(null!=model&&DirectoryHistory.Count>0)
            {
                int index = DirectoryHistory.IndexOf(model);
                for (int i = 0; i < DirectoryHistory.Count; i++)
                {
                    var item = DirectoryHistory[i];
                    if (i < index)
                    {
                        item.CurrentDirection = Defines.Direction.向后;
                    }
                    else if (i == index)
                    {
                        item.CurrentDirection = Defines.Direction.选中;
                    }
                    else if (i > index)
                    {
                        item.CurrentDirection = Defines.Direction.向前;
                    }
                }
            }
        }     

    }
}
