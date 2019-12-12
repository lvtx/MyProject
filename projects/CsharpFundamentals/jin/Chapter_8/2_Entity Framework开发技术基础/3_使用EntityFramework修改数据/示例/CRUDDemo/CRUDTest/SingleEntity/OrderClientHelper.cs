using CRUD;
using System;
using System.Data.Entity.Infrastructure;

namespace EntityAssociationTest.SingleEntity
{
    /// <summary>
    /// 一个为单元测试提供辅助功能的类
    /// </summary>
    public class OrderClientHelper
    {
        public static Random ran = new Random();
        public static OrderClient CreateOrderClient()
        {
            String clientName = "客户" + ran.Next(1, 10000);
            OrderClient client = new OrderClient()
            {
                ClientName = clientName,
                Address = clientName + "的地址",
                Email = clientName + "的电子邮件",
                PostCode = ran.Next(100000, 999999).ToString(),
                Telephone = "1" + ran.Next(10000, 99999) + ran.Next(10000, 99999)

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
