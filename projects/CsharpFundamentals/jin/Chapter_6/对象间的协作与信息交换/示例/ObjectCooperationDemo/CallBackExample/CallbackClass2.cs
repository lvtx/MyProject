using System;
using System.Collections.Generic;
using System.Text;


namespace CallBackExample
{
    class CallBackClass2:ICallBack
    {
        #region ICallBack 成员
        private int counter = 0;
        public void run()
        {
            counter++;
           
            System.Media.SystemSounds.Asterisk.Play();
            Console.WriteLine("I'm invoked " + counter.ToString() + " times");
            
        }

        #endregion
    }
}
