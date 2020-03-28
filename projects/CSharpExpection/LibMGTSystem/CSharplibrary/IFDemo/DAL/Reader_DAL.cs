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
    public class Reader_DAL
    {
        //无条件查询读者信息
        public DataSet selectReader()
        {
            string sql = @"select ReaderId,ReaderName,TimeIn,TimeOut,ReaderTypeName,DepartmentName,ClassName,IdentityCard,Gender,Special,Phone,Email,Address,ReaderRemark from Reader 
                            inner join ReaderType on ReaderType.ReaderTypeId=Reader.ReaderTypeId
                            inner join Department on Department.DepartmentId=Reader.DepartmentId
                            inner join Class on Class.ClassId=Reader.ClassId";
            return DBhelp.Create().ExecuteAdater(sql);
        }

        //根据读者类型ID查询的读者信息
        public DataSet selectReader(int ReaderTypeId)
        {

            string sql = @"select ReaderId,ReaderName,TimeIn,TimeOut,ReaderTypeName,DepartmentName,ClassName,IdentityCard,Gender,Special,Phone,Email,Address,ReaderRemark from Reader 
                            inner join ReaderType on ReaderType.ReaderTypeId=Reader.ReaderTypeId
                            inner join Department on Department.DepartmentId=Reader.DepartmentId
                            inner join Class on Class.ClassId=Reader.ClassId
                            where ReaderType.ReaderTypeId=@ReaderTypeId ";
            SqlParameter[] sp ={
                                   new SqlParameter("@ReaderTypeId",ReaderTypeId)
                              };
            return DBhelp.Create().ExecuteAdater(sql, sp);
        }

        //根据ID查询的读者信息
        public List<Reader> selectReader1(string ReaderId)
        {

            string sql = @"select ReaderId,ReaderName,TimeIn,TimeOut,ReaderTypeId,DepartmentId,ClassId,IdentityCard,
                            Gender,Special,Phone,Email,Address,ReaderRemark from Reader";
            SqlParameter[] sp ={
                                   new SqlParameter("@ReaderId",ReaderId)
                              };
            List<Reader> list = new List<Reader>();
            SqlDataReader reader = DBhelp.Create().ExecuteReader(sql, sp);
            while (reader.Read())
            {
                Reader r = new Reader();
                r.ReaderId = reader.GetString(0);
                r.ReaderName = reader.GetString(1);
                r.TimeIn = reader.GetDateTime(2);
                r.TimeOut = reader.GetDateTime(3);
                r.ReaderTypeId = reader.GetInt32(4);
                r.DepartmentId = reader.GetInt32(5);
                r.ClassId = reader.GetInt32(6);
                r.IdentityCard = reader.GetString(7);
                r.Gender = reader.GetString(8);
                r.Special = reader.GetString(9);
                r.Phone = reader.GetString(10);
                r.Email = reader.GetString(11);
                r.Address = reader.GetString(12);
                r.ReaderRemark = reader.GetString(13);
                list.Add(r);
            }
            reader.Close();
            return list;

        }

        //根据查询内容和条件查询的读者信息
        public DataSet selectReader(string A, string B)
        {

            string sql = string.Format(@"select ReaderId,ReaderName,TimeIn,TimeOut,ReaderTypeName,DepartmentName,ClassName,IdentityCard,Gender,Special,Phone,Email,Address,ReaderRemark from Reader 
                            inner join ReaderType on ReaderType.ReaderTypeId=Reader.ReaderTypeId
                            inner join Department on Department.DepartmentId=Reader.DepartmentId
                            inner join Class on Class.ClassId=Reader.ClassId
                            where {0} like '%{1}%'", A, B);
            return DBhelp.Create().ExecuteAdater(sql);
        }


        //根据查询条件查询的读者信息
        public DataSet selectReader(List<string> list, string B)
        {
            string sql = "";
            for (int i = 0; i < list.Count; i++)
            {
                if (i != list.Count - 1)
                {
                    sql += string.Format(@"select ReaderId,ReaderName,TimeIn,TimeOut,ReaderTypeName,DepartmentName,ClassName,IdentityCard,Gender,Special,Phone,Email,Address,ReaderRemark from Reader 
                            inner join ReaderType on ReaderType.ReaderTypeId=Reader.ReaderTypeId
                            inner join Department on Department.DepartmentId=Reader.DepartmentId
                            inner join Class on Class.ClassId=Reader.ClassId
                            where {0} like '%{1}%' union  ", list[i], B);
                }
                else
                {
                    sql += string.Format(@"select ReaderId,ReaderName,TimeIn,TimeOut,ReaderTypeName,DepartmentName,ClassName,IdentityCard,Gender,Special,Phone,Email,Address,ReaderRemark from Reader 
                            inner join ReaderType on ReaderType.ReaderTypeId=Reader.ReaderTypeId
                            inner join Department on Department.DepartmentId=Reader.DepartmentId
                            inner join Class on Class.ClassId=Reader.ClassId
                            where {0} like '%{1}%' ", list[i], B);
                }

            }

            return DBhelp.Create().ExecuteAdater(sql);
        }

        //删除读者信息
        public int deleteReader(string ReaderId)
        {
            string sql = @"
                            delete from BorrowReturn where ReaderId=@ReaderId
                            delete from Reader where ReaderId=@ReaderId";
            SqlParameter[] sp ={
                                   new SqlParameter("@ReaderId",ReaderId)
                              };
            return DBhelp.Create().ExecuteNonQuery(sql, sp: sp);
        }

        //修改读者信息
        public int updateReader(Reader reader)
        {
            string sql = @"update Reader set ReaderName=@ReaderName,TimeIn=@TimeIn,TimeOut=@TimeOut,ReaderTypeId=@ReaderTypeId,
                            DepartmentId=@DepartmentId,ClassId=@ClassId,IdentityCard=@IdentityCard,Gender=@Gender,Special=@Special,
                            Phone=@Phone,Email=@Email,Address=@Address,ReaderRemark=@ReaderRemark where ReaderId=@ReaderId";
            SqlParameter[] sp ={
                                   new SqlParameter("@ReaderName",reader.ReaderName),
                                   new SqlParameter("@TimeIn",reader.TimeIn),
                                   new SqlParameter("@TimeOut",reader.TimeOut),
                                   new SqlParameter("@ReaderTypeId",reader.ReaderTypeId),
                                   new SqlParameter("@DepartmentId",reader.DepartmentId),
                                   new SqlParameter("@ClassId",reader.ClassId),
                                   new SqlParameter("@IdentityCard",reader.IdentityCard),
                                   new SqlParameter("@Gender",reader.Gender),
                                   new SqlParameter("@Special",reader.Special),
                                   new SqlParameter("@Phone",reader.Phone),
                                   new SqlParameter("@Email",reader.Email),
                                   new SqlParameter("@Address",reader.Address),
                                   new SqlParameter("@ReaderRemark",reader.ReaderRemark),
                                   new SqlParameter("@ReaderId",reader.ReaderId)
                              };
            return DBhelp.Create().ExecuteNonQuery(sql, sp: sp);
        }


        //添加读者信息
        public int addReader(Reader r)
        {
            string sql = "proc_AddReader";
            SqlParameter[] sp ={
                                   new SqlParameter("@ReaderId",r.ReaderId),
                                   new SqlParameter("@ReaderName",r.ReaderName),
                                   new SqlParameter("@TimeIn",r.TimeIn),
                                   new SqlParameter("@TimeOut",r.TimeOut),
                                   new SqlParameter("@ReaderTypeId",r.ReaderTypeId),
                                   new SqlParameter("@DepartmentId",r.DepartmentId),
                                   new SqlParameter("@ClassId",r.ClassId),
                                   new SqlParameter("@IdentityCard",r.IdentityCard),
                                   new SqlParameter("@Gender",r.Gender),
                                   new SqlParameter("@Special",r.Special),
                                   new SqlParameter("@Phone",r.Phone),
                                   new SqlParameter("@Email",r.Email),
                                   new SqlParameter("@Address",r.Address),
                                   new SqlParameter("@ReaderRemark",r.ReaderRemark),
                                   new SqlParameter("@ReturnValue",DbType.Int32)
                              };
            sp[sp.Length - 1].Direction = ParameterDirection.ReturnValue;
            DBhelp.Create().ExecuteNonQuery(sql, CommandType.StoredProcedure, sp);
            return (int)sp[sp.Length - 1].Value;
        }

        //返回读者编号，读者姓名
        public List<Reader> selectReaderId(string ReaderId)
        {
            string sql = "select ReaderId,ReaderName from Reader where ReaderId like '%'+@ReaderId+'%' ";
            SqlParameter[] sp ={
                                   new SqlParameter("@ReaderId",ReaderId)
                              };
            SqlDataReader reader = DBhelp.Create().ExecuteReader(sql, sp: sp);
            List<Reader> list = new List<Reader>();
            while (reader.Read())
            {
                Reader r = new Reader();
                r.ReaderId = reader["ReaderId"].ToString();
                r.ReaderName = reader["ReaderName"].ToString();
                list.Add(r);
            }
            reader.Close();
            return list;
        }
    }
}
