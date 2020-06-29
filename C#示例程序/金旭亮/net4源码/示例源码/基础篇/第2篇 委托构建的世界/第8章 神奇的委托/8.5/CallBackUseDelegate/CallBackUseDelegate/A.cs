using System;
using System.Collections.Generic;
using System.Text;

namespace CallBackUseDelegate
{
    class A
    {
        public void AShowTime()
        {
            System.Console.WriteLine("A:" + DateTime.Now);
        }
    }

    class B
    {
        public static void BShowTime()
        {
            System.Console.WriteLine("B:" + DateTime.Now);
        }
    }
}
