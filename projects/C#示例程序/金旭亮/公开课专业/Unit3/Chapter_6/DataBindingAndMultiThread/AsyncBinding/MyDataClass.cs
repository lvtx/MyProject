using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;

namespace AsyncBinding
{
    public class MyDataClass : INotifyPropertyChanged
    {
        private string _info;
        public string Information
        {
            get
            {

                Console.WriteLine("get方法的执行线程:{0}", Thread.CurrentThread.ManagedThreadId);
                return _info;


            }
            set
            {

                Console.WriteLine("set方法的执行线程:{0}", Thread.CurrentThread.ManagedThreadId);
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

        public MyDataClass()
        {
            PropertyChanged += MyDataClass_PropertyChanged;
        }

        void MyDataClass_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Console.WriteLine("MyDataClass中的PropertyChanged事件响应方法的执行线程:{0}", Thread.CurrentThread.ManagedThreadId);
        }
    }
}
