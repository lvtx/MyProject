using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using PlugInInterface;

namespace MyOtherPlugInLibrary
{
    [Export(typeof(IPlugIn))]
    public class MyOtherPlugIn : IPlugIn
    {
        public void Print(string message)
        {
            Console.WriteLine("MyOtherPlugIn print:" + message);
        }
    }
}
