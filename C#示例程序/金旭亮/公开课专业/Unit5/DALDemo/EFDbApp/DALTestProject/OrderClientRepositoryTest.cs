using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DataAccessLayer;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Linq;
using Model;

namespace DALTestProject
{
    [TestClass]
    public class OrderClientRepositoryTest
    {
        OrderClientRepository repo = null;

        [TestInitialize]
        public void TestInitialize()
        {
            repo = new OrderClientRepository();
        }


        [TestMethod]
        public async Task TestGetAllClientsAsync()
        {
            var Clients = await repo.GetAllClientsAsync();
            Assert.IsTrue(Clients.Count() > 0);
        }

        [TestMethod]
        public async Task TestAddClient()
        {
            OrderClient client = OrderClientHelper.CreateOrderClient();
            repo.AddClient(client);
            int result = await repo.SaveChangesAsync();
            Assert.IsTrue(result == 1);
        }

        [TestMethod]
        public async Task TestDeleteClient()
        {
            var Clients = await repo.GetAllClientsAsync();
            repo.DeleteClient(Clients.Last().ClientID);
            int result = await repo.SaveChangesAsync();
            Assert.IsTrue(result == 1);
        }

        [TestMethod]
        public async Task TestModifyClient()
        {
            var clients = await repo.GetAllClientsAsync();
            OrderClient client = clients[0];
            //修改邮编
            int oldPostCode = int.Parse(client.PostCode);
            client.PostCode = (oldPostCode + 1).ToString();
            repo.ModifyClient(client);

            int result = await repo.SaveChangesAsync();
            Assert.IsTrue(result == 1);
        }
    }
}
