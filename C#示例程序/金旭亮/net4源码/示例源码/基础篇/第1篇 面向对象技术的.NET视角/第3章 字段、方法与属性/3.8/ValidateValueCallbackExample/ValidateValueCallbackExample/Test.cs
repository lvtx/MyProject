using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace ValidateValueCallbackExample
{
    public class Test : DependencyObject
    {
        public int MyIntProperty
        {
            get { return (int)GetValue(MyIntPropertyProperty); }
            set { SetValue(MyIntPropertyProperty, value); }
        }
        public static readonly DependencyProperty MyIntPropertyProperty =
            DependencyProperty.Register(
            "MyIntProperty", //依赖属性名
            typeof(int),      //依赖属性类型
            typeof(Test),	//依赖属性的“所有者”
            new PropertyMetadata(), //依赖属性元数据
                new ValidateValueCallback(IsValid)  //值验证的回调函数
        );

        public static bool IsValid(object value)
        {
            return (int)value >= 0;
        }
    }

}
