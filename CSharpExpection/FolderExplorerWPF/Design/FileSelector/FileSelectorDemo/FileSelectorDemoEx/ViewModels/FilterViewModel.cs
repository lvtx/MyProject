using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace FileSelectorDemo.ViewModels
{
   public class FilterViewModel:CodeBase.NotifyObject
    {
        /// <summary>
        /// 代表的是FileList关联的ViewModel对象
        /// </summary>
        private object m_AttachedDataContext;
        public FilterViewModel(object attachedDataContext)
        {
            m_AttachedDataContext = attachedDataContext;
        }


        #region 属性
        private object _SelectedItem;
        public object SelectedItem
        {
            get { return _SelectedItem; }
            set
            {
                if(value!= _SelectedItem)
                {
                    _SelectedItem = value;
                    OnPropertyChanged("SelectedItem");
                    DoFilterCurrentFileList((value as ComboBoxItem).Content.ToString());
                }
                
            }
        }
        #endregion

        private void DoFilterCurrentFileList(string filter)
        {
            if (null != m_AttachedDataContext && m_AttachedDataContext is ViewModels.FileListViewModel)
            {
                var attachedDataContext = m_AttachedDataContext as ViewModels.FileListViewModel;
                string[] toFilterFormat=filter.Split(new char[] {'(' ,'*', '|',')' },StringSplitOptions.RemoveEmptyEntries).Skip(1).ToArray();
                if (toFilterFormat != null && toFilterFormat.Length > 0)
                {
                    List<Models.FileListItemModel> filterCollection = new List<Models.FileListItemModel>();                   
                    attachedDataContext.CurrentFileList.ToList<Models.FileListItemModel>().ForEach((item)=>
                    {
                        toFilterFormat.ToList().ForEach((extension)=>
                        {
                            if (item.FileExtension.Equals(extension))
                            {
                                filterCollection.Add(item);
                            }
                        });
                    });

                }

             }
        }

    }
}
