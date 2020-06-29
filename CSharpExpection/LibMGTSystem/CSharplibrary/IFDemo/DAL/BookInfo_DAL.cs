using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using System.Data.SqlClient;
using System.Data;

namespace DAL
{
    public class BookInfo_DAL
    {
        /// <summary>
        /// 查询BookInfo表
        /// </summary>
        /// <returns></returns>
        public List<BookInfo> selectBookInfo()
        {
            string sql = @"select BookId,BookName,TimeIn,BookTypeName,Author,PinYinCode,Translator,Language,
                            BookNumber,Price,Layout,Address,ISBS,Versions,BookRemark from BookInfo
                            inner join BookType on BookType.BookTypeId=BookInfo.BookTypeId";
            List<BookInfo> list = new List<BookInfo>();
            SqlDataReader reader = DBhelp.Create().ExecuteReader(sql);
            while (reader.Read())
            {
                BookInfo b = new BookInfo();
                b.BookId = reader.GetString(0);
                b.BookName = reader.GetString(1);
                b.TimeIn = reader.GetDateTime(2);
                b.BookType = new BookType();
                b.BookType.BookTypeName = reader.GetString(3);
                b.Author = reader.GetString(4);
                b.PinYinCode = reader.GetString(5);
                b.Translator = reader.GetString(6);
                b.Language = reader.GetString(7);
                b.BookNumber = reader.GetString(8);
                b.Price = reader.GetString(9);
                b.Layout = reader.GetString(10);
                b.Address = reader.GetString(11);
                b.ISBS = reader.GetString(12);
                b.Versions = reader.GetString(13);
                b.BookRemark = reader.GetString(14);
                list.Add(b);
            }
            reader.Close();
            return list;
        }
        /// <summary>
        /// 根据ID查询bookinfo表
        /// </summary>
        /// <param name="BookId"></param>
        /// <returns></returns>
        public List<BookInfo> selectBookInfo(string BookId)
        {
            string sql = @"select BookId,BookName,TimeIn,BookTypeId,Author,PinYinCode,Translator,Language,BookNumber, Price,Layout,Address,ISBS,Versions,BookRemark from BookInfo where BookId=@BookId";
            SqlParameter[] sp ={
                                new SqlParameter("@BookId",BookId)
                              };
            SqlDataReader reader = DBhelp.Create().ExecuteReader(sql, sp);
            List<BookInfo> list = new List<BookInfo>();
            while (reader.Read())
            {
                BookInfo b = new BookInfo();
                b.BookId = reader.GetString(0);
                b.BookName = reader.GetString(1);
                b.TimeIn = reader.GetDateTime(2);
                b.BookTypeId = reader.GetInt32(3);
                b.Author = reader.GetString(4);
                b.PinYinCode = reader.GetString(5);
                b.Translator = reader.GetString(6);
                b.Language = reader.GetString(7);
                b.BookNumber = reader.GetString(8);
                b.Price = reader.GetString(9);
                b.Layout = reader.GetString(10);
                b.Address = reader.GetString(11);
                b.ISBS = reader.GetString(12);
                b.Versions = reader.GetString(13);
                b.BookRemark = reader.GetString(14);
                list.Add(b);
            }
            reader.Close();
            return list;
        }


        /// <summary>
        /// 查询BookInfo表
        /// </summary>
        /// <returns></returns>
        public DataSet selectBookInfo1()
        {
            string sql = @"select BookId,BookName,TimeIn,BookTypeName,Author,PinYinCode,Translator,Language,BookNumber,Price,Layout,Address,ISBS,Versions,BookRemark from BookInfo inner join BookType on BookType.BookTypeId=BookInfo.BookTypeId";
            return DBhelp.Create().ExecuteAdater(sql);
        }
        public DataSet selectBook;

        /// <summary>
        /// 查询BookInfo表 目前可以借阅的图书
        /// </summary>
        /// <param name="BookId"></param>
        /// <returns></returns>
        public DataSet selectBookInfo2(string BookId)
        {
            string sql = @"select BookInfo.BookId as 'BookId',BookName,TimeIn,BookTypeName,Author,PinYinCode,Translator,Language,
                    BookNumber,Price,Layout,Address,ISBS,Versions,BookRemark from BookInfo
                    inner join BookType on BookType.BookTypeId=BookInfo.BookTypeId
                   
                    where BookInfo.BookId like '%'+@BookId+'%' and BookInfo.BookId not in(select BookId from BorrowReturn where FactReturnTime is null )";
            SqlParameter[] sp ={
                                   new SqlParameter("@BookId",BookId)
                              };
            return DBhelp.Create().ExecuteAdater(sql, sp: sp);
        }
        /// <summary>
        /// 根据条件查询bookinfo表
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public DataSet selectBookInfo1(int index)
        {
            string sql = @"select BookId,BookName,TimeIn,BookTypeName,Author,PinYinCode,Translator,Language,
                            BookNumber,Price,Layout,Address,ISBS,Versions,BookRemark from BookInfo
                            inner join BookType on BookType.BookTypeId=BookInfo.BookTypeId
                            where BookType.BookTypeId=@BookTypeId";
            SqlParameter[] sp ={
                                   new SqlParameter("@BookTypeId",index)
                              };
            return DBhelp.Create().ExecuteAdater(sql, sp);
        }

