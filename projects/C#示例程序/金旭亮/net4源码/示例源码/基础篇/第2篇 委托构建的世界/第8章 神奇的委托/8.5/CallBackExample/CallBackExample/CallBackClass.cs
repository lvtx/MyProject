using System;
using System.Collections.Generic;
using System.Text;

namespace CallBackExample
{
    class CallBackClass:ICallBack 
    {
        public void Run()
        {
            //�����ǰʱ��
            System.Console.WriteLine(DateTime.Now );
        }
    }
}
