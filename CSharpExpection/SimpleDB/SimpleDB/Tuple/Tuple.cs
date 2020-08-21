using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SimpleDB
{
    /// <summary>
    /// 元组维护有关元组内容的信息。 
    /// 元组具有由TupleDesc对象指定的指定架构
    /// 并且包含Field对象以及每个字段的数据。
    /// </summary>
    public class Tuple
    {
        private static readonly long serialVersionUID = 1L;
        private TupleDesc td = null;
        private RecordId rid = null;
        private List<Field> fields = new List<Field>();
        /// <summary>
        /// 使用指定模式(schema)创建一个新元组
        /// </summary>
        /// <param name="td"></param>
        public Tuple(TupleDesc td)
        {
            this.td = td;
        }
        /// <summary>
        /// 表示此元组架构的TupleDesc
        /// </summary>
        /// <returns></returns>
        public TupleDesc GetTupleDesc()
        {
            return td;
        }
        /// <summary>
        /// RecordId表示此元组在磁盘上的位置(可能为空)
        /// </summary>
        /// <returns></returns>
        public RecordId GetRecordId()
        {
            return rid;
        }
        /// <summary>
        /// 设置该元组的RecordId信息
        /// </summary>
        /// <param name="rid"></param>
        public void SetRecordId(RecordId rid)
        {
            this.rid = rid;
        }
        /// <summary>
        /// 更改此元组的第i个字段的值
        /// </summary>
        /// <param name="i"></param>
        /// <param name="f"></param>
        public void SetField(int i,Field f)
        {
            try
            {
                fields.Insert(i, f);
            }
            catch (ArgumentOutOfRangeException)
            {
                Console.WriteLine("存储到数组时发生错误,请检查Tuple.SetField()");
            }
        }
        public Field GetField(int i)
        {
            try
            {
                if (fields[i] != null)
                {
                    return fields[i];
                }
            }
            catch (ArgumentOutOfRangeException)
            {
                Console.WriteLine("检索数组时发生错误,请检查Tuple.GetField()");
                return null;
            }
            return null;
        }
        public override String ToString()
        {
            string value = null;
            foreach (var field in fields)
            {
                value += field + ",";
            }
            return value;
        }
        public IEnumerable<Field> Fields()
        {
            for (int i = 0; i < fields.Count; i++)
            {
                yield return fields[i];
            }
        }
        public void ResetTupleDesc(TupleDesc td)
        {
            this.td = td;
        }
    }
}
