using System;
using System.Collections.Generic;
using System.Text;

namespace InnerClassExample
{
    partial class A
    {
        private B b;//内部包容的嵌套类对象
        private int privateField = 0;//将被嵌套类对象修改的私有字段
        public A()
        {
            b = new B(this); //创建包容的对象，将自身引用传给嵌套类
        }

    }
}
