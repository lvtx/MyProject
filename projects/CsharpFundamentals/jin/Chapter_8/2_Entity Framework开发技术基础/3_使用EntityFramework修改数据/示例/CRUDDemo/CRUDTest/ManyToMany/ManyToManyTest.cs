using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CRUD;
using System.Linq;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace CRUDTest.ManyToMany
{
    [TestClass]
    public class ManyToManyTest
    {
        private TestContext testContextInstance;
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }
        [ClassCleanup()]
        public static void MyClassCleanup() {
            //测试结束时，自动地移除所有记录
            ManyToManyHelper.DeleteAllRecord();
        }
      
        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext) {
            //开始测试时，自动创建测试用的数据
            ManyToManyHelper.Seed();
        }
        

        #region "Add"
        /// <summary>
        /// 在内存中创建两个用户和一个角色，让这两个用户属于这个角色
        /// 注意：这种编程方式不推荐，这里只是用于演示EF的特性
        /// 推荐的创建多对多关联的实例，请参看
        /// ManyToManyHelper.Seed()方法
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public void TestAddUserAndRoleAllInMemory()
        {
            using (var context = new MyDBContext())
            {
                User user = ManyToManyHelper.CreateUser();
                User user2 = ManyToManyHelper.CreateUser();
                Role role = ManyToManyHelper.CreateRole();
              
                //不管在哪一端添加对象，都可以达到添加目的

                //在User端，给User添加角色
                user.Roles.Add(role);
                //在Role端，给角色添加用户
                role.Users.Add(user);
                role.Users.Add(user2);
               
                //将Role加入到DbSet，Save之后，
                //它将会在User表和UserInRole表中同时创建两个用户和一个角色
                context.Roles.Add(role);

                //如果使用Users，则必须把两个用户都追加到DbSet中，
                //才能把两个用户和它们所属的角色保存到数据库表中
                //context.Users.Add(user);
                //context.Users.Add(user2);
                int result = context.SaveChanges();

                Console.WriteLine("result="+result);
                Assert.IsTrue(result > 0);
            }
        }
        #endregion

        #region "修改多对多对象之间的关联"
        /// <summary>
        /// 将一个新用户加入到现有角色中
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public void TestAddUserToRole()
        {
            using (var context = new MyDBContext())
            {
                Role admin = context.Roles
                        .FirstOrDefault(
                             r => r.RoleName == "SystemAdmin"
                        );
                if (admin != null)
                {
                    //添加一个系统管理员用户
                    User user = ManyToManyHelper.CreateUser();  
                    admin.Users.Add(user);
                    //保存修改，注意发出的SQL命令
                    int result=context.SaveChanges();
                    Assert.IsTrue(result > 0);
                    
                    //验证用户己经加入到系统管理员角色中……
                    Role adminFromDB =  context.Roles
                                    .FirstOrDefault(
                                            r => r.RoleName == "SystemAdmin"
                                    );
                    Assert.IsNotNull(adminFromDB.Users
                        .FirstOrDefault(u => u.UserId == user.UserId));
                    Console.WriteLine(user.UserName + "己加入到SystemAdmin角色中");
                    ManyToManyHelper.ShowSystemAdmin(context);
                }

            }
        }
       

        /// <summary>
        ///  给一个现有用户添加新角色（角色己经存在了）
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public void TestAssignRoleToUser()
        {
            using (var context = new MyDBContext())
            {
                Role admin = context.Roles.FirstOrDefault(
                    r => r.RoleName == "SystemAdmin");
                Role guest = context.Roles.FirstOrDefault(
                    r => r.RoleName == "Guest");
                User user = context.Users.FirstOrDefault();
                if (user.Roles.Count == 0)
                {
                    user.Roles.Add(admin);
                    user.Roles.Add(guest);
                }
                else
                {
                    if (user.Roles.Count == 1)
                    {
                        if (user.Roles.ElementAt(0).RoleName == "SystemAdmin")
                        {
                            user.Roles.Add(guest);
                        }
                        else
                            user.Roles.Add(admin);
                    }

                }
                int result=context.SaveChanges();
                Assert.IsTrue(result > 0);
                Console.WriteLine("给" + user.UserName + "添加了SystemAdmin和Guest两个角色");
               ManyToManyHelper.ShowRecords();
            }
        }

        #endregion

        #region "Delete"
        /// <summary>
        /// 将一个用户从角色中移除
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public void TestRemoveUserFromRole()
        {
            using (var context = new MyDBContext())
            {
                Role admin =  context.Roles.FirstOrDefault(
                    r => r.RoleName == "SystemAdmin");
                if (admin != null)
                {
                    User user = admin.Users.FirstOrDefault();
                    if (user != null)
                    {
                        admin.Users.Remove(user);
                        int result = context.SaveChanges();
                        Assert.IsTrue(result > 0);
                        Console.WriteLine(user.UserName + "己从SystemAdmin角色中移除");
                        ManyToManyHelper.ShowSystemAdmin(context);
                    }
                }
            }

        }
        /// <summary>
        /// 删除一个角色
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public void DeleteSystemAdminRole()
        {
            using (var context = new MyDBContext())
            {
                Role systemAdmin = context.Roles.Include("Users")
                    .FirstOrDefault(r => r.RoleName == "SystemAdmin");
                if (systemAdmin != null)
                {
                    List<User> users= systemAdmin.Users.ToList();
                    Console.WriteLine("{0}中包容用户{1}个",systemAdmin.RoleName,
                      users.Count);
                    
                    for (int i = 0; i < users.Count; i++)
                    {
                        Console.WriteLine("\t{0}",systemAdmin.Users.ElementAt(i).UserName);
                    }
                    //删除“系统管理员”这个角色
                    context.Roles.Remove(systemAdmin);

                    int result = context.SaveChanges();
                    Assert.IsTrue(result == users.Count + 1);

                    //确认Role表中确实没有SystemAdmin的记录了
                    Role roleFromDB = context.Roles.FirstOrDefault(
                        r => r.RoleName == "SystemAdmin");
                    Assert.IsNull(roleFromDB);
                   
                   //确认用户表中的记录仍然存在
                    User temp = null;
                    for (int i = 0; i < users.Count; i++)
                    {
                        int id=users[i].UserId;
                        temp =  context.Users.FirstOrDefault(
                            u => u.UserId == id);
                        Assert.IsNotNull(temp);

                    }

                }
            }
        }
        #endregion

    }
}
