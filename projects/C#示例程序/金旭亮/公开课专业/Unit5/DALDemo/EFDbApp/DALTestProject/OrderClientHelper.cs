using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DALTestProject
{
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
    }
}
