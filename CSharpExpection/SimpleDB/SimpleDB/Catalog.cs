using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Policy;

namespace SimpleDB
{
    /// <summary>
    /// 目录跟踪数据库中所有可用表及其关联的模式
    /// 目前，这是一个存根目录，必须先由用户程序在其中填充表，然后才能使用它
    /// 最终，应将其转换为从磁盘读取目录表的目录。
    /// </summary>
    public class Catalog
    {
        private Hashtable hashNameToFile = new Hashtable();
        private Hashtable hashIdToFile = new Hashtable();
        /// <summary>
        /// 创建一个新的空目录
        /// </summary>
        public Catalog()
        {

        }
        /// <summary>
        /// 将新表添加到目录中
        /// 该表的内容存储在指定的DbFile中
        /// </summary>
        /// <param name="file">file.GetId()是file/tupledesc参数的标识符用于调用getTupleDesc和getFile</param>
        /// <param name="name">表名 -- 如果存在的名字发生冲突,使用最新要添加的表的名字作为表名</param>
        /// <param name="pkeyField">主键字段的名称</param>
        public void AddTable(DbFile file, String name, String pkeyField)
        {
            if (hashNameToFile.ContainsKey(name) == true)
            {
                hashNameToFile[name] = file;
                hashIdToFile[file.GetId()] = file;
            }
            else
            {
                hashNameToFile.Add(name, file);
                hashIdToFile.Add(file.GetId(), file);
            }
        }

        public void AddTable(DbFile file, String name)
        {
            AddTable(file, name, "");
        }

        /// <summary>
        /// 将新表添加到目录中
        /// 该表具有使用指定的TupleDesc格式化的元组，并且其内容存储在指定的DbFile中。
        /// </summary>
        /// <param name="file">file.getId()是file/tupledesc参数的标识符
        /// 用于调用getTupleDesc()和getFile()</param>
        public void AddTable(DbFile file)
        {
            AddTable(file, (System.Guid.NewGuid().ToString()));
        }

        /// <summary>
        /// 根据给定表名返回表
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public int GetTableId(String name)
        {
            if (hashNameToFile.ContainsKey(name))
            {
                return (hashNameToFile[name] as DbFile).GetId();
            }
            throw new ArgumentNullException();
        }

        /// <summary>
        /// 返回给定表的tuple descriptor(schema)
        /// </summary>
        /// <param name="tableid">由传递给addTable的DbFile.getId()函数指定的表ID</param>
        /// <returns></returns>
        public TupleDesc GetTupleDesc(int tableid)
        {
            DbFile file = null;
            try
            {
                if (hashIdToFile.ContainsKey(tableid))
                {
                    file = hashIdToFile[tableid] as DbFile;
                    return file.GetTupleDesc();
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine("捕获Catalog.GetTupleDesc()异常:{0}",ex);
                return null;
            }
        }

        /// <summary>
        /// 返回DbFile可以用于读取指定表的内容
        /// </summary>
        /// <param name="tableid">由传递给addTable的DbFile.getId()函数指定的表ID</param>
        /// <returns></returns>
        public DbFile GetDatabaseFile(int tableid)
        {
            DbFile file = null;
            try
            {
                if (hashIdToFile.ContainsKey(tableid))
                {
                    file = hashIdToFile[tableid] as DbFile;
                    return file;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine("捕获Catalog.GetDatabaseFile()异常:{0}",ex);
                return null;
            }
        }

        public String GetPrimaryKey(int tableid)
        {
            // some code goes here
            return null;
        }

        public IEnumerable<int> TableIdIterator()
        {
            // some code goes here
            return null;
        }

        public String GetTableName(int id)
        {
            // some code goes here
            return null;
        }

        /** Delete all tables from the catalog */
        public void Clear()
        {
            // some code goes here
        }

        /**
         * Reads the schema from a file and creates the appropriate tables in the database.
         * @param catalogFile
         */
        public void LoadSchema(String catalogFile)
        {
            String line = "";
            var file = File.Create(catalogFile);
            String baseFolder = Directory.GetParent(catalogFile).FullName;
            try
            {
                BufferedStream br = new BufferedStream(new FileStream(catalogFile, FileMode.Open));

                while ((line = new StreamReader(br).ReadLine()) != null)
                {
                    //assume line is of the format name (field type, field type, ...)
                    String name = line.Substring(0, line.IndexOf("(")).Trim();
                    //System.out.println("TABLE NAME: " + name);
                    String fields = line.Substring(line.IndexOf("(") + 1, line.IndexOf(")")).Trim();
                    String[] els = fields.Split(',');
                    List<String> names = new List<String>();
                    List<Type> types = new List<Type>();
                    String primaryKey = "";
                    foreach (var e in els)
                    {
                        String[] els2 = e.Trim().Split(' ');
                        names.Add(els2[0].Trim());
                        if (els2[1].Trim().ToLower().Equals("int"))
                            types.Add(typeof(INT_TYPE));
                        else if (els2[1].Trim().ToLower().Equals("string"))
                            types.Add(typeof(STRING_TYPE));
                        else
                        {
                            Console.WriteLine("Unknown type " + els2[1]);
                            Environment.Exit(0);
                        }
                        if (els2.Length == 3)
                        {
                            if (els2[2].Trim().Equals("pk"))
                                primaryKey = els2[0].Trim();
                            else
                            {
                                Console.WriteLine("Unknown annotation " + els2[2]);
                                Environment.Exit(0);
                            }
                        }
                    }
                    Type[] typeAr = types.ToArray();
                    String[] namesAr = names.ToArray();
                    TupleDesc t = new TupleDesc(typeAr, namesAr);
                    HeapFile tabHf = new HeapFile(new FileInfo(baseFolder + "/" + name + ".dat"), t);
                    AddTable(tabHf, name, primaryKey);
                    Console.WriteLine("Added table : " + name + " with schema " + t);
                }
            }
            catch (IOException e)
            {
                Console.WriteLine(e.StackTrace);
                Environment.Exit(0);
            }
            catch (IndexOutOfRangeException)
            {
                Console.WriteLine("Invalid catalog entry : " + line);
                Environment.Exit(0);
            }
        }
    }
}
