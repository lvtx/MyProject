using System;

namespace SimpleDB
{
    /// <summary>
    /// HeapPage对象的唯一标识符
    /// </summary>
    public class HeapPageId : PageId
    {
        private int tableId;
        private int pgNo;

        /// <summary>
        /// 为特定表的特定页面创建PageID结构
        /// </summary>
        public HeapPageId(int tableId, int pgNo)
        {
            try
            {
                this.tableId = tableId;
                this.pgNo = pgNo;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 与该PageId关联的表
        /// </summary>
        /// <returns></returns>
        public int GetTableId()
        {
            return tableId;
        }

        /// <summary>
        ///此页的哈希码，由表号和页码的串联表示
        ///（例如，如果将PageId用作BufferPool中的哈希表中的键，则为必需）
        /// </summary>
        /// <returns></returns>
        public int HashCode()
        {
            return GetHashCode();
        }

        /// <summary>
        /// 与该PageId关联的表中的页码
        /// </summary>
        /// <returns></returns>
        public int PageNumber()
        {
            return pgNo;
        }

        /// <summary>
        /// 以整数数组形式返回此对象的表示形式，以写入磁盘。
        /// 返回数组的大小必须包含与构造函数之一的args数量相对应的整数数量。
        /// </summary>
        /// <returns></returns>
        public int[] Serialize()
        {
            int[] data = new int[2];
            data[0] = tableId;
            data[1] = pgNo;
            return data;
        }

        public override bool Equals(object o)
        {
            if (o != null)
            {
                try
                {
                    HeapPageId pid = o as HeapPageId;
                    if (pid.tableId == tableId && pid.pgNo == pgNo)
                    {
                        return true;
                    }
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return false;
        }

        public override int GetHashCode()
        {
            //计算pgNo位数
            Func<int> func = () =>
            {
                int count = 0;
                int num = pgNo;
                while (num != 0)
                {
                    num = num / 10;
                    count++;
                }
                return count;
            };
            int n = func();
            try
            {
                int ret = tableId * (int)Math.Pow(10, n) + pgNo;
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