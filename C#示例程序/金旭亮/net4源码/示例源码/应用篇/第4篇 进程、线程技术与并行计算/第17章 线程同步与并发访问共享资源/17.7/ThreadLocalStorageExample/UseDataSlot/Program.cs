using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace UseDataSlot
{
    class Program
    {
        static void Main(string[] args)
        {
            Thread[] newThreads = new Thread[4];
            for (int i = 0; i < newThreads.Length; i++)
            {
                newThreads[i] =
                    new Thread(new ThreadStart(UseSlotClass.SlotTest));
                newThreads[i].Start();
               
                
            }
            Console.ReadKey();

        }
    }
}
