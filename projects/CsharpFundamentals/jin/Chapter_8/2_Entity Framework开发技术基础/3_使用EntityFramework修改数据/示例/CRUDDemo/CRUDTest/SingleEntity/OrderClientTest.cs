using CRUD;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;

namespace EntityAssociationTest.SingleEntity
{
    [TestClass]
    public class OrderClientTest
    {
        [TestMethod]
        public void TestAdd()
        {
            using (var context = new MyDBContext())
            {
                //创建一个新的数据实体
                OrderClient client = OrderClientHelper.CreateOrderClient();
                context.OrderClients.Add(client);
                int result = context.SaveChanges();
                Assert.IsTrue(result == 1);
                Console.WriteLine(client);
                //从数据库中重新装载数据
                OrderClient clientFromDB = context.OrderClients.FirstOrDefault(
                    c => c.ClientID == client.ClientID
                    );
                Assert.IsNotNull(clientFromDB);
                //检测两个OrderClient对象是相等的
                Assert.IsTrue(clientFromDB.Equals(client));
            }
        }

        [TestMethod]
        public void TestModify()
        {
            using (var context = new MyDBContext())
            {
                //获取最后一条记录
                //由于EF不支持Last()查询，因此，先对其进行降序排列，然后取第一条记录
                var client =(from c in context.OrderClients
                                 orderby c.ClientID descending
                                 select c).FirstOrDefault();
                if (client != null)
                {
                    //把邮编当成数字，加1，作为修改后的新值进行测试
                    int newPostCode = Int32.Parse(client.PostCode) + 1;
                    client.PostCode = newPostCode.ToString();
                    int result = context.SaveChanges();
                    Assert.IsTrue(result == 1);

                    //从数据库中再次提取原始值，以确信数据的确更新了
                    OrderClient clientFromDB = context.OrderClients.First(c => c.ClientID == client.ClientID);
                    Assert.IsTrue(client.Equals(clientFromDB));
                }

            }
        }

        [TestMethod]
        public void TestDelete()
        {
            using (var context = new MyDBContext())
            {
                //获取最后一条记录
                //由于EF不支持Last()查询，因此，先对其进行降序排列，然后取第一条记录
                var client =  (from c in context.OrderClients
                                    orderby c.ClientID descending
                                    select c).First();
                if (client != null)
                {
                    context.OrderClients.Remove(client);
                    int result = context.SaveChanges();
                    Assert.IsTrue(result == 1);
                }
            }
        }

        [TestMethod]
        public void TestEntityState()
        {
            using (var context = new MyDBContext())
            {
                //获取最后一条记录
                //由于EF不支持Last()查询，因此，先对其进行降序排列，然后取第一条记录
                var client = (from c in context.OrderClients
                                   orderby c.ClientID descending
                                   select c).FirstOrDefault();
                if (client != null)
                {
                    DbEntityEntry<OrderClient> clientEntry = context.Entry<OrderClient>(client);
                    Console.WriteLine("\n修改属性前：状态={0}", clientEntry.State);
                    Assert.IsTrue(clientEntry.State == EntityState.Unchanged);
                    OrderClientHelper.PrintEntityPropertyValue(clientEntry, "PostCode");
                    //把邮编当成数字，加1，作为修改后的新值进行测试
                    int newPostCode = Int32.Parse(client.PostCode) + 1;
                    client.PostCode = newPostCode.ToString();
                    context.ChangeTracker.DetectChanges();
                    Console.WriteLine("\n修改属性后：状态={0}", clientEntry.State);
                    Assert.IsTrue(clientEntry.State == EntityState.Modified);
                    OrderClientHelper.PrintEntityPropertyValue(clientEntry, "PostCode");

                    int result = context.SaveChanges();
                    Assert.IsTrue(result == 1);
                    Console.WriteLine("\n保存到数据库后：状态={0}",clientEntry.State);
                    OrderClientHelper.PrintEntityPropertyValue(clientEntry, "PostCode");
                }
            }
        }

        [TestMethod]
        public void TestDisableStateTracking()
        {
            using (var context = new MyDBContext())
            {
                //获取最后一条记录
                //由于EF不支持Last()查询，因此，先对其进行降序排列，然后取第一条记录
                var client =  (from c in context.OrderClients.AsNoTracking()
                                    orderby c.ClientID descending
                                    select c).FirstOrDefault();
                if (client != null)
                {
                    DbEntityEntry<OrderClient> clientEntry = context.Entry<OrderClient>(client);
                    Console.WriteLine("\n修改属性前：状态={0}", clientEntry.State);
                    Assert.IsTrue(clientEntry.State == EntityState.Detached);
                    
                    //把邮编当成数字，加1，作为修改后的新值进行测试
                    int newPostCode = Int32.Parse(client.PostCode) + 1;
                    client.PostCode = newPostCode.ToString();
                    context.ChangeTracker.DetectChanges();
                    Console.WriteLine("\n修改属性后：状态={0}", clientEntry.State);
                    Assert.IsTrue(clientEntry.State == EntityState.Detached);
                 
                    int result = context.SaveChanges();
                    Assert.IsTrue(result == 0);
                    Console.WriteLine("\n调用SaveChanges()尝试保存到数据库之后：状态={0}", clientEntry.State);
                    Assert.IsTrue(clientEntry.State == EntityState.Detached);
                }
            }
        }
    }
}
