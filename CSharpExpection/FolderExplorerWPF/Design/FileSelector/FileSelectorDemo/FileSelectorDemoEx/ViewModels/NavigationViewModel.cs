using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FileSelectorDemo.ViewModels
{
   public class NavigationViewModel:CodeBase.NotifyObject
    {
        private object m_AttachedDataContext = null;
        public NavigationViewModel(object attachedDataContext)
        {
            m_AttachedDataContext = attachedDataContext;
            if (null != m_AttachedDataContext)
            {
                (m_AttachedDataContext as ViewModels.FileListViewModel).m_NavigationStateAction = RaiseNavigationButtonState;
            }
            GoBack = new CodeBase.DelegateCommand(DoGoBack);
            GoForward = new CodeBase.DelegateCommand(DoGoForward);
            GoUp = new CodeBase.DelegateCommand(DoGoUp);
            ShowDirectoryHistory= new CodeBase.DelegateCommand(DoShowDirectoryHistory);
        }

        private void RaiseNavigationButtonState()
        {
            IsGoBackEnable = CanGoBack();
            IsGoForwardEnable = CanGoForward();
            IsGoUpEnable = CanGoUp();
            IsShowDirectoryEnable = CanShowDirectoryHistory();
        }

        #region 命令
        /// <summary>
        /// 后退
        /// </summary>
        private ICommand _GoBack;
        public ICommand GoBack
        {
            get { return _GoBack; }
            set
            {
                if(value!= _GoBack)
                {
                    _GoBack = value;
                    OnPropertyChanged("GoBack");
                }               
            }
        }

        private void DoGoBack()
        {
            if (null != m_AttachedDataContext && m_AttachedDataContext is ViewModels.FileListViewModel)
            {
                var attachedViewModel = m_AttachedDataContext as ViewModels.FileListViewModel;
                int currentIndex = attachedViewModel.DirectoryHistory.IndexOf(attachedViewModel.CurrentModel);
                if (currentIndex < attachedViewModel.DirectoryHistory.Count-1)
                {                  
                    attachedViewModel.SwitchDirectory.Execute(attachedViewModel.DirectoryHistory[currentIndex +1]);
                }
            }
        }

        private bool CanGoBack()
        {
            if (null != m_AttachedDataContext && m_AttachedDataContext is ViewModels.FileListViewModel)
            {
                var attachedViewModel = m_AttachedDataContext as ViewModels.FileListViewModel;
                int currentIndex = attachedViewModel.DirectoryHistory.IndexOf(attachedViewModel.CurrentModel);
                if (currentIndex < attachedViewModel.DirectoryHistory.Count - 1)
                {
                    return true;
                }                
            }
            return false;
        }

        /// <summary>
        /// 绑定到GoBack按钮的IsEnable属性上
        /// </summary>
        private bool _IsGoBackEnable = false;
        public bool IsGoBackEnable
        {
            get { return _IsGoBackEnable; }
            set
            {
                if (value != _IsGoBackEnable)
                {
                    _IsGoBackEnable = value;
                    OnPropertyChanged("IsGoBackEnable");
                }
            }
        }

        /// <summary>
        /// 前进
        /// </summary>
        private ICommand _GoForward;
        public ICommand GoForward
        {
            get { return _GoForward; }
            set
            {
                if (value != _GoForward)
                {
                    _GoForward = value;
                    OnPropertyChanged("GoForward");
                }
            }
        }

        private void DoGoForward()
        {
            if (null != m_AttachedDataContext && m_AttachedDataContext is ViewModels.FileListViewModel)
            {
                var attachedViewModel = m_AttachedDataContext as ViewModels.FileListViewModel;
                int currentIndex = attachedViewModel.DirectoryHistory.IndexOf(attachedViewModel.CurrentModel);
                if (currentIndex >=1)
                {
                    attachedViewModel.SwitchDirectory.Execute(attachedViewModel.DirectoryHistory[currentIndex-1]);
                }
            }
        }

        private bool CanGoForward()
        {
            if (null != m_AttachedDataContext && m_AttachedDataContext is ViewModels.FileListViewModel)
            {
                var attachedViewModel = m_AttachedDataContext as ViewModels.FileListViewModel;
                int currentIndex = attachedViewModel.DirectoryHistory.IndexOf(attachedViewModel.CurrentModel);
                if (currentIndex >= 1)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 绑定到GoForward按钮的IsEnable属性上
        /// </summary>
        private bool _IsGoForwardEnable = false;
        public bool IsGoForwardEnable
        {
            get { return _IsGoForwardEnable; }
            set
            {
                if (value != _IsGoForwardEnable)
                {
                    _IsGoForwardEnable = value;
                    OnPropertyChanged("IsGoForwardEnable");
                }
            }
        }

        /// <summary>
        /// 向上
        /// </summary>
        private ICommand _GoUp;
        public ICommand GoUp
        {
            get { return _GoUp; }
            set
            {
                if (value != _GoUp)
                {
                    _GoUp = value;
                    OnPropertyChanged("GoUp");
                }
            }
        }

        private void DoGoUp()
        {
            if (null != m_AttachedDataContext && m_AttachedDataContext is ViewModels.FileListViewModel)
            {
                var attachedViewModel = m_AttachedDataContext as ViewModels.FileListViewModel;
                attachedViewModel.SwitchToParentDirectory.Execute(null);                
            }
        }

        private bool CanGoUp()
        {
            if (null != m_AttachedDataContext && m_AttachedDataContext is ViewModels.FileListViewModel)
            {
                var attachedViewModel = m_AttachedDataContext as ViewModels.FileListViewModel;
                var ret = Utils.DirectoryUtil.GetParentDirectory(attachedViewModel.CurrentModel.CurrentDirectory);
                if (null != ret)
                {
                    return true;
                }
            }
            return false;              
        }

        /// <summary>
        /// 绑定到GoUp按钮的IsEnable属性上
        /// </summary>
        private bool _IsGoUpEnable = false;
        public bool IsGoUpEnable
        {
            get { return _IsGoUpEnable; }
            set
            {
                if (value != _IsGoUpEnable)
                {
                    _IsGoUpEnable = value;
                    OnPropertyChanged("IsGoUpEnable");
                }
            }
        }

        /// <summary>
        /// 显示历史访问信息
        /// </summary>
        private ICommand _ShowDirectoryHistory;
        public ICommand ShowDirectoryHistory
        {
            get { return _ShowDirectoryHistory; }
            set
            {
                if (value != _ShowDirectoryHistory)
                {
                    _ShowDirectoryHistory = value;
                    OnPropertyChanged("ShowDirectoryHistory");
                }
            }
        }

        private void DoShowDirectoryHistory()
        {
            
        }

        private bool CanShowDirectoryHistory()
        {
            if (null != m_AttachedDataContext && m_AttachedDataContext is ViewModels.FileListViewModel)
            {
                var attachedViewModel = m_AttachedDataContext as ViewModels.FileListViewModel;               
                if (attachedViewModel.DirectoryHistory.Count>0)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 绑定到ShowDirectory按钮的IsEnable属性上
        /// </summary>
        private bool _IsShowDirectoryEnable = false;
        public bool IsShowDirectoryEnable
        {
            get { return _IsShowDirectoryEnable; }
            set
            {
                if (value != _IsShowDirectoryEnable)
                {
                    _IsShowDirectoryEnable = value;
                    OnPropertyChanged("IsShowDirectoryEnable");
                }
            }
        }
        #endregion       

    }
}
