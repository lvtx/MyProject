using System;
using System.Threading.Tasks;
using EFConsoleApp;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.Entity;

namespace TestEFConsoleApp.OneToMany
{
    [TestClass]
    public class OneToManyTest
    {
        #region "Query"
        [TestMethod]
        public async Task TestExplicitLoadingBookReview()
        {
            using (var context = new MyDBEntities())
            {
                var query = from book in context.Books
                            select book;
                Book firstBook = await query.FirstAsync();
                Assert.IsFalse(firstBook.BookReviews.Count == 0);
                context.Entry<Book>(firstBook)
                    .Collection<BookReview>("BookReviews").Load();
                Assert.IsTrue(firstBook.BookReviews.Count > 0);
            }
        }
        #endregion

        #region "Add"
        /// <summary>
        /// 在内存中创建10本书，每本书10个书评，保存到数据库中
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task TestAddBooks()
        {
            using (var context = new MyDBEntities())
            {
                var books = OneToManyHelper.CreateBooks(10, 10);
                context.Books.AddRange(books);
                int result = await context.SaveChangesAsync();
                Assert.IsTrue(result > 10);
            }
        }
        /// <summary>
        /// 给书添加书评
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task TestAddBookReviewToBooks()
        {
            using (var context = new MyDBEntities())
            {
                var firstBook = await context.Books.FirstOrDefaultAsync();
                int reviewCount = firstBook.BookReviews.Count;
                BookReview review = OneToManyHelper.CreateBookReview(firstBook);
                int result = await context.SaveChangesAsync();
                Assert.IsTrue(result > 0);

                Book firstBookBySearched = await context.Books.FirstOrDefaultAsync();
                Assert.IsTrue(reviewCount + 1
                    == firstBookBySearched.BookReviews.Count);
                //查找新添加的书评
                BookReview newReview = firstBookBySearched.BookReviews.
                    FirstOrDefault(r => r.BookReviewId == review.BookReviewId);
                Assert.IsNotNull(newReview);
            }
        }
        #endregion

        #region "Delete"
        //删除整本书
        [TestMethod]
        public async Task TestDeleteBook()
        {
            using (var context = new MyDBEntities())
            {
                Book lastBook = await (from book in context.Books
                                       orderby book.BookId descending
                                       select book).FirstAsync();
                if (lastBook != null)
                {
                    context.Books.Remove(lastBook);
                    int result = await context.SaveChangesAsync();
                    Assert.IsTrue(result > 0);
                    //测试是否真的被删除
                    Book bookBySearched = context.Books.FirstOrDefault
                        (b => b.BookId == lastBook.BookId);
                    Assert.IsNull(bookBySearched);
                    //测试书评是否也被删除
                    var query = from review in context.BookReviews
                                where review.BookId == lastBook.BookId
                                select review;
                    Assert.IsTrue(query.Count() == 0);
                }
            }
        }
        //删除最后一本书的第一个书评
        [TestMethod]
        public async Task TestDeleteFirstBookFirstBookReview()
        {
            using (var context = new MyDBEntities())
            {
                Book lastbook = await (from book in context.Books
                                       orderby book.BookId descending
                                       select book).Include("BookReviews").
                                       FirstOrDefaultAsync();
                if (lastbook != null && lastbook.BookReviews.Count > 0)
                {
                    BookReview review = lastbook.BookReviews.FirstOrDefault();
                    int reviewCount = lastbook.BookReviews.Count;
                    //从主对象的集合属性中移除（可选）
                    //firstBook.BookReviews.Remove(firstBookReview);

                    //从DbSet中移除
                    context.BookReviews.Remove(review);
                    int result = await context.SaveChangesAsync();
                    Assert.IsTrue(result == 1);

                    //重新提取记录
                    Book bookBySearched = await (from book in context.Books
                                           orderby book.BookId descending
                                           select book).Include("BookReviews").
                                           FirstOrDefaultAsync();
                    Assert.IsNotNull(bookBySearched);
                    Assert.IsTrue(bookBySearched.BookReviews.Count
                        == reviewCount - 1);
                    //确认相关书评己经被删除
                    var query = from re in context.BookReviews
                                where re.BookReviewId == review.BookReviewId
                                select re;
                    Assert.IsTrue(query.Count() == 0);
                }
            }
        }
        #endregion

        #region "Update"
        //更新单个图书信息(略）

        //更新单个书评信息（略）

        //将第一本书的第一个书评移动到第二本书
        [TestMethod]
        public async Task TestBookReviewMove()
        {
            using (var context = new MyDBEntities())
            {
                //找到第一本和第二本书，把第一本书的第一条书评移给第二本书
                Book lastBook = await (from book in context.Books
                                       orderby book.BookId descending
                                       select book).Include("BookReviews").FirstAsync();
                Book secondBook = (from book in context.Books
                                         orderby book.BookId descending
                                         select book).Include("BookReviews")
                                         .ToList().ElementAt(1);
                if (lastBook != null && lastBook.BookReviews.Count > 0)
                {
                    BookReview bookReview = lastBook.BookReviews.ElementAt(0);
                    OneToManyHelper.ShowBookReivew(bookReview);
                    //先从原对象集合中移除
                    lastBook.BookReviews.Remove(bookReview);
                    //再追加到新对象的集合中
                    secondBook.BookReviews.Add(bookReview);
                    int result = await context.SaveChangesAsync();
                    Assert.IsTrue(result > 0);
                    //重新提取书评
                    BookReview reviewFromDB = await context.BookReviews.
                        FirstOrDefaultAsync(r => r.BookReviewId == bookReview.BookReviewId);
                    Assert.IsTrue(reviewFromDB.BookId == secondBook.BookId);
                    OneToManyHelper.ShowBookReivew(reviewFromDB);
                }
            }
        }
        #endregion
    }
}
