using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ObjectEmbodied
{
    /// <summary>
    /// 包容方式一
    /// 一般情况下，内部对象不能被外界直接访问
    /// （当然，根据需要也可以设为Public的）
    /// 要在包容类的构造函数中创建被包容对象
    /// 如果此种对象仅在本类中使用，还可以把其类的定义为内部类
    /// </summary>
    class OneToOneClass
    {
        private InnerClass obj;
        public OneToOneClass()
        {
            obj = new InnerClass();
        }
    }

    /// <summary>
    /// 包容方式二
    /// 包容的对象由外界负责创建,通常采用对象注入的方式
    /// </summary>
    class OneToOneClass2
    {
        private InnerClass obj = null;
        public OneToOneClass2(InnerClass outerObj)
        {
            this.obj = outerObj;
        }
    }
}
