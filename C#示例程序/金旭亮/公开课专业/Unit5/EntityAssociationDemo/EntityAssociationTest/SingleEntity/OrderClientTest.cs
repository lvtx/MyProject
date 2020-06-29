using EntityAssociation;
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
        public async Task TestAdd()
        {
            using (var context = new MyDBEntities())
            {
                //创建一个新的数据实体
                OrderClient client = OrderClientHelper.CreateOrderClient();
                context.OrderClients.Add(client);
                int result = await context.SaveChangesAsync();
                Assert.IsTrue(result == 1);
                Console.WriteLine(client);
                //从数据库中重新装载数据
                OrderClient clientFromDB = await context.OrderClients.FirstOrDefaultAsync(
                    c => c.ClientID == client.ClientID
                    );
                Assert.IsNotNull(clientFromDB);
                //检测两个OrderClient对象是相等的
                Assert.IsTrue(clientFromDB.Equals(client));
            }
        }

        [TestMethod]
        public async Task TestModify()
        {
            using (var context = new MyDBEntities())
            {
                //获取最后一条记录
                //由于EF不支持Last()查询，因此，先对其进行降序排列，然后取第一条记录
                var client =await (from c in context.OrderClients
                                 orderby c.ClientID descending
                                 select c).FirstOrDefaultAsync();
                if (client != null)
                {
                    //把邮编当成数字，加1，作为修改后的新值进行测试
                    int newPostCode = Int32.Parse(client.PostCode) + 1;
                    client.PostCode = newPostCode.ToString();
                    int result = await context.SaveChangesAsync();
                    Assert.IsTrue(result == 1);

                    //从数据库中再次提取原始值，以确信数据的确更新了
                    OrderClient clientFromDB = context.OrderClients.First(c => c.ClientID == client.ClientID);
                    Assert.IsTrue(client.Equals(clientFromDB));
                }
            }
        }

        [TestMethod]
        public async Task TestDelete()
        {
            using (var context = new MyDBEntities())
            {
                //获取最后一条记录
                //由于EF不支持Last()查询，因此，先对其进行降序排列，然后取第一条记录
                var client = await (from c in context.OrderClients
                                    orderby c.ClientID descending
                                    select c).FirstAsync();
                if (client != null)
                {
                    context.OrderClients.Remove(client);
                    int result = await context.SaveChangesAsync();
                    Assert.IsTrue(result == 1);
                }
            }
        }

        [TestMethod]
        public  async Task TestEFTransaction()
        {
            OrderClient firstClient, lastClient, newClient;
            using (var context = new MyDBEntities())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    //修改第一个客户对象
                    firstClient = await context.OrderClients.FirstOrDefaultAsync();
                    String oldPostCode = firstClient.PostCode;
                    if (firstClient != null)
                    {
                        //把邮编当成数字，加1，作为修改后的新值进行测试
                        int newPostCode = Int32.Parse(oldPostCode) + 1;
                        firstClient.PostCode = newPostCode.ToString();
                    }
                    int result = await context.SaveChangesAsync();
                    Assert.IsTrue(result > 0);
                    Console.WriteLine("客户ID={0}的邮编被改为：{1}", firstClient.ClientID,
                        firstClient.PostCode);

                    //获取最后一条记录并删除之
                    //由于EF不支持Last()查询，因此，先对其进行降序排列，然后取第一条记录
                    lastClient = await (from c in context.OrderClients
                                            orderby c.ClientID descending
                                            select c).FirstOrDefaultAsync();

                    context.OrderClients.Remove(lastClient);
                    result = await context.SaveChangesAsync();
                    Assert.IsTrue(result > 0);

                    Console.WriteLine("客户ID={0}被删除", lastClient.ClientID);
                    //新建一个客户对象
                    newClient = OrderClientHelper.CreateOrderClient();

                    context.OrderClients.Add(newClient);
                    result = await context.SaveChangesAsync();
                    Assert.IsTrue(result > 0);
                    Console.WriteLine("新建客户ID：{0}，姓名：{1}", newClient.ClientID,
                       newClient.ClientName);
                    //回滚之后，导致所有修改都被取消
                    transaction.Rollback();
                }
            }

            using (var context = new MyDBEntities())
            {
                //验证数据是否己被修改
                var firstClient2 = await context.OrderClients.FirstOrDefaultAsync(
                    c => c.ClientID == firstClient.ClientID);
                Assert.IsFalse(firstClient.Equals(firstClient2));
                //验证数据没被删除
                var lastClient2 = await (from c in context.OrderClients
                                         where c.ClientID==lastClient.ClientID
                                         select c).FirstOrDefaultAsync();
                Assert.IsNotNull(lastClient2);
                //验证数据没被插入
                var newClient2 = await context.OrderClients.SingleOrDefaultAsync(
                    c => c.ClientID == newClient.ClientID);
                Assert.IsNull(newClient2);
            }
            
        }

        [TestMethod]
        public async Task TestEntityState()
        {
            using (var context = new MyDBEntities())
            {
                //获取最后一条记录
                //由于EF不支持Last()查询，因此，先对其进行降序排列，然后取第一条记录
                var client = await(from c in context.OrderClients
                                   orderby c.ClientID descending
                                   select c).FirstOrDefaultAsync();
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

                    int result = await context.SaveChangesAsync();
                    Assert.IsTrue(result == 1);
                    Console.WriteLine("\n保存到数据库后：状态={0}",clientEntry.State);
                    OrderClientHelper.PrintEntityPropertyValue(clientEntry, "PostCode");
                }
            }
        }

        [TestMethod]
        public async Task TestDisableStateTracking()
        {
            using (var context = new MyDBEntities())
            {
                //获取最后一条记录
                //由于EF不支持Last()查询，因此，先对其进行降序排列，然后取第一条记录
                var client = await (from c in context.OrderClients.AsNoTracking()
                                    orderby c.ClientID descending
                                    select c).FirstOrDefaultAsync();
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
                 
                    int result = await context.SaveChangesAsync();
                    Assert.IsTrue(result == 0);
                    Console.WriteLine("\n调用SaveChangesAsync()尝试保存到数据库之后：状态={0}", clientEntry.State);
                    Assert.IsTrue(clientEntry.State == EntityState.Detached);
                }
            }
        }
    }
}
