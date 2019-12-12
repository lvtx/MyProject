using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryData
{
    class Program
    {
        static void Main(string[] args)
        {
            //FindSingle();
            TestFindMethod();
            Console.ReadKey();
        }
        /// <summary>
        /// 混用LINQ和扩展方法查找单条记录
        /// </summary>
        static void FindSingle()
        {
            using (var context = new MyDBContext())
            {
                //查找ID=10的客户信息，找不到,返回null
                var client = (from c in context.OrderClients
                              where c.ClientID==-10
                              select c).SingleOrDefault();
                if (client == null)
                {
                    Console.WriteLine("没有找到");
                }
                else
                {
                    Console.WriteLine("{0} 住在:{1}",client.ClientName,client.Address);
                }
            }
        }
        /// <summary>
        /// 展示Find方法的特点
        /// </summary>
        static void TestFindMethod()
        {
            using (var context = new MyDBContext())
            {
                context.Database.Log = Console.WriteLine;
                //提取所有ClientID大于10的客户记录
                var allClient = from client in context.OrderClients
                                where client.ClientID>10
                                select client;

                //将导致一条SQL命令的发送，提取记录数
                Console.WriteLine("共有{0}条记录",allClient.Count());
                //循环访问记录，将EF发送Select命令装载并自动跟踪相关记录
                foreach (var item in allClient)
                {
                    Console.WriteLine("ClientID：{0}, ClientName:{1}",item.ClientID,item.ClientName);
                }
                //ClientID=30的记录在内存中，因此，Find方法不会导致再次发送SQL命令
                var c = context.OrderClients.Find(30);
                
                if (c == null)
                {
                    Console.WriteLine("没有找到");
                }
                else
                {
                    Console.WriteLine("{0}(ClientID={1}) 住在:{2}",
                        c.ClientName,c.ClientID, c.Address);
                }
                //EF发现ClientID=2的记录不在内存中，
                //因此，Find方法会发送SQL命令去原始数据库中查询
                c = context.OrderClients.Find(2);
                if (c == null)
                {
                    Console.WriteLine("没有找到");
                }
                else
                {
                    Console.WriteLine("{0}(ClientID={1}) 住在:{2}",
                       c.ClientName, c.ClientID, c.Address);
                }
            }
        }            
    }
}
