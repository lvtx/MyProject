using System;
using Model;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class DBHelper
    {
        public static int Login(Admin admin)
        {
            using(var context = new LibraryEntities())
            {
                Admin _admin = (from a in context.Admins
                                      where a.LoginId == admin.LoginId
                                      select a).FirstOrDefault();
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
    }
}
