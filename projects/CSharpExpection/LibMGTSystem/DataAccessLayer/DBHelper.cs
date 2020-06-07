using System;
using LibraryModel;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Principal;
using System.Windows.Forms;
using System.Data.Entity;

namespace DataAccessLayer
{
    public class DBHelper
    {
        public static async Task<int> Login(Admin admin)
        {
            using(var context = new LibraryEntities())
            {
                Admin _admin = await context.Admin.FindAsync(admin);
                if (_admin == null)
                {
                    MessageBox.Show("用户不存在");
                    return 0;
                }
                else
                {
                    if(_admin.LoginPwd != admin.LoginPwd)
                    {
                        MessageBox.Show("密码错误");
                        return 0;
                    }
                }
                return 1;
            }
        }
        /// <summary>
        /// 获取读者类型
        /// </summary>
        /// <returns></returns>
        public static List<RoleType> GetReaderType()
        {
            List<RoleType> readerTypes = new List<RoleType>();
            using (var context = new LibraryEntities())
            {
                IQueryable<RoleType> RoleTypes = context.GetReaderType();
                
                foreach (var roleType in RoleTypes)
                {
                    readerTypes.Add(roleType);
                }
            };
            return readerTypes;
            //return readerTypes;
        }
    }
}
