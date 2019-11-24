using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CallBack
{
    class Program
    {
        static void Main(string[] args)
        {
            Controller controller = new Controller();
            controller.AddObject(new CallBackClass());
            controller.AddObject(new CallBackClass2());
            controller.Begin();
        }
    }
}
