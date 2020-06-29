using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;

namespace InterfaceLibrary
{
    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class,AllowMultiple=false)]
    public class MyComponentAttribute:ExportAttribute
    {
        public MyComponentAttribute()
            : base(typeof(IMyInterface))
        {
        }

        public string ComponentName { get; set; }
    }
}
