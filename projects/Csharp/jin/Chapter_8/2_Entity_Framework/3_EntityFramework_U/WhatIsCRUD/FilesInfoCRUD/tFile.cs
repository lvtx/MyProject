//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FilesInfoCRUD
{
    using System;
    using System.Collections.Generic;
    
    public partial class tFile
    {
        public int FileId { get; set; }
        public int DirectoryId { get; set; }
        public string sFile { get; set; }
    
        public virtual tDirectory tDirectory { get; set; }
    }
}
