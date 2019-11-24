using System;
using System.Collections.Generic;
using System.Text;

namespace CallBackExample
{
    //实现回调的类必须实现此接口
    public interface ICallBack
    {
         void run();
    }
}
