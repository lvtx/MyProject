using System;
using System.Collections;
using System.Collections.Generic;

namespace SimpleDB
{
    /// <summary>
    /// SeqScan是顺序扫描访问方法的一种实现，
    /// 该方法以不特定的顺序(例如，按照它们在磁盘上的布局)读取表的每个元组
    /// </summary>
    public class SeqScan : DbIterator
    {
        private static readonly long serialVersionUID = 1L;
        private TransactionId tid = null;
        private int tableid = 0;
        private string tableAlias = null;
        private HeapFile hf = null;

        /// <summary>
        /// 创建对指定表的顺序扫描，作为指定事务的一部分。
        /// </summary>
        /// <param name="tid">此扫描作为其一部分运行的事务</param>
        /// <param name="tableid">要扫描的表</param>
        /// <param name="tableAlias">该表的别名(解析器需要)        
        /// 返回的 tupleDesc需要有名称为tableAlias.fieldName
        /// 的字段(注意：该类不负责处理
        /// tableAlias或fieldName为空的情况。 
        /// 如果它们为空，它应该不会崩溃，
        /// 但是结果名称可以是null.fieldName、
        /// tableAlias.null或null.null)</param>
        public SeqScan(TransactionId tid, int tableid, String tableAlias):this(tid,tableid)
        {
            this.tableAlias = tableAlias;
        }
        public SeqScan(TransactionId tid, int tableid)
        {
            this.tid = tid;
            this.tableid = tableid;
            hf = Database.GetCatalog().GetDatabaseFile(tableid) as HeapFile;
        }
        /**
         * @return
         *       return the table name of the table the operator scans. This should
         *       be the actual name of the table in the catalog of the database
         * */
        public String GetTableName()
        {
            return Database.GetCatalog().GetTableName(tableid);
        }

        /**
         * @return Return the alias of the table this operator scans. 
         * */
        public String GetAlias()
        {
            return tableAlias;
        }

        /**
         * Reset the tableid, and tableAlias of this operator.
         * @param tableid
         *            the table to scan.
         * @param tableAlias
         *            the alias of this table (needed by the parser); the returned
         *            tupleDesc should have fields with name tableAlias.fieldName
         *            (note: this class is not responsible for handling a case where
         *            tableAlias or fieldName are null. It shouldn't crash if they
         *            are, but the resulting name can be null.fieldName,
         *            tableAlias.null, or null.null).
         */
        public void Reset(int tableid, String tableAlias)
        {
            this.tableid = tableid;
            this.tableAlias = tableAlias;
        }



        /**
         * Returns the TupleDesc with field names from the underlying HeapFile,
         * prefixed with the tableAlias string from the constructor. This prefix
         * becomes useful when joining tables containing a field(s) with the same
         * name.
         * 
         * @return the TupleDesc with field names from the underlying HeapFile,
         *         prefixed with the tableAlias string from the constructor.
         */
        public TupleDesc GetTupleDesc()
        {
            // some code goes here
            return null;
        }
        private DbFileIterator tupleIterator;
        public void Open()
        {
            tupleIterator = hf.Iterator(tid);
            tupleIterator.Open();
        }
        public bool HasNext()
        {
            if (tupleIterator == null)
            {
                Console.WriteLine("Seq Scan tupleIterator为空");
                return false;
            }
            return tupleIterator.HasNext();
        }

        public Tuple Next()
        {
            Tuple tuple = tupleIterator.Next();
            if (tuple == null)
            {
                Console.WriteLine("元组是空的");
            }
            return tuple;
        }

        public void Rewind()
        {
            tupleIterator.Rewind();
            Open();
        }

        public void Close()
        {
            tupleIterator.Close();
            tupleIterator = null;
        }
    }
}
