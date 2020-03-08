using EntityAssociation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Threading.Tasks;

namespace EntityAssociationTest.OneToOne
{
    [TestClass]
    public class OneToOneTest
    {
        #region "Query"

        [TestMethod]
        public async Task TestFetchFirstPerson()
        {
            using (var context = new MyDBEntities())
            {
                Person person = await context.People.FirstOrDefaultAsync();
                if (person != null && person.IdentityCard!=null)
                {
                    Console.WriteLine("姓名：{0} 身份证号：{1}", person.Name, person.IdentityCard.IDNumber);
                }
            }
        }
        [TestMethod]
        public async Task TestFetchFirstPersonUseInclude()
        {
            using (var context = new MyDBEntities())
            {
                Person person = await context.People
                    .Include("IdentityCard").FirstOrDefaultAsync();
                if (person != null && person.IdentityCard!=null)
                {
                    Console.WriteLine("姓名：{0} 身份证号：{1}",
                        person.Name,
                        person.IdentityCard.IDNumber);
                }
            }
        }

        #endregion

        #region "Add"

        /// <summary>
        /// 主从对象都是“全新的”，在内存中关联，然后同时写入数据库
        /// 会生成两条SQL命令
        /// 第一条插入主对象，得到ID后，再使用此ID设置从对象，然后插入
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task TestAddPersonAndHisIdentityCard()
        {

            using (var context = new MyDBEntities())
            {
                //创建一个Person对象，引用唯一的IdentityCard，并且将其插入到数据库中
                Person person = OneToOneHelper.CreatePersonWithoutIDCard();
                person.IdentityCard = OneToOneHelper.CreateIndependentIdentityCard();
                //将主对象追加到DbSet中
                context.People.Add(person);
                //保存，将向数据库发送两次SQL命令
                //第一次为插入Person记录，同时返回主键值
                //第二次使用返回的主键值插入IndentityCard记录
                int result = await context.SaveChangesAsync();
                //共保存两条记录，应该返回2
                Assert.IsTrue(result == 2);
            }
        }

        /// <summary>
        /// 主对象是“老的”，从对象是“新”的，在内存中关联，然后写入数据库
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task TestAddIdentityCardToPerson()
        {
            using (var context = new MyDBEntities())
            {
                //创建主记录(没有关联从记录）
                Person person = OneToOneHelper.CreatePersonWithoutIDCard();
                context.People.Add(person);
                int result = await context.SaveChangesAsync();
                Assert.IsTrue(result == 1);

                //创建一个新的从对象，并关联上主对象
                person.IdentityCard = OneToOneHelper.CreateIndependentIdentityCard();
                result = await context.SaveChangesAsync();
                Assert.IsTrue(result ==1);

                //重新装入主从对象，现在两个对象应该都不为null
                Person personFromDB = await context.People.Include("IdentityCard")
                    .FirstOrDefaultAsync(p => p.PersonId == person.PersonId);
                Assert.IsNotNull(personFromDB);
                Assert.IsNotNull(personFromDB.IdentityCard);
            }
        }

        /// <summary>
        /// 单独创建一个从对象，试图插入数据库,将会报告DbUpdateException异常 
        /// 最终数据没有插入，所以，“永远不要在一对一关联中单独地插入从对象”
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        [ExpectedException(typeof(DbUpdateException))]
        public async Task TestAddSingleIdentityCard()
        {
            using (var context = new MyDBEntities())
            {
                //创建一个“独立的”从对象
                IdentityCard idCard = OneToOneHelper.CreateIndependentIdentityCard();
                //获取主键值
                int maxId = await context.IdentityCards.MaxAsync(
                    id=>id.IdentityCardId);
                idCard.IdentityCardId = maxId + 1;
                //追加到DbSet中
                context.IdentityCards.Add(idCard);
                //由于对应的主记录不存在，所以，数据插入失败
                int result = await context.SaveChangesAsync();
                //此断言是永远不可能被满足
                Assert.IsTrue(result > 0);
            }
        }
        #endregion

