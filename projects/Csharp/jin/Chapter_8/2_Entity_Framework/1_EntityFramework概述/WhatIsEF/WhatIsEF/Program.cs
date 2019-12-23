using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhatIsEF
{
    class Program
    {
        static void Main(string[] args)
        {
            //FetchData();
            //ThisIsUsing();
            //FetchDataExpandUsing();
            HowLINQIsWorking();
            Console.ReadLine();
        }
        #region "1.提取OrderClients表中的数据"
        private static void FetchData()
        {
            //LINQ to Entities查询数据
            MyDBContext context = new MyDBContext();
            var query = from client in context.OrderClients
                    select client;
            foreach (var client in query)
            {
                Console.WriteLine("{0}:{1}",
                    client.ClientName,client.Address);
            }
            //关闭数据库连接
            context.Dispose();
        }
        #endregion

        #region "2.using 关键字"
        private static void ThisIsUsing()
        {
            try
            {               
                using (var myClass = new MyClass())
                {
                    throw new Exception("Error!");
                    myClass.PrintMessage();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);               
            }
        }
        #endregion

        #region "3.拓展 使用Using关键字"
        private static void FetchDataExpandUsing()
        {
            using(var context = new MyDBContext())
            {
                var query = from client in context.OrderClients
                            select client;
                foreach (var client in query)
                {
                    Console.WriteLine(client.Address, client.ClientName);
                }
            }
        }
        #endregion

        #region "4.LINQ是如何工作的"
        private static void HowLINQIsWorking()
        {
            using (var context = new MyDBContext())
            {
                context.Database.Log = Console.WriteLine;
                var query = from client in context.OrderClients
                            select client;
                foreach (var client in query)
                {
                    Console.WriteLine("{0}:{1}",client.ClientName,client.Address);
                }
            }
        }
        #endregion
    }
    #region "2.using 关键字，定义MyClass类"
    class MyClass : IDisposable
    {
        public void PrintMessage()
        {
            Console.WriteLine("这是我的类");
        }
        public void Dispose()
        {
            Console.WriteLine("关闭了这个类");
        }
    }
    #endregion
}
