using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace UseDataSlot
{
    public class OtherClass
    {
        public void ModifySlotData()
        {
            LocalDataStoreSlot dataslot = Thread.GetNamedDataSlot("Random");

            object dataValue = Thread.GetData(dataslot);
            int OriginalValue = (int)dataValue;
            Console.WriteLine("OtherClass.ModifySlotData方法修改线程{0}中DataSlot中的数据，将其值*2", Thread.CurrentThread.ManagedThreadId);
            //修改数据
            Thread.SetData(dataslot, OriginalValue * 2);

        }

    }
}
