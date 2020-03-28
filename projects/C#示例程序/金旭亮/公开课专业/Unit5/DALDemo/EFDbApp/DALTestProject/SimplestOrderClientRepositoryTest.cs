using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DataAccessLayer;
using Model;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Data.Entity;

namespace DALTestProject
{
    [TestClass]
    public class SimplestOrderClientRepositoryTest
    {
        ISimplestOrderClientRepository repo = null;

        [TestInitialize]
        public void TestInitialize()
        {
            repo = new SimplestOrderClientRepository();
        }
        [TestCleanup]
        public void TestCleanup()
        {
            // Not required - here for illustration
            
        }

        [TestMethod]
        public void TestGetAllClients()
        {
            List<OrderClient> Clients = repo.GetAllClients();
            Assert.IsTrue(Clients.Count() > 0);
        }

        [TestMethod]
        public void TestAddClient()
        {
            OrderClient client = OrderClientHelper.CreateOrderClient();
            int result = repo.AddClient(client);
            Assert.IsTrue(result == 1);
        }

        [TestMethod]
        public void TestDeleteClient()
        {
            List<OrderClient> Clients = repo.GetAllClients();
            int result = repo.DeleteClient(Clients.Last().ClientID);
            Assert.IsTrue(result == 1);
        }

        [TestMethod]
        public void TestModifyClient()
        {
            //新加一条记录
            OrderClient client = OrderClientHelper.CreateOrderClient();
            int result = repo.AddClient(client);
            Assert.IsTrue(result == 1);

            //修改邮编
            int oldPostCode = int.Parse(client.PostCode);
            client.PostCode = (oldPostCode + 1).ToString();

            result = repo.ModifyClient(client);
            Assert.IsTrue(result == 1);
        }

        [TestMethod]
        public void TestFindClientsByName()
        {
             //新加一条记录
            OrderClient client = OrderClientHelper.CreateOrderClient();
            int result = repo.AddClient(client);
            Assert.IsTrue(result == 1);

            String FindWhat = client.ClientName.Substring(0, 2);
            List<OrderClient> clients = repo.FindClientsByName(FindWhat);

            Assert.IsTrue(clients.Count() > 0);
            foreach (var c in clients)
            {
                Assert.IsTrue(c.ClientName.StartsWith(FindWhat));
            }
        }
    }
}
