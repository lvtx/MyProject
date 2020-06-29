using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;

namespace ImportingConstructor
{
    [Export]
    public class MyPart2
    {
      
        public string Information;
     
        public DateTime DefaultTime;


        [ImportingConstructor]
        public MyPart2(string info, DateTime time)
        {
            Information = info;
            DefaultTime = time;
        }
        
    }
}
