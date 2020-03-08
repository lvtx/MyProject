using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConcurrencyTest
{
    public class ConcurrencyTestHelper
    {
        /// <summary>
        /// 清空原始记录
        /// </summary>
        public static void DeleteAllRecord()
        {
            using (var context = new MyDBEntities())
            {
                context.Database.ExecuteSqlCommand("delete from Concurrency");
            }
        }
        /// <summary>
        /// 添加测试数据
        /// </summary>
        public static void Seed()
        {
            using (var context = new MyDBEntities())
            {
                DeleteAllRecord();
                for (int i = 0; i < 3; i++)
                {
                    Concurrency concurrency = new Concurrency()
                    {
                        numField = i,
                        StringField = "String" + i.ToString()
                    };
                    context.Concurrencies.Add(concurrency);
                }
                context.SaveChanges();
                ShowRecords();
            }
        }

        /// <summary>
        /// 显示所有记录
        /// </summary>
        public static void ShowRecords()
        {
            using (var context = new MyDBEntities())
            {
                foreach (var item in context.Concurrencies)
                {
                    ShowConcurrency(item);
                }
            }

        }

        /// <summary>
        /// 显示一条Concurrency记录
        /// </summary>
        /// <param name="conc"></param>
        public static void ShowConcurrency(Concurrency conc)
        {
            if (conc == null)
            {
                return;
            }
            Console.WriteLine("ID：{0}，StringField:{1}，numField:{2},rowversion:{3}", conc.ID, conc.StringField, conc.numField, ByteToHexStr(conc.rowversion));
        }

        /// <summary> 
        /// 字节数组转16进制字符串 
        /// </summary> 
        /// <param name="bytes"></param> 
        /// <returns></returns> 
        public static string ByteToHexStr(byte[] bytes)
        {
            string returnStr = "";
            if (bytes != null)
            {
                for (int i = 0; i < bytes.Length; i++)
                {
                    returnStr += bytes[i].ToString("X2");
                }
            }
            return returnStr;
        }

        /// <summary>
        /// 打印属性值
        /// </summary>
        /// <param name="values"></param>
        public static void PrintPropertyValues(DbPropertyValues values)
        {
            foreach (var propertyName in values.PropertyNames)
            {
                Console.WriteLine(" - {0}: {1}",
                propertyName,
                propertyName == "rowversion" ?
                ByteToHexStr((byte[])values[propertyName]) : values[propertyName]);
            }
        }

        /// <summary>
        /// 显示并发冲突信息
        /// </summary>
        /// <param name="ex"></param>
        public static void ShowDbUpdateConcurrencyException(DbUpdateConcurrencyException ex)
        {
            foreach (var entry in ex.Entries)
            {
                Console.WriteLine(
                "{0}对象引发的并发冲突：",
                entry.Entity.GetType());
                Console.WriteLine("\n尝试保存以下值（CurrentValues）:");
                PrintPropertyValues(entry.CurrentValues);
                Console.WriteLine("\n最初从数据库中提取的原始值(OriginalValues)为:");
                PrintPropertyValues(entry.OriginalValues);
                var databaseValues = entry.GetDatabaseValues();
                Console.WriteLine("\n由于另一个用户修改了数据，数据库中的当前值(databaseValues)为:");
                PrintPropertyValues(databaseValues);
            }

        }
       
    }
}
