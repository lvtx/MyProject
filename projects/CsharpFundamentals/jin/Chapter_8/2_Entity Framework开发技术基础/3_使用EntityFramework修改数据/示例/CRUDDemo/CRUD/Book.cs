//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CRUD
{
    using System;
    using System.Collections.Generic;
    
    public partial class Book
    {
        public Book()
        {
            this.BookReviews = new HashSet<BookReview>();
        }
    
        public int BookId { get; set; }
        public string BookName { get; set; }
    
        public virtual ICollection<BookReview> BookReviews { get; set; }
    }
}
