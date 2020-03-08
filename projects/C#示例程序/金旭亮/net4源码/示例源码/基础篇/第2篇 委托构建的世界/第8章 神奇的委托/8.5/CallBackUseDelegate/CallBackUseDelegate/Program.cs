using System;
using System.Collections.Generic;
using System.Text;

namespace CallBackUseDelegate
{
    class Program
    {
        static void Main(string[] args)
        {
            
            A a= new A();   //创建被回调方法的对象
            Controller c = new Controller();
            //注册两个回调方法
            c.RegisterDelegateForCallback(a.AShowTime);
            c.RegisterDelegateForCallback(B.BShowTime);

            //可以从回调列表中移除一个回调方法
            //c.UnRegisterDelegateForCallback(a.AShowTime);
           
            Console.WriteLine("敲任意键显示当前时间，ESC键退出....");

            while (Console.ReadKey(true).Key != ConsoleKey.Escape)
            {
                c.CallBack(); //回调
            }

        }
    }
}
