using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PartialMethod
{

    partial class MyClass
    {
        //===============================================
        //是否取消以下方法的注释，会直接影响到C#编译器是否为
        //分部方法生成IL指令。
        //===============================================

        //partial void PartialMethod() //分部方法的实现代码
        //{
        //    Console.WriteLine("分部方法中的功能代码");
        //}
    }
}
