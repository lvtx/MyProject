using System;
using System.Collections.Generic;
using System.Text;

namespace CallBackExample
{
    class Program
    {
        static void Main(string[] args)
        {
            //创建控制器对象，将提供给它的回调对象传入
            Controller obj = new Controller(new CallBackClass());
            //启动控制器对象运行
            obj.Begin();
        }
    }
}
