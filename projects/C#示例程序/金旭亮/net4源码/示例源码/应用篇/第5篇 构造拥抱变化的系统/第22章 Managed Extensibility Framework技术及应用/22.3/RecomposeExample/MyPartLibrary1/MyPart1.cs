using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyPartInterfaceLibrary;
using System.ComponentModel.Composition;

namespace MyPartLibrary1
{
    [Export(typeof(IMyPart))]
    public class MyPart1:IMyPart
    {
        public string IntroduceMySelt()
        {
            return "MyPart1";
        }
    }
}
