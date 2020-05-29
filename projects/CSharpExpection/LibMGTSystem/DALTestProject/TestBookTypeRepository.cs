using System;
using LibraryModel;
using DataAccessLayer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace DALTestProject
{
    [TestClass]
    public class TestBookTypeRepository
    {
        [TestMethod]
        public void ShowBookTypes()
        {
            BookTypeRepository bookTypeEntity = new BookTypeRepository();
            List<BookType> bookTypes = bookTypeEntity.GetAllClient();
            foreach (var bookType in bookTypes)
            {
                Console.WriteLine(bookType.BookTypeName);
            }
        }
    }
}