        #region "Delete"
        /// <summary>
        /// 删除一条有从记录的主记录
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task TestDeletePerson()
        {
            //插入一条全新的主从记录

            using (var context = new MyDBEntities())
            {
                //创建一个Person对象，引用唯一的IdentityCard，并且将其插入到数据库中
                Person person = OneToOneHelper.CreatePersonWithoutIDCard();
                person.IdentityCard = OneToOneHelper.CreateIndependentIdentityCard();
                context.People.Add(person);
                int result = await context.SaveChangesAsync();
                Assert.IsTrue(result == 2);

                //取出刚刚插入的记录
                Person personFromDB = await context.People.FirstOrDefaultAsync(
                    p => p.PersonId == person.PersonId);
                Assert.IsNotNull(personFromDB);
                //从DbSet中移除主对象
                context.People.Remove(personFromDB);
                //由于数据模型中启用了“级联删除”，将导致主从记录一起删除
                //返回值2，表示删除了两条记录
                //观察SQL命令的发送情况，会发现EF先发出删除IdentityCard记录的命令，
                //再发出删除Person记录的命令
                result = await context.SaveChangesAsync();
                Assert.IsTrue(result == 2);
            }
        }
        /// <summary>
        /// 删除一条没有从记录的主记录
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task TestDeletePersonWithoutIDCard()
        {
            //插入一条全新的主记录
            using (var context = new MyDBEntities())
            {
                //创建一个Person对象，没有IdentityCard信息，插入到数据库中
                Person person = OneToOneHelper.CreatePersonWithoutIDCard();

                context.People.Add(person);
                int result = await context.SaveChangesAsync();
                Assert.IsTrue(result == 1);

                //删除
                Person personFromDB = await context.People.FirstOrDefaultAsync(
                    p => p.PersonId == person.PersonId);
                Assert.IsNotNull(personFromDB);
                context.People.Remove(personFromDB);
                //将只删除一条
                result = await context.SaveChangesAsync();
                Assert.IsTrue(result == 1);
            }
        }
        /// <summary>
        /// 删除主记录的从记录，主记录不动
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task TestDeleteOnlyIdentityCard()
        {
            //插入一条全新的主从记录

            using (var context = new MyDBEntities())
            {
                //创建一个Person对象，引用唯一的IdentityCard，并且将其插入到数据库中
                Person person = OneToOneHelper.CreatePersonWithoutIDCard();
                person.IdentityCard = OneToOneHelper.CreateIndependentIdentityCard();
                context.People.Add(person);
                int result = await context.SaveChangesAsync();
                Assert.IsTrue(result == 2);

                //查找新加入的IdentityCard记录
                IdentityCard idCard = await context.IdentityCards.FirstAsync(
                    ic => ic.IdentityCardId == person.IdentityCard.IdentityCardId);
                Assert.IsNotNull(idCard);
                //移除IDCard
                context.IdentityCards.Remove(idCard);
                result = await context.SaveChangesAsync();
                Assert.IsTrue(result == 1);

                //从数据库中装入主记录，其从记录应该为null
                Person personFromDB = await context.People.Include("IdentityCard")
                    .FirstAsync(p => p.PersonId == person.PersonId);
                Assert.IsNotNull(personFromDB);
                Assert.IsNull(personFromDB.IdentityCard);


            }
        }

        #endregion

        #region "Update"
        /// <summary>
        /// 修改从记录中的字段
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task TestUpdateIdCard()
        {
            using (var context = new MyDBEntities())
            {
                //创建一个Person对象，引用唯一的IdentityCard，并且将其插入到数据库中
                Person person = OneToOneHelper.CreatePersonWithoutIDCard();
                person.IdentityCard = OneToOneHelper.CreateIndependentIdentityCard();
                context.People.Add(person);
                int result = await context.SaveChangesAsync();
                Assert.IsTrue(result == 2);

                //从数据库中装入数据
                Person personFromDB =await context.People.FirstAsync(
                    p => p.PersonId == person.PersonId);
                Assert.IsNotNull(personFromDB);
                Assert.IsNotNull(personFromDB.IdentityCard);
                
                //修改身份证号
                String oldIDNumber = personFromDB.IdentityCard.IDNumber;
                String newIDNumber=OneToOneHelper.CreateIDNumber();
                personFromDB.IdentityCard.IDNumber = newIDNumber;

                result = await context.SaveChangesAsync();
                Assert.IsTrue(result == 1);

                //再次装入数据
                personFromDB = await context.People.FirstAsync(
                    p => p.PersonId == person.PersonId);
                Assert.IsTrue(personFromDB.IdentityCard.IDNumber == newIDNumber);
            }
        }

        #endregion
    }
}
