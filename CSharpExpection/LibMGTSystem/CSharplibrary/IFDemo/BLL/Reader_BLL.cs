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
    public class Reader_BLL
    {
        Reader_DAL reader_dal = new Reader_DAL();
        //无条件查询读者信息
        public DataSet selectReader()
        {
            return reader_dal.selectReader();
        }

        //根据ID查询全部的读者信息
        public DataSet selectReader(int ReaderTypeId)
        {
            return reader_dal.selectReader(ReaderTypeId);
        }

        //根据查询内容和条件查询的读者信息
        public DataSet selectReader(string A, string B)
        {
            return reader_dal.selectReader(A, B);
        }

        //根据查询条件查询的读者信息
        public DataSet selectReader(List<string> list, string B)
        {
            return reader_dal.selectReader(list, B);
        }

        //删除读者信息
        public int deleteReader(string ReaderId)
        {
            return reader_dal.deleteReader(ReaderId);
        }

        //根据ID查询的读者信息
        public List<Reader> selectReader1(string ReaderId)
        {
            return reader_dal.selectReader1(ReaderId);
        }

        //修改读者信息
        public int updateReader(Reader reader)
        {
            return reader_dal.updateReader(reader);
        }

        //添加读者信息
        public int addReader(Reader r)
        {
            return reader_dal.addReader(r);
        }

        //返回读者编号，读者姓名
        public List<Reader> selectReaderId(string ReaderId)
        {
            return reader_dal.selectReaderId(ReaderId);
        }
    }
}
