using Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class BorrowReturn_DAL
    {
        //查询借还表信息
        public DataSet selectHostory(BorrowReturn b, string radioName, String cboBorrowTimeType, Boolean checkTime)
        {
            string sql = string.Format(@"select BookInfo.BookId as 'BookId',Reader.ReaderId as 'ReaderId',BookName,ReaderName,
                            BookTypeName,ReaderTypeName,Gender,IdentityCard,
                            BorrowTime,ReturnTime,FactReturnTime,Fine,RenewCount
                            from BorrowReturn
                            inner join BookInfo on BookInfo.BookId=BorrowReturn.BookId
                            inner join Reader on Reader.ReaderId=BorrowReturn.ReaderId
                            inner join BookType on BookType.BookTypeId=BookInfo.BookTypeId
                            inner join ReaderType on ReaderType.ReaderTypeId=Reader.ReaderTypeId
                            where BookInfo.BookId like '%{0}%' and 
                            Reader.ReaderId like '%{1}%' ", b.BookId, b.ReaderId);
            if (radioName == "全部")
            {

            }
            else if (radioName == "已借")
            {
                sql += " and FactReturnTime	is  null ";
            }
            else if (radioName == "已还")
            {
                sql += " and FactReturnTime	is not null ";
            }
            if (checkTime)
            {
                if (cboBorrowTimeType == "日期")
                {
                    sql += string.Format(@" and BorrowId in(
                                        select BorrowId from BorrowReturn where BorrowTime between '{0}' and '{1}' union
                                        select BorrowId from BorrowReturn where ReturnTime between '{0}' and '{1}' union
                                        select BorrowId from BorrowReturn where FactReturnTime between '{0}' and '{1}'
                                        ) ", b.TimeIn, b.TimeOut);
                }
                else if (cboBorrowTimeType == "借书日期")
                {
                    sql += @" and BorrowTime between '" + b.TimeIn + "' and '" + b.TimeOut + "' ";
                }
                else if (cboBorrowTimeType == "应还书日期")
                {
                    sql += @" and ReturnTime between '" + b.TimeIn + "' and  '" + b.TimeOut + "' ";
                }
                else if (cboBorrowTimeType == "实际还书日期")
                {
                    sql += @" and FactReturnTime between '" + b.TimeIn + "' and '" + b.TimeOut + "' ";
                }
            }

            return DBhelp.Create().ExecuteAdater(sql);
        }

        //借还表全部信息 包括现在借阅的和历史借阅的
        public DataSet AllBorrowReturn()
        {
            string sql = @"select BookInfo.BookId as 'BookId',Reader.ReaderId as 'ReaderId',BookName,ReaderName,
                            BookTypeName,ReaderTypeName,Gender,IdentityCard,
                            BorrowTime,ReturnTime,FactReturnTime,Fine,RenewCount
                            from BorrowReturn
                            inner join BookInfo on BookInfo.BookId=BorrowReturn.BookId
                            inner join Reader on Reader.ReaderId=BorrowReturn.ReaderId
                            inner join BookType on BookType.BookTypeId=BookInfo.BookTypeId
                            inner join ReaderType on ReaderType.ReaderTypeId=Reader.ReaderTypeId";
            return DBhelp.Create().ExecuteAdater(sql);
        }

        //查询图书借还表（表连接）
        public DataSet selectBorrowReturn(string BookId)
        {
            string sql = @"select BookInfo.BookId,BookName,Reader.ReaderId,ReaderName,BorrowTime,ReturnTime,FactReturnTime,Fine,RenewCount,BorrowRemark from BookInfo
                    inner join BorrowReturn on BorrowReturn.BookId=BookInfo.BookId
                    inner join Reader on Reader.ReaderId=BorrowReturn.ReaderId 
                    where BookInfo.BookId=@BookId ";
            SqlParameter[] sp ={
                                   new SqlParameter("@BookId",BookId)
                              };

            return DBhelp.Create().ExecuteAdater(sql, sp);
        }


        //读者借阅记录
        public DataSet ReaderBorrowReturn(string ReaderId)
        {
            string sql = @"select BorrowId,Reader.ReaderId as 'ReaderId',ReaderName,BookInfo.BookId as 'BookId',BookName,BorrowTime,ReturnTime,FactReturnTime,Fine,RenewCount,BorrowRemark from Reader
                            inner join BorrowReturn on BorrowReturn.ReaderId=Reader.ReaderId
                            inner join BookInfo on BookInfo.BookId=BorrowReturn.BookId
                            where Reader.ReaderId=@ReaderId and FactReturnTime is null ";
            SqlParameter[] sp ={
                                   new SqlParameter("@ReaderId",ReaderId)
                              };
            return DBhelp.Create().ExecuteAdater(sql, sp);
        }

        //读者历史借阅记录
        public DataSet ReaderBorrowReturn1(string ReaderId)
        {
            string sql = @"select BorrowId,Reader.ReaderId as 'ReaderId',ReaderName,BookInfo.BookId as 'BookId',BookName,BorrowTime,ReturnTime,FactReturnTime,Fine,RenewCount,BorrowRemark from Reader
                            inner join BorrowReturn on BorrowReturn.ReaderId=Reader.ReaderId
                            inner join BookInfo on BookInfo.BookId=BorrowReturn.BookId
                            where Reader.ReaderId=@ReaderId and FactReturnTime is not null ";
            SqlParameter[] sp ={
                                   new SqlParameter("@ReaderId",ReaderId)
                              };
            return DBhelp.Create().ExecuteAdater(sql, sp);
        }

        //还书
        public int ReturnBook(int BorrowReturnId)
        {
            string sql = @"update BorrowReturn set FactReturnTime=@FactReturnTime,RenewCount=0 where BorrowId=@BorrowId";
            SqlParameter[] sp ={
                                   new SqlParameter("@FactReturnTime",DateTime.Now),
                                   new SqlParameter("@BorrowId",BorrowReturnId)
                              };
            return DBhelp.Create().ExecuteNonQuery(sql, sp: sp);
        }

        //借书
        public int BorrowBook(BorrowReturn b)
        {
            string sql = "proc_BorrowBook";
            SqlParameter[] sp ={
                                   new SqlParameter("@BorrowId",DbType.Int32),
                                   new SqlParameter("@BookId",b.BookId),
                                   new SqlParameter("@ReaderId",b.ReaderId),
                                   new SqlParameter("@BorrowTime",b.BorrowTime),
                                   new SqlParameter("@ReturnTime",b.ReturnTime),
                                   new SqlParameter("@Fine",b.Fine),
                                   new SqlParameter("@RenewCount",b.RenewCount),
                                   new SqlParameter("@BorrowRemark",b.BorrowRemark),
                                   new SqlParameter("@ReturnValue",DbType.Int32)
                              };
            sp[0].Direction = ParameterDirection.Output;
            sp[sp.Length - 1].Direction = ParameterDirection.ReturnValue;
            DBhelp.Create().ExecuteNonQuery(sql, CommandType.StoredProcedure, sp);
            b.BorrowId = (int)sp[0].Value;
            return (int)sp[sp.Length - 1].Value;
        }

        //续借
        public int RenewBook(BorrowReturn b)
        {
            string sql = @"update BorrowReturn set ReturnTime=dateadd(month,3,ReturnTime),RenewCount=RenewCount+1 where BorrowId=@BorrowId";
            SqlParameter[] sp ={
                                   new SqlParameter("@BorrowId",b.BorrowId)
                              };
            return DBhelp.Create().ExecuteNonQuery(sql, sp: sp);
        }
    }
}
