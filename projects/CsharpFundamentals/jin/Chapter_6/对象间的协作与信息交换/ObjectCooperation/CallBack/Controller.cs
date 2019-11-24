using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CallBack
{
    class Controller
    {
        public List<ICallBack> CallBackObjects = new List<ICallBack>();

        public void AddObject(ICallBack callBackObject)
        {
            CallBackObjects.Add(callBackObject);
        }

        public void Begin()
        {
            while(Console.ReadKey(true).Key != ConsoleKey.Escape)
            {
                Console.WriteLine();
                foreach (ICallBack callBackItem in CallBackObjects)
                {
                    callBackItem.Run();
                }
            }
        }
    }
}
