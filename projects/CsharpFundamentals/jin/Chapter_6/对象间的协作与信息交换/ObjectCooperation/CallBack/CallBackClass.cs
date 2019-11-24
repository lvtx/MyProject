using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CallBack
{
    class CallBackClass:ICallBack
    {
        public void Run()
        {
            Console.WriteLine(DateTime.Now);
        }
    }
}
