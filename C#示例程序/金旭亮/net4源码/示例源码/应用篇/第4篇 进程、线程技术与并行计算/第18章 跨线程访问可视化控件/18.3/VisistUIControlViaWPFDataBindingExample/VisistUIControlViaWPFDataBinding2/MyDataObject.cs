using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace VisistUIControlViaWPFDataBinding2
{
    public class MyDataObject : INotifyPropertyChanged
    {
        /// <summary>
        /// 定义一个“属性值更改”事件
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

       
        /// <summary>
        /// 负责触发“属性值更改”事件
        /// </summary>
        /// <param name="name"></param>
        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        private int _value;
        public int Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
                //给属性赋值时，触发“Value属性值更改”事件
                OnPropertyChanged("Value");
            }
        }
    }
}
