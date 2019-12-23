using CRUD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUDTest
{
    public class ManyToManyHelper
    {

        private static Random ran = new Random();
        /// <summary>
        /// 创建一个用于测试的User对象
        /// </summary>
        /// <returns></returns>
        public static User CreateUser()
        {
            User user = new User { UserName = "User" + ran.Next(1, 100) };
            return user;
        }
        /// <summary>
        /// 创建一个用于测试的Role对象
        /// </summary>
        /// <returns></returns>
        public static Role CreateRole()
        {
            Role role = new Role { RoleName = "Role" + ran.Next(1, 100) };
            return role;
        }
        /// <summary>
        /// 删除所有数据
        /// </summary>
        public static void DeleteAllRecord()
        {
            using (var context = new MyDBContext())
            {
                context.Database.ExecuteSqlCommand("Delete from [User]");
                context.Database.ExecuteSqlCommand("Delete from Role");
                context.Database.ExecuteSqlCommand("Delete from UserInRole");
            }
        }
        /// <summary>
        /// 向数据库中添加初始记录（这是一种最规范的添加多对多关联的方式）
        /// </summary>
        public static void Seed()
        {
            using (var context = new MyDBContext())
            {
                DeleteAllRecord();
                //添加管理员角色
                Role role = new Role { RoleName = "SystemAdmin" };
                context.Roles.Add(role);

                //管理员角色有三个用户
                User user = null;
                for (int i = 0; i < 3; i++)
                {
                    user = new User { UserName = "SystemAdmin" + i };
                    //设定其角色为管理员
                    user.Roles.Add(role);
                    //加入到用户DbSet中
                    context.Users.Add(user);
                }

                //添加游客角色
                role = new Role { RoleName = "Guest" };
                context.Roles.Add(role);

                //游客角色有三个用户
                for (int i = 0; i < 3; i++)
                {
                    user = new User { UserName = "Guest" + i };
                    //设定其角色为游客
                    user.Roles.Add(role);
                    //加入到用户DbSet中
                    context.Users.Add(user);
                }

                context.SaveChanges();
                Console.WriteLine("测试数据己创建完毕");

            }
        }


        /// <summary>
        /// 显示数据库中的所有记录
        /// </summary>
        public static void ShowRecords()
        {
            using (var context = new MyDBContext())
            {
                ShowSystemAdmin(context);
                ShowGuest(context);
            }
        }


        public static void ShowGuest(MyDBContext context)
        {
            Console.WriteLine("游客");
            var query = from admin in context.Users
                        where admin.Roles.FirstOrDefault(r => r.RoleName == "Guest") != null
                        select admin;
            foreach (var user in query)
            {
                Console.WriteLine("\t" + user.UserName);
            }
        }

        public static void ShowSystemAdmin(MyDBContext context)
        {
            Console.WriteLine("管理员");
            var query = from admin in context.Users
                        where admin.Roles.FirstOrDefault(r => r.RoleName == "SystemAdmin") != null
                        select admin;
            foreach (var user in query)
            {
                Console.WriteLine("\t" + user.UserName);
            }

        }

    }
}
