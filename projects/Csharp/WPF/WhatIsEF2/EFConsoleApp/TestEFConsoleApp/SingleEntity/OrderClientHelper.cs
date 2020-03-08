using EFConsoleApp;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestEFConsoleApp.SingleEntity
{
    public class OrderClientHelper
    {
        /// <summary>
        /// 打印出所有的用户名和地址
        /// </summary>
        /// <param name="client"></param>
        public static void ShowClient(OrderClient client)
        {
            if (client != null)
            {
                Console.WriteLine("--------------------------------------------------------");
                Console.WriteLine("{0}: {1}", client.ClientName, client.Address);
                Console.WriteLine("--------------------------------------------------------");
            }
        }
        /// <summary>
        /// 使用随机数创建一个新客户
        /// </summary>
        /// <returns></returns>
        public static OrderClient CreateOrderClient()
        {
            Random random = new Random();
            string clientName = "客户" + random.Next(1, 10000);
            OrderClient client = new OrderClient()
            {
                ClientName = clientName,
                Address = clientName + "的地址",
                PostCode = random.Next(100000, 999999).ToString(),
                Telephone = "1" + random.Next(10000, 99999) + random.Next(10000, 99999),
                Email = clientName + "的电子邮件"
            };
            return client;
        }
        /// <summary>
        /// 打印实体对象状态值
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="PropertyName"></param>
        public static void PrintEntityPropertyValue(DbEntityEntry entry, String PropertyName)
        {
            Console.WriteLine("CurrentValue:{0}\nOriginalValue:{1}\nDatabaseValue:{2}",
                entry.CurrentValues[PropertyName],
                entry.OriginalValues[PropertyName],
                entry.GetDatabaseValues()[PropertyName]);
        }
    }
}
