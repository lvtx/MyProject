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
    public class ReaderType_DAL
    {
        //查询全部的读者类型
        public List<ReaderType> selectReaderType()
        {
            List<ReaderType> list = new List<ReaderType>();
            string sql = @"select * from ReaderType";
            SqlDataReader reader = DBhelp.Create().ExecuteReader(sql);
            while (reader.Read())
            {
                ReaderType type = new ReaderType();
                type.ReaderTypeId = reader.GetInt32(0);
                type.ReaderTypeName = reader.GetString(1);
                list.Add(type);
            }
            reader.Close();
            return list;
        }
        //查询全部的读者类型
        public DataSet selectReaderType1()
        {
            string sql = @"select * from ReaderType";
            return DBhelp.Create().ExecuteAdater(sql);
        }

        //添加读者类型
        public int addReaderType(ReaderType r)
        {
            string sql = "proc_addReaderType";
            SqlParameter[] sp ={
                                   new SqlParameter("@ReaderTypeId",DbType.Int32),
                                   new SqlParameter("@ReaderTypeName",r.ReaderTypeName),
                                   new SqlParameter("@ReturnValue",DbType.Int32)
                              };
            sp[0].Direction = ParameterDirection.Output;
            sp[2].Direction = ParameterDirection.ReturnValue;
            DBhelp.Create().ExecuteNonQuery(sql, CommandType.StoredProcedure, sp);
            r.ReaderTypeId = (int)sp[0].Value;
            return (int)sp[2].Value;
        }
        //删除读者类型
        public int deleteReader(int ReaderTypeId)
        {
            string sql = @"delete from BorrowReturn where ReaderId in(select ReaderId from Reader where ReaderTypeId=@ReaderTypeId)
                            delete from Reader where ReaderTypeId=@ReaderTypeId
                            delete from ReaderType where ReaderTypeId=@ReaderTypeId";
            SqlParameter[] sp ={
                                   new SqlParameter("@ReaderTypeId",ReaderTypeId)
                              };
            return DBhelp.Create().ExecuteNonQuery(sql, sp: sp);
        }

        //修改读者类型
        public int updateReaderType(ReaderType r)
        {
            string sql = "update ReaderType set ReaderTypeName=@ReaderTypeName where ReaderTypeId=@ReaderTypeId";
            SqlParameter[] sp ={
                                   new SqlParameter("@ReaderTypeId",r.ReaderTypeId),
                                   new SqlParameter("@ReaderTypeName",r.ReaderTypeName)
                              };
            return DBhelp.Create().ExecuteNonQuery(sql, sp: sp);
        }
    }
}
