using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConcurrencyTest
{
    class Program
    {
        private static Random ran = new Random();
        static void Main(string[] args)
        {
            Console.WriteLine("数据库初始数据为：");
           // ConcurrencyTestHelper.Seed();
            ConcurrencyTestHelper.ShowRecords();
           // ModifyFieldTest();
           // ShowConcurrencyException();
            DoWithConcurrencyException();
            Console.ReadKey();
        }

        /// <summary>
        /// 每当更改任何一个字段时，数据库都会为Rowversion字段自动给一个新值
        /// </summary>
        public static void ModifyFieldTest()
        {
            using (var context = new MyDBEntities())
            {
                Concurrency conc = context.Concurrencies.FirstOrDefault();
                Console.WriteLine("原始数据：");
                ConcurrencyTestHelper.ShowConcurrency(conc);
                conc.numField++;
                context.SaveChanges();
                Console.WriteLine("修改numField属性值");
                Concurrency conc2 = context.Concurrencies.First(c => c.ID == conc.ID);
                ConcurrencyTestHelper.ShowConcurrency(conc2);
            }
        }
       

        /// <summary>
        /// 此方法将引发DbUpdateConcurrencyException 
        /// </summary>
        private static void ShowConcurrencyException()
        {
            using (var context = new MyDBEntities())
            {
                Concurrency conc = context.Concurrencies.FirstOrDefault();
                conc.numField++;
                Console.WriteLine("尝试向数据库写入对象");
                //直接修改数据库，以便引发并发存取冲突
                context.Database.ExecuteSqlCommand("Update Concurrency set numField={0} where ID={1} ", ran.Next(1, 10000), conc.ID);
                //不加异常捕获，将导致异常的出现
                //context.SaveChanges();

                try
                {
                    context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    ConcurrencyTestHelper.ShowDbUpdateConcurrencyException(ex);
                }
            }
        }

        /// <summary>
        /// 处理并发冲突异常
        /// </summary>
        private static void DoWithConcurrencyException()
        {
            using (var context = new MyDBEntities())
            {
                Concurrency conc = context.Concurrencies.FirstOrDefault();
                conc.numField++;
                Console.WriteLine("尝试向数据库写入对象：");
               ConcurrencyTestHelper.ShowConcurrency(conc);
                //直接修改数据库，以便引发并发存取冲突
                context.Database.ExecuteSqlCommand("Update Concurrency set numField={0} where ID={1} ", ran.Next(1, 10000), conc.ID);
                try
                {
                    context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    ConcurrencyTestHelper.ShowDbUpdateConcurrencyException(ex);
                    DbEntityEntry entry = ex.Entries.ElementAt(0);
                    //获取数据库中的当前值
                    var databaseValues = entry.GetDatabaseValues();
                    //将状态对象的原始值重置为数据库的当前值
                    entry.OriginalValues.SetValues(databaseValues);
                    //再次写入数据库
                    context.SaveChanges();
                    Console.WriteLine("冲突已经解决，数据己经写入到数据库中,其内容为：");
                    ConcurrencyTestHelper.ShowRecords();
                }
            }
        }

    }
}
