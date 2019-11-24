using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CallBack
{
    class CallBackClass2 : ICallBack
    {
        #region ICallBack 成员
        private int counter = 0;
        public void Run()
        {
            counter++;

            System.Media.SystemSounds.Asterisk.Play();
            Console.WriteLine("I'm invoked " + counter.ToString() + " times");

        }

        #endregion
    }
}
