using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DelayBinding
{
    public class MyDataClass : INotifyPropertyChanged
    {
        private string _info;
        public string Information
        {
            get
            {
                
                return _info;
            }
            set
            {
              
                SetProperty(ref _info, value);
            }

        }
        protected void SetProperty<T>(ref T field, T value,
            [CallerMemberName] string propName = null)
        {
            if (!EqualityComparer<T>.Default.Equals(field, value))
            {
                field = value;
                var pc = PropertyChanged;
                if (pc != null)
                    pc(this, new PropertyChangedEventArgs(propName));
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

      
      
    }
}
