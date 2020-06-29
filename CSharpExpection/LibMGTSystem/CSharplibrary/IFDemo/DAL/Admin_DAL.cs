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
    public class Admin_DAL
    {
        /// <summary>
        /// 查询全部
        /// </summary>
        /// <returns></returns>
        public DataSet selectAdmin()
        {
            string sql = @"select * from Admin";
            return DBhelp.Create().ExecuteAdater(sql);
        }
        /// <summary>
        /// 删除管理员
        /// </summary>
        /// <param name="LoginId"></param>
        /// <returns></returns>
        public int deleteAdmin(string LoginId)
        {
            string sql = "delete from Admin where LoginId=@LoginId";
            SqlParameter[] sp ={
                                  new SqlParameter("@LoginId",LoginId)
                              };
            return DBhelp.Create().ExecuteNonQuery(sql, sp: sp);
        }
        /// <summary>
        /// 修改管理员
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public int ExitAdmin(Admin a)
        {
            string sql = "update Admin set LoginId=@LoginId,LoginPwd=@LoginPwd,LoginType=@LoginType,LoginRemark=@LoginRemark where LoginId=@LoginId";
            SqlParameter[] sp = { 
                                new SqlParameter("LoginId",a.LoginId),
                                new SqlParameter("LoginPwd",a.LoginPwd),
                                new SqlParameter("LoginType",a.LoginType),
                                new SqlParameter("LoginRemark",a.LoginRemark)
                                };
            return DBhelp.Create().ExecuteNonQuery(sql, sp: sp);
        }
        /// <summary>
        /// 添加管理员
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public int AddAdmin(Admin a)
        {
            string sql = "insert into Admin select @LoginId,@LoginPwd,@LoginType,@LoginRemark";
            SqlParameter[] sp ={
                                new SqlParameter("LoginId",a.LoginId),
                                new SqlParameter("LoginPwd",a.LoginPwd),
                                new SqlParameter("LoginType",a.LoginType),
                                new SqlParameter("LoginRemark",a.LoginRemark)
                              };
            return DBhelp.Create().ExecuteNonQuery(sql, sp: sp);
        }

        /// <summary>
        /// 查询相同用户
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public int Scalar(Admin a)
        {
            string sql = "select COUNT(*) from Admin where LoginId=@LoginId and LoginPwd=@LoginPwd and LoginType=@LoginType";
            SqlParameter[] sp ={
                                new SqlParameter("@LoginId",a.LoginId),
                                new SqlParameter("@LoginPwd",a.LoginPwd),
                                new SqlParameter("@LoginType",a.LoginType)
                              };
            return DBhelp.Create().ExecuteScalar(sql, sp);
        }
    }
}
