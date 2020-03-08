using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.ComponentModel;

namespace UnifiedModelForCancellation
{
    public class ThreadObject : INotifyPropertyChanged
    {
        private int _value;
        public int Value
        {
            get { return _value; }
            set
            {
                _value = value;
                OnPropertyChanged("Value");
            }
        }

        private CancellationToken _token ;

        public ThreadObject(CancellationToken token)
        {
            _token = token;
        }

        public void DoWork()
        {
            while (_token.IsCancellationRequested!=true)
            {
                if (Value + 5 > 100)
                    Value = 0;
                else
                    Value += 5;

                Thread.Sleep(200);
            }
        }
        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
