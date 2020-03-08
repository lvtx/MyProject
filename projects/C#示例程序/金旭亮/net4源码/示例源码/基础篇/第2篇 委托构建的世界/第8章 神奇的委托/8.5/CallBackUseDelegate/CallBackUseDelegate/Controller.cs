using System;
using System.Collections.Generic;
using System.Text;

namespace CallBackUseDelegate
{
    public delegate void ShowTimeDelegate();

    class Controller
    {
        private ShowTimeDelegate d; //���ڽ����������ṩ�ķ�������ʵ�ֻص�

        //��������Ҫ�ص��ķ�������
        public void RegisterDelegateForCallback(ShowTimeDelegate method)
        {
            d += method;
        }

        //�Ƴ�Ҫ�ص��ķ���
        public void UnRegisterDelegateForCallback(ShowTimeDelegate method)
        {
            d -= method;
        }

        //ʵ�ֻص�
        public void CallBack()
        {
            if (d != null)
                d.Invoke();  //�������лص��ķ���
        }

    }
}