        /// <summary>
        /// 根据条件查询bookinfo表
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        public DataSet selectBookInfo1(string A, string B)
        {
            string sql = string.Format(@"select BookId,BookName,TimeIn,BookTypeName,Author,PinYinCode,Translator,Language,
                            BookNumber,Price,Layout,Address,ISBS,Versions,BookRemark from BookInfo
                            inner join BookType on BookType.BookTypeId=BookInfo.BookTypeId
                            where {0} like '%{1}%'", A, B);
            return DBhelp.Create().ExecuteAdater(sql);
        }
        /// <summary>
        /// 查询BookInfo表 带全部查询(表中所有相关的字段)
        /// </summary>
        /// <param name="list"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        public DataSet selectBookInfo1(List<string> list, string B)
        {
            string sql = "";
            for (int i = 0; i < list.Count; i++)
            {
                if (i != list.Count - 1)
                {
                    sql += string.Format(@"select BookId,BookName,TimeIn,BookTypeName,Author,PinYinCode,Translator,Language,
                            BookNumber,Price,Layout,Address,ISBS,Versions,BookRemark from BookInfo
                            inner join BookType on BookType.BookTypeId=BookInfo.BookTypeId
                            where {0} like '%{1}%' union  ", list[i], B);
                }
                else
                {
                    sql += string.Format(@"select BookId,BookName,TimeIn,BookTypeName,Author,PinYinCode,Translator,Language,
                            BookNumber,Price,Layout,Address,ISBS,Versions,BookRemark from BookInfo
                            inner join BookType on BookType.BookTypeId=BookInfo.BookTypeId
                            where {0} like '%{1}%' ", list[i], B);
                }

            }

            return DBhelp.Create().ExecuteAdater(sql);
        }
        /// <summary>
        /// 添加图书信息
        /// </summary>
        /// <param name="book"></param>
        /// <returns></returns>
        public int AddBookInfo(BookInfo book)
        {
            string sql = @"insert into BookInfo select @BookId,@BookName,@TimeIn,@BookTypeId,@Author,
                    @PinYinCode,@Translator,@Language,@BookNumber,@Price,@Layout,@Address,@ISBS,@Versions,@BookRemark";
            SqlParameter[] sp ={
                                   new SqlParameter("@BookId",book.BookId),
                                   new SqlParameter("@BookName",book.BookName),
                                   new SqlParameter("@TimeIn",book.TimeIn),
                                   new SqlParameter("@BookTypeId",book.BookTypeId),
                                   new SqlParameter("@Author",book.Author),
                                   new SqlParameter("@PinYinCode",book.PinYinCode),
                                   new SqlParameter("@Translator",book.Translator),
                                   new SqlParameter("@Language",book.Language),
                                   new SqlParameter("@BookNumber",book.BookNumber),
                                   new SqlParameter("@Price",book.Price),
                                   new SqlParameter("@Layout",book.Layout),
                                   new SqlParameter("@Address",book.Address),
                                   new SqlParameter("@ISBS",book.ISBS),
                                   new SqlParameter("@Versions",book.Versions),
                                   new SqlParameter("@BookRemark",book.BookRemark),
                              };
            return DBhelp.Create().ExecuteNonQuery(sql, sp: sp);
        }
        /// <summary>
        /// 修改图书信息
        /// </summary>
        /// <param name="book"></param>
        /// <returns></returns>
        public int ExitBookInfo(BookInfo book)
        {
            string sql = @"update BookInfo set BookName=@BookName,TimeIn=@TimeIn,BookTypeId=@BookTypeId,
            Author=@Author,PinYinCode=@PinYinCode,Translator=@Translator,Language=@Language,BookNumber=@BookNumber,
            Price=@Price,Layout=@Layout,Address=@Address,ISBS=@ISBS,Versions=@Versions,BookRemark=@BookRemark
            where BookId=@BookId";
            SqlParameter[] sp ={
                                    new SqlParameter("@BookName",book.BookName),
                                    new SqlParameter("@TimeIn",book.TimeIn),
                                    new SqlParameter("@BookTypeId",book.BookTypeId),
                                    new SqlParameter("@Author",book.Author),
                                    new SqlParameter("@PinYinCode",book.PinYinCode),
                                    new SqlParameter("@Translator",book.Translator),
                                    new SqlParameter("@Language",book.Language),
                                    new SqlParameter("@BookNumber",book.BookNumber),
                                    new SqlParameter("@Price",book.Price),
                                    new SqlParameter("@Layout",book.Layout),
                                    new SqlParameter("@Address",book.Address),
                                    new SqlParameter("@ISBS",book.ISBS),
                                    new SqlParameter("@Versions",book.Versions),
                                    new SqlParameter("@BookRemark",book.BookRemark),
                                    new SqlParameter("@BookId",book.BookId)
                              };

            return DBhelp.Create().ExecuteNonQuery(sql, sp: sp);
        }
        /// <summary>
        /// 根据Id删除
        /// </summary>
        /// <param name="BookId"></param>
        /// <returns></returns>
        public int DeleteBookInfo(string BookId)
        {
            string sql = @"
                            delete from BorrowReturn where BookId=@BookId
                            delete from BookInfo where BookId=@BookId";
            SqlParameter[] sp ={
                                   new SqlParameter("@BookId",BookId)
                              };
            return DBhelp.Create().ExecuteNonQuery(sql, sp: sp);
        }
    }
}
