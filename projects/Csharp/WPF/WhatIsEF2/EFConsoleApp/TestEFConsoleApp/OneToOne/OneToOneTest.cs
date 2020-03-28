using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EFConsoleApp;
using System.Linq;
using System.Data.Entity;
using System.Threading.Tasks;

namespace TestEFConsoleApp.OneToOne
{
    [TestClass]
    public class OneToOneTest
    {
        #region "virtual代理类"
        [TestMethod]
        public async Task TestFetchFirstPersonAsync()
        {
            using (var context = new MyDBEntities())
            {
                Person person = await context.People.FirstOrDefaultAsync();
                if (person != null)
                {
                    Console.WriteLine("姓名: {0}", person.Name);
                    if (person.IdentityCard != null)
                    {
                        Console.WriteLine("身份证号: {0}", person.IdentityCard.IDNumber);
                    }
                    else
                    {
                        Console.WriteLine("身份证号: 无");
                    }
                }
            }
        }
        #endregion

        #region "预装载(Sql的联结)"
        [TestMethod]
        public async Task TestFetchFirstPersonUseInclude()
        {
            using (var context = new MyDBEntities())
            {
                Person person = await context.People
                    .Include("IdentityCard").FirstOrDefaultAsync();
                if (person != null)
                {
                    Console.WriteLine("姓名: {0},身份证号: {1}", person.Name,
                        person.IdentityCard.IDNumber);
                }
            }
        }
        #endregion

        #region "显式加载"
        [TestMethod]
        public async Task TestExplicitLoadingBookReview()
        {
            using (var context = new MyDBEntities())
            {
                var query = from book in context.Books
                            select book;
                Book firstBook = await query.FirstAsync();
                Assert.IsTrue(firstBook.BookReviews.Count == 0);
                context.Entry<Book>(firstBook)
                    .Collection<BookReview>("BookReviews").Load();
                Assert.IsTrue(firstBook.BookReviews.Count > 0);
            }
        }
        #endregion

        #region "Add"
        [TestMethod]
        public async Task TestAddPersonAndHisIdentityCard()
        {
            using (var context = new MyDBEntities())
            {
                Person person = OneToOneHelper.CreatePersonWithoutIDCard();
                person.IdentityCard = OneToOneHelper.CreateIndependentIdentityCard();
                context.People.Add(person);
                //保存，将向数据库发送两次SQL命令
                //第一次为插入Person记录，同时返回主键值
                //第二次使用返回的主键值插入IndentityCard记录
                int result = await context.SaveChangesAsync();
                //共保存两条记录，应该返回2
                Assert.IsTrue(result == 2);
            }
        }
        [TestMethod]
        public async Task TestAddIdentityCardToPerson()
        {
            using (var context = new MyDBEntities())
            {
                Person person = OneToOneHelper.CreatePersonWithoutIDCard();
                context.People.Add(person);
                int result = await context.SaveChangesAsync();
                Assert.IsTrue(result == 1);

                //错误示范
                //IdentityCard identityCard = OneToOneHelper.CreateIndependentIdentityCard();
                //context.IdentityCards.Add(identityCard);
                IdentityCard identityCard = OneToOneHelper.CreateIndependentIdentityCard();
                person.IdentityCard = identityCard;
                result = await context.SaveChangesAsync();
                Assert.IsTrue(result == 1);

                Person personBySearched = await context.People.Include("IdentityCard")
                    .FirstOrDefaultAsync(p => p.PersonId == person.PersonId);
                Assert.IsNotNull(personBySearched);
                Assert.IsNotNull(personBySearched.IdentityCard);
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
            using (var context = new MyDBEntities())
            {
                Person person = OneToOneHelper.CreatePersonWithoutIDCard();
                person.IdentityCard = OneToOneHelper.CreateIndependentIdentityCard();
                context.People.Add(person);
                int result = await context.SaveChangesAsync();
                Assert.IsTrue(result == 2);

                var personBySearched = await context.People.
                    FirstOrDefaultAsync(p => p.PersonId == person.PersonId);
                Assert.IsNotNull(personBySearched);
                context.People.Remove(personBySearched);
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
                Person personFromDB = await context.People.FirstAsync(
                    p => p.PersonId == person.PersonId);
                Assert.IsNotNull(personFromDB);
                Assert.IsNotNull(personFromDB.IdentityCard);

                //修改身份证号
                String oldIDNumber = personFromDB.IdentityCard.IDNumber;
                String newIDNumber = OneToOneHelper.CreateIDNumber();
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

