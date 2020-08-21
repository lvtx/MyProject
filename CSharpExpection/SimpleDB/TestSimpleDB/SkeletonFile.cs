using SimpleDB;
using System.Collections.Generic;

namespace TestSimpleDB
{
    /// <summary>
    /// 测试用
    /// </summary>
    internal class SkeletonFile : DbFile
    {
        private int v;
        private TupleDesc tupleDesc;

        public SkeletonFile(int v, TupleDesc tupleDesc)
        {
            this.v = v;
            this.tupleDesc = tupleDesc;
        }

        public List<Page> DeleteTuple(TransactionId tid, Tuple t)
        {
            throw new System.NotImplementedException();
        }

        public int GetId()
        {
            return v;
        }

        public TupleDesc GetTupleDesc()
        {
            return tupleDesc;
        }

        public List<Page> InsertTuple(TransactionId tid, Tuple t)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerator<Tuple> Iterator(TransactionId tid)
        {
            throw new System.NotImplementedException();
        }

        public Page ReadPage(PageId id)
        {
            throw new System.NotImplementedException();
        }

        public void WritePage(Page p)
        {
            throw new System.NotImplementedException();
        }

        DbFileIterator DbFile.Iterator(TransactionId tid)
        {
            throw new System.NotImplementedException();
        }
    }
}