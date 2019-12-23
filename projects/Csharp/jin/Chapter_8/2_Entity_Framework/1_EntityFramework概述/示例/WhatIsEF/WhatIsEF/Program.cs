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
            //UnderstandUsingKeyWord();
            //FetchDataUseUsingKeyWord();
            HowLINQIsWorking();
            Console.ReadKey();
        }
        /// <summary>
        /// 使用LINQ直接查询数据库
        /// </summary>
        private static void FetchData()
        {
            var context = new MyDBContext();
            //使用LINQ to Entities查询数据
            var query = from client in context.OrderClients
                        select client;
            //循环遍历结果集
            foreach (var client in query)
            {
                Console.WriteLine("{0}:{1}", client.ClientName,
                    client.Address);
            }
            //关闭数据库连接
            context.Dispose();
        }
        /// <summary>
        /// 使用using关键字，在退出using结构时，自动调用它所监控的对象的Dispose()方法
        /// </summary>
        private static void UnderstandUsingKeyWord()
        {
            try
            {

                using (var obj = new MyClass())
                {
                    throw new Exception("Error!");
                    obj.printMessage();
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }
            
          
        }

        /// <summary>
        /// 使用using关键字让EF自动关闭数据库连接
        /// </summary>
        private static void FetchDataUseUsingKeyWord()
        {
            //using结构所监控的对象，必须实现IDisposable接口
            using (var context = new MyDBContext())
            {
                //使用LINQ to Entities查询数据
                var query = from client in context.OrderClients
                            select client;
                //循环遍历结果集
                foreach (var client in query)
                {
                    Console.WriteLine("{0}:{1}", client.ClientName,
                        client.Address);
                }
            }
        }
        /// <summary>
        /// 展示EF如何与数据库交互的
        /// </summary>
        private static void HowLINQIsWorking()
        {
            //using结构所监控的对象，必须实现IDisposable接口
            using (var context = new MyDBContext())
            {
                //指示要记录EF所进行的所有数据库相关活动
                //Log属性是一个Action<String>委托
                //我们可以把任意一个符合此委托的方法传给它,
                //从而以自己的方式处理这些数据库活动记录
                //本例中使用Console.WriteLine方法,
                //表明希望在控制台窗口中输出这些信息
                context.Database.Log = Console.WriteLine;
                //使用LINQ to Entities查询数据
                var query = from client in context.OrderClients
                            select client;
                //循环遍历结果集
                foreach (var client in query)
                {
                    Console.WriteLine("{0}:{1}", client.ClientName,
                        client.Address);
                }
            }
        }

    }

    class MyClass : IDisposable
    {
        public void printMessage()
        {
            Console.WriteLine("MyClass对象的HashCode={0}",
                this.GetHashCode());
        }
        public void Dispose()
        {
            Console.WriteLine("MyClass Object is Disposed.");
        }
    }
}
