using System;
using System.Collections.Generic;
using System.Text;

namespace CallBackExample
{
    class Controller
    {
        private List<ICallBack> CallBackObjects = new List<ICallBack>();//回调对象

        public void AddCallBack(ICallBack callback)
        {
            CallBackObjects.Add(callback);
        }

        public void Begin()
        {
            Console.WriteLine("敲任意键回调方法 , ESC键退出...");

            while (Console.ReadKey(true).Key != ConsoleKey.Escape)
            {
                Console.WriteLine();
                foreach (ICallBack obj in CallBackObjects)
                    obj.run();
            }

        }


    }
}
