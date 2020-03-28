//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class BookInfo
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public BookInfo()
        {
            this.BorrowReturns = new HashSet<BorrowReturn>();
        }
    
        public string BookId { get; set; }
        public string BookName { get; set; }
        public System.DateTime TimeIn { get; set; }
        public int BookTypeId { get; set; }
        public string Author { get; set; }
        public string PinYinCode { get; set; }
        public string Translator { get; set; }
        public string Language { get; set; }
        public string Price { get; set; }
        public string Layout { get; set; }
        public string Address { get; set; }
        public string ISBS { get; set; }
        public string Versions { get; set; }
        public string BookRemark { get; set; }
        public string PageNumber { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BorrowReturn> BorrowReturns { get; set; }
        public virtual BookType BookType { get; set; }
    }
}
