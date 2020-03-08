using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyPartInterfaceLibrary;
using System.ComponentModel.Composition;

namespace MyPartLibrary2
{ 
    [Export(typeof(IMyPart))]
    public class MyPart2:IMyPart
    {
        
        public string IntroduceMySelt()
        {
            return "MyPart2";
        }
    }
}
