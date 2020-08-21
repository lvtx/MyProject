using System;

namespace SimpleDB
{
    /// <summary>
    /// RecordId是对特定表的特定页面上的特定元组的引用。
    /// </summary>
    public class RecordId
    {
        private static readonly long serialVersionUID = 1L;
        private PageId pid;
        private int tupleno;

        /// <summary>
        /// 引用指定的PageId和元组编号创建一个新的RecordId
        /// </summary>
        /// <param name="pid">页Id</param>
        /// <param name="tupleno">元组编号</param>
        public RecordId(PageId pid, int tupleno)
        {
            this.pid = pid;
            this.tupleno = tupleno;
        }

        /// <summary>
        /// 返回此RecordId引用的元组编号
        /// </summary>
        /// <returns></returns>
        public int Tupleno()
        {
            return tupleno;
        }

        /// <summary>
        /// 返回此RecordId引用的页面ID
        /// </summary>
        /// <returns></returns>
        public PageId GetPageId()
        {
            return pid;
        }

        /// <summary>
        /// 如果两个RecordId对象表示相同的元组，则认为它们相等
        /// 如果this和o表示相同的元组，则为True
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public override bool Equals(Object o)
        {
            try
            {
                if (o != null)
                {
                    RecordId rid = o as RecordId;
                    if (rid.tupleno == tupleno && pid.Equals(rid.pid) == true)
                    {
                        return true;
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
            return false;
        }

        /// <summary>
        /// 使两个相等的RecordId实例(相对于equals())
        /// 具有相同的hashCode()
        /// </summary>
        /// <returns></returns>
        public int HashCode()
        {
            return GetHashCode();
        }

        public override int GetHashCode()
        {
            try
            {
                int ret = pid.HashCode() + tupleno;
                return ret;
            }
            catch (OverflowException ex)
            {
                Console.WriteLine(ex.Message);
                return 0;
            }
        }
    }
}