using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EFConsoleApp;
using System.Linq;
using System.Data.Entity;
using System.Threading.Tasks;

namespace TestEFConsoleApp.OneToOne
{
    [TestClass]
    public class OneToOneTest
    {
        #region "virtual代理类"
        [TestMethod]
        public async Task TestFetchFirstPersonAsync()
        {
            using (var context = new MyDBEntities())
            {
                Person person = await context.People.FirstOrDefaultAsync();
                if (person != null)
                {
                    Console.WriteLine("姓名: {0}",person.Name);
                    if (person.IdentityCard != null)
                    {
                        Console.WriteLine("身份证号: {0}",person.IdentityCard.IDNumber);
                    }
                    else
                    {
                        Console.WriteLine("身份证号: 无");
                    }
                }
            }
        }
        #endregion

        #region "预装载(Sql的联结)"
        [TestMethod]
        public async Task TestFetchFirstPersonUseInclude()
        {
            using (var context = new MyDBEntities())
            {
                Person person = await context.People
                    .Include("IdentityCard").FirstOrDefaultAsync();
                if (person != null)
                {
                    Console.WriteLine("姓名: {0},身份证号: {1}", person.Name, 
                        person.IdentityCard.IDNumber);
                }
            }
        }
        #endregion

        #region "显式加载"
        [TestMethod]
        public async Task TestExplicitLoadingBookReview()
        {
            using (var context = new MyDBEntities())
            {
                var query = from book in context.Books
                            select book;
                Book firstBook = await query.FirstAsync();
                Assert.IsTrue(firstBook.BookReviews.Count == 0);
                context.Entry<Book>(firstBook).Collection<BookReview>("BookReviews").Load();
                Assert.IsTrue(firstBook.BookReviews.Count > 0);
            }
        }
        #endregion

        
    }
}
