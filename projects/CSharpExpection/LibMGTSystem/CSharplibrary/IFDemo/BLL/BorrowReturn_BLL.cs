using DAL;
using Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class BorrowReturn_BLL
    {
        BorrowReturn_DAL borrowReturn_dal = new BorrowReturn_DAL();

        //查询借还表信息
        public DataSet selectHostory(BorrowReturn b, string radioName, String cboBorrowTimeType, Boolean checkTime)
        {
            return borrowReturn_dal.selectHostory(b, radioName, cboBorrowTimeType, checkTime);
        }
        //借还表全部信息 包括现在借阅的和历史借阅的
        public DataSet AllBorrowReturn()
        {
            return borrowReturn_dal.AllBorrowReturn();
        }
        //查询图书借还表
        public DataSet selectBorrowReturn(string BookId)
        {
            return borrowReturn_dal.selectBorrowReturn(BookId);
        }

        //读者借阅记录
        public DataSet ReaderBorrowReturn(string ReaderId)
        {
            return borrowReturn_dal.ReaderBorrowReturn(ReaderId);
        }

        //读者历史借阅记录
        public DataSet ReaderBorrowReturn1(string ReaderId)
        {
            return borrowReturn_dal.ReaderBorrowReturn1(ReaderId);
        }

        //还书
        public int ReturnBook(int BorrowReturnId)
        {
            return borrowReturn_dal.ReturnBook(BorrowReturnId);
        }

        //借书
        public int BorrowBook(BorrowReturn b)
        {
            return borrowReturn_dal.BorrowBook(b);
        }

        //续借
        public int RenewBook(BorrowReturn b)
        {
            return borrowReturn_dal.RenewBook(b);
        }
    }
}
