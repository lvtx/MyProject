//展示部分类的使用

using System;
using System.Collections.Generic;
using System.Text;
using MyDLL;    //声明程序集命名空间
using MyDLL.MyChildDLL;

namespace UseDLL
{
    class Program
    {
        static void Main(string[] args)
        {
            //创建程序集中的类的对象
            MyPublicClass obj = new MyPublicClass();
            //访问类的基本功能
            obj.i = 100;
            obj.f();
            //访问类的扩充功能
            obj.j = 200;
            obj.g();
        }
    }
}
