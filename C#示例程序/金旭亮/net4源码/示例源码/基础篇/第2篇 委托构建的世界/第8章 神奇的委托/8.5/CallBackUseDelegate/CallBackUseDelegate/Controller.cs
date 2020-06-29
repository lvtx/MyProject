using System;
using System.Collections.Generic;
using System.Text;

namespace CallBackUseDelegate
{
    public delegate void ShowTimeDelegate();

    class Controller
    {
        private ShowTimeDelegate d; //用于接收外界对象提供的方法，以实现回调

        //外界对象将需要回调的方法传入
        public void RegisterDelegateForCallback(ShowTimeDelegate method)
        {
            d += method;
        }

        //移除要回调的方法
        public void UnRegisterDelegateForCallback(ShowTimeDelegate method)
        {
            d -= method;
        }

        //实现回调
        public void CallBack()
        {
            if (d != null)
                d.Invoke();  //调用所有回调的方法
        }

    }
}
