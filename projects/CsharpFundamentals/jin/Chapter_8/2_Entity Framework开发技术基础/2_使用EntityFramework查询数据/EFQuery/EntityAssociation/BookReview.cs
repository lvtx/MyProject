//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EntityAssociation
{
    using System;
    using System.Collections.Generic;
    
    public partial class BookReview
    {
        public int BookReviewId { get; set; }
        public string Review { get; set; }
        public int BookId { get; set; }
        public string ReaderName { get; set; }
    
        public virtual Book Book { get; set; }
    }
}
