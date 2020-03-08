using System;
using System.Collections.Generic;
using System.Text;

namespace InnerClassExample
{
    partial class A
    {
        private class B  //向外界隐藏内部类的定义
        {
            private A container; //保存外部类的对象引用

            public B(A obj) //接收外部类的对象引用
            {
                container = obj;
            }
            public void ChangeValue()
            {
                container.privateField += 1;//访问外部类的私有字段
            }

        }
    }
}
