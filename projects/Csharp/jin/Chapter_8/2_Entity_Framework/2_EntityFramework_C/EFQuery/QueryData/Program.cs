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
            Console.ReadLine();
        }
        static void FindSingle()
        {
            using (var context = new MyDBContext())
            {

                var query = (from client in context.OrderClients
                             where client.ClientID == -10
                             select client).SingleOrDefault();
                if (query == null)
                {
                    Console.WriteLine("没有找到");
                }
                else
                    Console.WriteLine("{0}:{1}", query.ClientName, query.Address);
            }
        }
        static void TestFindMethod()
        {
            using (var context = new MyDBContext())
            {
                context.Database.Log = Console.WriteLine;
                var Clients = from client in context.OrderClients
                              where client.ClientID > 10
                              select client;
                //将导致一条SQL命令的发送，提取记录数
                Console.WriteLine("共有{0}条记录", Clients.Count());
                //循环访问记录，将EF发送Select命令装载并自动跟踪相关记录
                //foreach (var client in Clients)
                //{
                //    Console.WriteLine("{0}：{1}",client.ClientName,client.Address);
                //}
                //ClientID=30的记录在内存中，因此，Find方法不会导致再次发送SQL命令
                Console.WriteLine("=========================================");
                var c = context.OrderClients.Find(30);
                if (c == null)
                {
                    Console.WriteLine("没有找到");
                }
                else
                {
                    Console.WriteLine("找到{0}：{1}", c.ClientName, c.Address);
                }
                var c2 = context.OrderClients.Find(2);
                if (c2 == null)
                {
                    Console.WriteLine("没有找到");
                }
                else
                {
                    Console.WriteLine("找到{0}：{1}", c2.ClientName, c2.Address);
                }
            }
        }
    }
}
