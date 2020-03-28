using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using Model;
using System.Data;
namespace BLL
{
    public class ReaderType_BLL
    {
        ReaderType_DAL reader_dal = new ReaderType_DAL();
        //查询全部的读者类型
        public List<ReaderType> selectReaderType()
        {
            return reader_dal.selectReaderType();
        }

        //查询全部的读者类型
        public DataSet selectReaderType1()
        {
            return reader_dal.selectReaderType1();
        }


        //添加读者类型
        public int addReaderType(ReaderType r)
        {
            return reader_dal.addReaderType(r);
        }

        //删除读者类型
        public int deleteReader(int ReaderTypeId)
        {
            return reader_dal.deleteReader(ReaderTypeId);
        }

        //修改读者类型
        public int updateReaderType(ReaderType r)
        {
            return reader_dal.updateReaderType(r);
        }
    }
}
