using System;
using System.Collections.Generic;
using System.Text;

namespace CallBackExample
{
    class CallBackClass:ICallBack 
    {
        public void Run()
        {
            //输出当前时间
            System.Console.WriteLine(DateTime.Now );
        }
    }
}
