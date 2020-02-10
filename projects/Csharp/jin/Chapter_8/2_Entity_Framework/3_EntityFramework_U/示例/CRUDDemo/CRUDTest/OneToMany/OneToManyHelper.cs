
using CRUD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUDTest.OneToMany
{
    public class OneToManyHelper
    {
        private static Random ran = new Random();
        public static Book CreateBook()
        {
            Book book = new Book()
            {
                 BookName="Book"+ran.Next(1,10000)
            };
            return book;
        }
        /// <summary>
        /// 如果book不为null，则创建一个BookReview并将其插入到Book的书评集合中
        /// 否则，创建独立的BookReview
        /// </summary>
        /// <param name="book"></param>
        /// <returns></returns>
        public static BookReview CreateBookReview(Book book = null)
        {
            BookReview review = new BookReview(); 
            review.ReaderName = "读者" + ran.Next(1, 1000);
            if (book != null)
            {
                //设置导航属性
                review.Book = book;
                //设置外键关联
                review.BookId = book.BookId;
                review.Review = book.BookName + "的书评" + ran.Next(1, 1000);
                //将自己追加到BookReivews集合中
                book.BookReviews.Add(review);
            }
            else
            {
                review.Review = "书评" + ran.Next(1, 1000);
            }
            return review;
        }

        //创建指定数目的图书，每本书包容reviewCount个书评
        public static List<Book> CreateBooks(int bookCount,int reviewCount)
        {
            List<Book> books = new List<Book>();
            Book book = null;
            for (int i = 0; i <= bookCount; i++)
            {
                book = CreateBook();
                for (int j = 0; j < reviewCount;j++)
                    CreateBookReview(book);
                books.Add(book);
            }
            return books;
        }
        /// <summary>
        /// 显示一条书评的信息
        /// </summary>
        /// <param name="review"></param>
        public static void ShowBookReivew(BookReview review)
        {
            if (review != null)
            {
                Console.WriteLine("BookReviewId:{0} ReaderName:{1} Review:{2},属于书：{3}", 
                    review.BookReviewId, 
                    review.ReaderName,
                    review.Review,
                    review.BookId);
            }
        }
    }
}
