using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;

namespace StrongTypeExportMetadataExample
{
    [MetadataAttribute]   //指明此属性用于元数据定义
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class MyExportAttribute : ExportAttribute
    {
        public string PartName { get; set; }   //这是附加的元数据

        public MyExportAttribute()
            : base(typeof(IMyPart))  //指定此导出属性应用于IMyPart接口
        {
        }
    }
}
