using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;

namespace WhatIsMEF1
{
    [Export(typeof(IPlugIn))]
    public class MyPlugIn : IPlugIn
    {
        public void Print(string message)
        {
            Console.WriteLine("MyPlugIn print:" + message);
        }
    }

    

}
