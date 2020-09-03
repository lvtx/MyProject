using System;

namespace SimpleDB
{
    [Serializable]
    public class JoinPredicate
    {
        private readonly long serialVersionUID = 1L;
        Predicate.Op op;
        int index1 = 0;
        int index2 = 0;
        /// <summary>
        /// 构造函数--在包含两个元组的两个字段上创建一个新谓词
        /// </summary>
        /// <param name="index1">
        ///            谓词中第一个元组的字段索引 </param>
        /// <param name="index2">
        ///            谓词中第二个元组的字段索引 </param>
        /// <param name="op">
        ///            要应用的操作(如Predicate.Op中所定义); either
        ///            Predicate.Op.GREATER_THAN, Predicate.Op.LESS_THAN,
        ///            Predicate.Op.EQUAL, Predicate.Op.GREATER_THAN_OR_EQ, or
        ///            Predicate.Op.LESS_THAN_OR_EQ </param>
        /// <seealso cref= Predicate </seealso>
        public JoinPredicate(int index1, Predicate.Op op, int index2)
        {
            this.index1 = index1;
            this.index2 = index2;
            this.op = op;
        }

        /// <summary>
        /// 将谓词应用于两个指定的元组
        /// 可以通过Field的比较方法进行比较
        /// </summary>
        /// <returns> 如果元组满足谓词，则为True </returns>
        public virtual bool Filter(Tuple t1, Tuple t2)
        {
            return t1.GetField(index1).Compare(op, t2.GetField(index2));
        }

        public virtual int Index1
        {
            get
            {
                return index1;
            }
        }

        public virtual int Index2
        {
            get
            {
                return index2;
            }
        }

        public virtual Predicate.Op Operator
        {
            get
            {
                // some code goes here
                return null;
            }
        }
    }
}
