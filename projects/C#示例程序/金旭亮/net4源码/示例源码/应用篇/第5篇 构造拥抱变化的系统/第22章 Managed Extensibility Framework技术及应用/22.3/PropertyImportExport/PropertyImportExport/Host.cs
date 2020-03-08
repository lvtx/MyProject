using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Reflection;
using System.Collections;

namespace PropertyImportExport
{
    public class Host<T>
    {
        [Import(typeof(IEnumerable))]
        public IEnumerable<T> collection { get; set; }

       
    }
}
