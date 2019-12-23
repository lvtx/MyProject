using CRUD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUDTest.SingleEntity
{
    public static class OrderClientHelper
    {
        public static OrderClient CreateOrderClient()
        {
            Random ran = new Random();
            string clientName = "客户" + ran.Next(1, 100000000);
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
    }
}
