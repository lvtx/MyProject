using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace UseDependencyInDataBinding
{
    public class MyClass : DependencyObject
    {

        public int MyProperty
        {
            get { return (int)GetValue(MyPropertyProperty); }
            set { SetValue(MyPropertyProperty, value); }
        }

        public static readonly DependencyProperty MyPropertyProperty =
            DependencyProperty.Register("MyProperty", typeof(int), typeof(MyClass));




        

        
    }
}
