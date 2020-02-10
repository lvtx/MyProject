using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSelectorDemo.ViewModels
{
   public class BreadCrumbViewModel:CodeBase.NotifyObject
    {
        private object m_AttachedDataContext;
        public BreadCrumbViewModel(object attachedDataContext)
        {
            m_AttachedDataContext = attachedDataContext;
        }


    }
}
