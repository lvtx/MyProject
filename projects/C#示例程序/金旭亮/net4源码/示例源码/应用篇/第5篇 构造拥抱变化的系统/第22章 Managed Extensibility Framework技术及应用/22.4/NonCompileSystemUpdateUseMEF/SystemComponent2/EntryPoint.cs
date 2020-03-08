using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.Composition;
using InterfaceLibrary;

namespace SystemComponent2
{
     [MyComponent(ComponentName = "SystemComponent2")]
    public class EntryPoint:InterfaceLibrary.IMyInterface
    {
         void InterfaceLibrary.IMyInterface.Run()
        {
            frmComponent2 frm = new frmComponent2();
            frm.Show();
        }

        
    }
}
