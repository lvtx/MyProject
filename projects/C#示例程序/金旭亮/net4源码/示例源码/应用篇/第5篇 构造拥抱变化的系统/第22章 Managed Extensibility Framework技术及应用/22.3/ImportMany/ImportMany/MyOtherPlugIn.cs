using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;

namespace WhatIsMEF2
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
