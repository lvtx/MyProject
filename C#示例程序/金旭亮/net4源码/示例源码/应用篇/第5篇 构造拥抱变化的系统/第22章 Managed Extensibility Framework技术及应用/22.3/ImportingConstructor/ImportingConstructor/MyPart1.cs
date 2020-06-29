using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;

namespace ImportingConstructor
{
    class MyPart1
    {
        [Export]
        public string Infomation = "Hello";
        [Export]
        public DateTime DefaultTime = DateTime.Now;
    }
}
