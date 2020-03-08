using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace WhatIsEF
{
    class Program
    {
        static void Main(string[] args)
        {
            //ShowProducts();
            //UseDatabaseLog();
            //ShowProductsUseAsync();
            UseStoreProcedures();
            //UseView();
            Console.ReadKey();
        }

        static void ShowProducts()
        {
            using (var context = new NorthwindEntities())
            {
                int count = 0;
                //使用LINQ to Entities查询数据
                //var query = from p in context.Products
                //            select p;

                //也可以使用护展方法查询数据
                var query = context.Products.Where(p => p.ProductName.Length > 1);
                foreach (var product in query)
                {
                    Console.WriteLine(product.ProductName);
                    count++;
                }

                Console.WriteLine("共有{0}个产品", count);
            }
        }
        static async void ShowProductsUseAsync()
        {
            using (var context = new NorthwindEntities())
            {
                int count = 0;
                await context.Products.ForEachAsync((product) =>
                {
                    Console.WriteLine(product.ProductName);
                    count++;
                });
                Console.WriteLine("共有{0}个产品", count);
            }
        }

        static void UseDatabaseLog()
        {
            using (var context = new NorthwindEntities())
            {
                //指示要记录EF所进行的所有数据库相关活动
                //Log属性是一个Action<string>类型委托
                //我们可以把任意一个符合此委托的方法传给它，
                //从而以自己的方式处理这些数据库活动记录
                //使用Console.WriteLine方法，
                //表明要在控制台窗口中输出这些信息
                context.Database.Log = Console.WriteLine;

                var query = from p in context.Products
                            select p;
                //ToList()方法将导致EF向数据库发送SQL命令
                List<Product> products = query.ToList();
                Console.WriteLine("产品数据提取完毕");

            }
        }
        /// <summary>
        /// 使用数据库中的视图
        /// </summary>
        static void UseView()
        {
            using (var context = new NorthwindEntities())
            {
                foreach (var product in context.Current_Product_Lists)
                {
                     Console.WriteLine("产品ID:{0}  名称：{1}",product.ProductID,product.ProductName);
                }
            }
        }
        /// <summary>
        /// 使用数据库中的存储过程
        /// </summary>
        static void UseStoreProcedures()
        {
            using (var context = new NorthwindEntities())
            {
                Console.Write("请输入ProductId（大于0的整数）：");
                int id = int.Parse(Console.ReadLine());
                var product = context.GetProduct(id).FirstOrDefault();
                if (product != null)
                {
                    Console.WriteLine("产品名称：{0}",product.ProductName);
                }
            }
        }
    }
}
