
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
namespace OneWayBinding
{
    public class MyData : INotifyPropertyChanged
    {
        private string name = "";
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged();
            }
        }
        private int _value = 0;
        public int Value
        {
            get { return _value; }
            set
            {
                _value = value;
                OnPropertyChanged();
            }
        }

        private DateTime? time=null;
        public DateTime? Time
        {
            get
            {
                return time;
            }
            set
            {
                time = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        //使用[CallerMemberName]，可以直接地获取调用此方法的方法或属性名，并将它传给caller这个参数
        private void OnPropertyChanged([CallerMemberName]string caller = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(caller));
            }
        }
    }
}
