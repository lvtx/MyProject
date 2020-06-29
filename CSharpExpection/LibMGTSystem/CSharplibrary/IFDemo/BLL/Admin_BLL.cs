﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using DAL;
using System.Data;

namespace BLL
{
    public class Admin_BLL
    {
        Admin_DAL aa = new Admin_DAL();

        //删除管理员
        public int deleteAdmin(String LoginId)
        {
            return aa.deleteAdmin(LoginId);
        }
        //修改管理员
        public int ExitAdmin(Admin a)
        {
            return aa.ExitAdmin(a);
        }
        //添加管理员
        public int AddAdmin(Admin a)
        {
            return aa.AddAdmin(a);
        }
        public DataSet selectAdmin()
        {
            return aa.selectAdmin();
        }


        public int Scalar(Admin a)
        {
            return aa.Scalar(a);
        }
    }
}
