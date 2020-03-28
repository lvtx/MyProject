using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using DAL;
using System.Data;

namespace BLL
{
    public class BookInfo_BLL
    {
        BookInfo_DAL b = new BookInfo_DAL();

        /// <summary>
        /// 查询BookInfo表
        /// </summary>
        /// <returns></returns>
        public List<BookInfo> selectBookInfo()
        {
            return b.selectBookInfo();
        }
        /// <summary>
        /// 查询BookInfo表
        /// </summary>
        /// <param name="BookId"></param>
        /// <returns></returns>
        public List<BookInfo> selectBookInfo(string BookId)
        {
            return b.selectBookInfo(BookId);
        }
        /// <summary>
        /// 查询BookInfo表
        /// </summary>
        /// <returns></returns>
        public DataSet selectBookInfo1()
        {
            return b.selectBookInfo1();
        }
        /// <summary>
        /// 查询BookInfo表带条件
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public DataSet selectBookInfo1(int index)
        {
            return b.selectBookInfo1(index);
        }
        /// <summary>
        /// 查询BookInfo表带条件
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        public DataSet selectBookInfo1(string A, string B)
        {
            return b.selectBookInfo1(A, B);
        }

        //查询BookInfo表带条件 全部
        public DataSet selectBookInfo1(List<string> list, string B)
        {
            return b.selectBookInfo1(list, B);
        }

        /// <summary>
        /// 查询BookInfo表 目前可以借阅的图书
        /// </summary>
        /// <param name="BookId"></param>
        /// <returns></returns>
        public DataSet selectBookInfo2(string BookId)
        {
            return b.selectBookInfo2(BookId);
        }
        //修改图书信息
        public int ExitBookInfo(BookInfo book)
        {
            return b.ExitBookInfo(book);
        }

        //添加图书信息
        public int AddBookInfo(BookInfo book)
        {
            return b.AddBookInfo(book);
        }

        //删除图书信息
        public int DeleteBookInfo(string BookId)
        {
            return b.DeleteBookInfo(BookId);
        }
    }
}
