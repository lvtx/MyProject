using CRUD;
using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CRUDTest.SingleEntity
{
    [TestClass]
    public class OrderClientTest
    {
        [TestMethod]
        public void TestAdd()
        {
            using (var context = new MyDBContext())
            {
                var client = OrderClientHelper.CreateOrderClient();
                context.OrderClients.Add(client);
                int result = context.SaveChanges();
                Assert.IsTrue(result == 1);
                Console.WriteLine(client);
                OrderClient clientFromDB = context.OrderClients
                    .FirstOrDefault(c => c.ClientID == client.ClientID);
                Assert.IsNotNull(clientFromDB);
                Assert.IsTrue(client.Equals(clientFromDB));
            }
        }

        [TestMethod]
        public void TestModify()
        {
            using (var context = new MyDBContext())
            {
                //EF不支持last先降序排列再选出第一个
                var client = (from c in context.OrderClients
                              orderby c.ClientID descending
                              select c).FirstOrDefault();
                if (client != null)
                {
                    //把邮编当成数字，加1，作为修改后的新值进行测试
                    int value = int.Parse(client.PostCode) + 1;
                    client.PostCode = value.ToString();
                    int result = context.SaveChanges();
                    Assert.IsTrue(result == 1);
                }
                //将数据提取出来验证
                var clientFromDB = context.OrderClients.FirstOrDefault(
                    c => c.PostCode == client.PostCode);
                Assert.IsTrue(clientFromDB.Equals(client));
            }
        }

        [TestMethod]
        public void TestDelete()
        {
            using (var context = new MyDBContext())
            {
                var clients = (from c in context.OrderClients
                               select c).FirstOrDefault(c => c.ClientName == "张均已");
                context.OrderClients.Remove(clients);
                int result = context.SaveChanges();
                Assert.IsTrue(result == 1);
            }        
        }

        [TestMethod]
        public void TestEntityState()
        {

        }
    }
}
