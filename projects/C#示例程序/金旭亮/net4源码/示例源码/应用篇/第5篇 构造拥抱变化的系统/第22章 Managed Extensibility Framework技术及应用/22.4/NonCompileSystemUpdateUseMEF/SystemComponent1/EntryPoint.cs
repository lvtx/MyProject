using System;
using System.Collections.Generic;
using System.Text;
using InterfaceLibrary;
using System.ComponentModel.Composition;

namespace SystemComponent1
{
    
    [MyComponent(ComponentName="SystemComponent1")]
    public class EntryPoint:IMyInterface
    {
       
        void IMyInterface.Run()
        {
            frmComponent1 frm = new frmComponent1();
            frm.Show();
        }

        
    }
}
