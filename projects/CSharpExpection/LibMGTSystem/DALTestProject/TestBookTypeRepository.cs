using System;
using LibraryModel;
using DataAccessLayer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace DALTestProject
{
    [TestClass]
    public class TestBookTypeRepository
    {
        BookTypeRepository bookTypeEntity = null;
        [TestInitialize]
        public void TestInitialize()
        {
            bookTypeEntity = new BookTypeRepository();
        }

        [TestMethod]
        public void TestGetAllClient()
        {
            List<BookType> bookTypes = bookTypeEntity.GetAllClient();
            foreach (var bookType in bookTypes)
            {
                Console.WriteLine(bookType.BookTypeName);
            }
        }
        [TestMethod]
        public async Task TestAdd()
        {
            BookType bookType = new BookType() { BookTypeName = "漫画" };
            bookTypeEntity.AddClient(bookType);
            int ret = await bookTypeEntity.SaveChangesAsync();
            Assert.IsTrue(ret == 1);
        }
        [TestMethod]
        public async Task TestDelete()
        {
            List<BookType> bookTypes = await bookTypeEntity.GetAllClientsAsync();
            BookType a = new BookType() { BookTypeName = "漫画" };
            var bookType = bookTypes.Find(r => a.BookTypeName == r.BookTypeName);
            bookTypeEntity.DeleteClient(bookType.BookTypeId);
            int ret = await bookTypeEntity.SaveChangesAsync();
            Assert.IsTrue(ret == 1);
        }
        [TestMethod]
        public async Task TestModifyClient()
        {
            var bookTypes = await bookTypeEntity.GetAllClientsAsync();
            BookType bookType = bookTypes.Last();
            BookType newBookType = new BookType() 
            { BookTypeId = bookType.BookTypeId, BookTypeName = "动画片" };
            bookTypeEntity.ModifyClient(newBookType);
            int ret = await bookTypeEntity.SaveChangesAsync();
            Assert.IsTrue(ret == 1);
            Console.WriteLine(bookType.BookTypeName);
        }
    }
}
