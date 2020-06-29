using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;

namespace MethodImportExport
{
  
    public class AddProcessor
    {
        [Export(typeof(Func<int, int, long>))]
        public long Add(int num1, int num2)
        {
            return num1 + num2;
        }
    }

}
