using System;
using System.Collections.Generic;
using System.Text;

namespace CallBackExample
{
    class Controller
    {
        private ICallBack CallBackObject = null;//回调对象
        public Controller(ICallBack obj)
        {
            this.CallBackObject = obj;
        }

           
        public void Begin()
        {
            
            Console.WriteLine("敲任意键显示当前时间，ESC键退出....");

            while (Console.ReadKey(true).Key != ConsoleKey.Escape)
            {
                CallBackObject.Run();
            }
         
        }


    }
}
