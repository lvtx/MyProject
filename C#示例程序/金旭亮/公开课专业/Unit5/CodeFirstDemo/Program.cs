using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFirstDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            //AddBlog();

            //AddUser();

            ShowData();
            Console.ReadKey(true);

        }
        /// <summary>
        /// 显示数据
        /// </summary>
        private static void ShowData()
        {
            using (BloggingContext db = new BloggingContext())
            {
                // Display all Blogs from the database
                var query = from b in db.Blogs
                            orderby b.Name
                            select b;

                Console.WriteLine("All blogs in the database:");
                foreach (var item in query)
                {
                    Console.WriteLine(item.Name);
                }

                Console.WriteLine("Users in the database:");
                foreach (var user in db.Users)
                {
                    Console.WriteLine(user.Username);
                }
            }
        }
        /// <summary>
        /// 创建新用户
        /// </summary>
        private static void AddUser()
        {
            using (BloggingContext db = new BloggingContext())
            {
                User user = new User();
                Random ran = new Random();
                user.Username = "user" + ran.Next(1, 1000);
                user.DisplayName = "display name for " + user.Username;
                db.Users.Add(user);

                db.SaveChanges();
            }
        }
        /// <summary>
        /// 新加BLOG，如果数据库不存在，则创建之
        /// </summary>
        private static void AddBlog()
        {
            using (BloggingContext db = new BloggingContext())
            {
                Console.Write("Enter a name for a new Blog: ");
                var name = Console.ReadLine();
                var blog = new Blog { Name = name };
                db.Blogs.Add(blog);
                db.SaveChanges();
                Console.WriteLine("数据库创建完毕，记录己插入。");
            }
        }
    }


}
