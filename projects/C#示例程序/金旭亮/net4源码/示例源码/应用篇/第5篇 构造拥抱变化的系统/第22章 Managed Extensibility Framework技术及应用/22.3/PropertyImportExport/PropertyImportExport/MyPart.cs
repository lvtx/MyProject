using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using System.Collections;

namespace PropertyImportExport
{
    public class MyPart<T>
    {
        [Export(typeof(IEnumerable))]
        public List<T> numbers = new List<T>();


        
    }
}
