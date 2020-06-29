using System;
using System.Collections.Generic;
using System.Text;

namespace CallBackExample
{
    class Controller
    {
        private ICallBack CallBackObject = null;//�ص�����
        public Controller(ICallBack obj)
        {
            this.CallBackObject = obj;
        }

           
        public void Begin()
        {
            
            Console.WriteLine("���������ʾ��ǰʱ�䣬ESC���˳�....");

            while (Console.ReadKey(true).Key != ConsoleKey.Escape)
            {
                CallBackObject.Run();
            }
         
        }


    }
}
