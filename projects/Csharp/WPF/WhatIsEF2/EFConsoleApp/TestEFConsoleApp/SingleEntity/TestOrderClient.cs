using System;
using EFConsoleApp;
using System.Linq;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace TestEFConsoleApp.SingleEntity
{
    [TestClass]
    public class TestOrderClient
    {
        private static MyDBEntities context;

        private TestContext testContextInstance;

        public TestContext TestContext
        {
            get { return testContextInstance; }
            set { testContextInstance = value; }
        }

        [ClassInitialize]
        public static void MyClassInitialize(TestContext testContext)
        {
            context = new MyDBEntities();
        }
        [ClassCleanup]
        public static void MyClassCleanup()
        {
            context.Dispose();
        }

        [TestMethod]
        public void GetClients()
        {
            var clients = from q in context.OrderClients
                          select q;
            Assert.IsTrue(clients.Count() > 0);
            foreach (var client in clients)
            {
                OrderClientHelper.ShowClient(client);
            }
        }
        [TestMethod]
        public async Task GetClientsAsync()
        {
            var clients = await context.OrderClients.ToListAsync();
            Assert.IsTrue(clients.Count() > 0);
            foreach (var client in clients)
            {
                OrderClientHelper.ShowClient(client);
            }
        }

        #region "添加一条数据"
        [TestMethod]
        public async Task TestAdd()
        {
            OrderClient client = OrderClientHelper.CreateOrderClient();
            context.OrderClients.Add(client);
            int result = await context.SaveChangesAsync();
            Assert.IsTrue(result == 1);
            Console.WriteLine(client);
            var clientBySearched = await context.OrderClients.FirstOrDefaultAsync(
                c => c.ClientID == client.ClientID);
            //检测查找出的对象是否为空
            Assert.IsNotNull(clientBySearched);
            //检测两个对象是否相等
            Assert.IsTrue(client.Equals(clientBySearched));
        }
        #endregion

        #region "修改一条数据"
        [TestMethod]
        public async Task TestModify()
        {
            OrderClient client = await (from c in context.OrderClients
                                        orderby c.ClientID descending
                                        select c).FirstOrDefaultAsync();
            int newPostCode = int.Parse(client.PostCode) + 1;
            client.PostCode = "" + newPostCode;
            int result = await context.SaveChangesAsync();
            Assert.IsTrue(result == 1);
            OrderClient clientBySearched = await context.OrderClients.
                FirstOrDefaultAsync(c => c.ClientID == client.ClientID);
            Assert.IsTrue(client.Equals(clientBySearched));
        }
        #endregion

        #region "删除一条数据"
        [TestMethod]
        public async Task TestDelete()
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
        #endregion

        #region "删除一条数据,Attach的方法"
        [TestMethod]
        public async Task TestDelete2()
        {
                OrderClient stubClient = new OrderClient { ClientID = 362 };
                context.OrderClients.Attach(stubClient);
                context.OrderClients.Remove(stubClient);
                await context.SaveChangesAsync();           
        }
        #endregion

        #region "事务"
        [TestMethod]
        public async Task TestEFTransaction()
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
                                         where c.ClientID == lastClient.ClientID
                                         select c).FirstOrDefaultAsync();
                Assert.IsNotNull(lastClient2);
                //验证数据没被插入
                var newClient2 = await context.OrderClients.SingleOrDefaultAsync(
                    c => c.ClientID == newClient.ClientID);
                Assert.IsNull(newClient2);
            }
        }
        #endregion

        #region "跟踪数据更新"
        [TestMethod]
        public async Task TestEntityState()
        {
            using (var context = new MyDBEntities())
            {
                var client = await (from c in context.OrderClients
                                    orderby c.ClientID descending
                                    select c).FirstOrDefaultAsync();
                if (client != null)
                {
                    DbEntityEntry<OrderClient> clientEntry = context.Entry<OrderClient>(client);
                    Console.WriteLine("\n修改属性前: 状态={0}", clientEntry.State);
                    Assert.IsTrue(clientEntry.State == EntityState.Unchanged);
                    OrderClientHelper.PrintEntityPropertyValue(clientEntry, "PostCode");
                    int newPostCode = Int32.Parse(client.PostCode) + 1;
                    client.PostCode = newPostCode + "";
                    context.ChangeTracker.DetectChanges();
                    Console.WriteLine("\n修改后的属性：状态={0}", clientEntry.State);
                    Assert.IsTrue(clientEntry.State == EntityState.Modified);
                    OrderClientHelper.PrintEntityPropertyValue(clientEntry, "PostCode");

                    int result = await context.SaveChangesAsync();
                    Assert.IsTrue(result == 1);
                    Console.WriteLine("\n保存到数据库后：状态={0}", clientEntry.State);
                    OrderClientHelper.PrintEntityPropertyValue(clientEntry, "PostCode");
                }
            }
        }
        #endregion

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
