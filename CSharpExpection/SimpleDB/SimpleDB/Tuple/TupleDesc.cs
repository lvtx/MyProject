using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace SimpleDB
{
    /// <summary>
    /// TupleDesc 描述了元组的模式(Schema).
    /// 模式(schema)关于数据库和表的布局及特性的信息。
    /// </summary>
    public class TupleDesc
    {
        /// <summary>
        /// 辅助类,组织各个字段的信息
        /// </summary>
        public sealed class TDItem
        {
            private long serialVersionUID = 1L;
            //字段类型
            public readonly Type fieldType;
            //字段名
            public readonly string fieldName;
            public TDItem(Type t, string n)
            {
                this.fieldType = t;
                this.fieldName = n;
            }
            public override string ToString()
            {
                return fieldName + "(" + fieldType + ")";
            }
        }
        /// <summary>
        /// 存储多个TDItem
        /// </summary>
        private List<TDItem> tdItems = null;

        public List<TDItem> TDItems
        {
            get { return tdItems; }
        }

        /// <summary>
        /// 迭代此TupleDesc中包含的所有字段TDItem的迭代器
        /// </summary>
        /// <returns></returns>
        private IEnumerable<TDItem> iterator()
        {
            return tdItems;
        }
        private static readonly long serialVersionUID = 1L;
        /// <summary>
        /// 创建一个新的TupleDesc，其中带有指定类型的字段以及关联的命名字段
        /// </summary>
        /// <param name="typeAr">指定此TupleDesc中字段的数量和类型,
        /// 至少包含一个条目</param>
        /// <param name="fieldAr">指定字段名称的数组,可为空</param>
        public TupleDesc(Type[] typeAr, String[] fieldAr)
        {
            tdItems = new List<TDItem>();
            if (typeAr != null)
            {
                for (int i = 0; i < typeAr.Length; i++)
                {
                    TDItem tdItem = new TDItem(typeAr[i], fieldAr[i]);
                    tdItems.Add(tdItem);
                }
            }
        }
        /// <summary>
        /// 使用typeAr.length字段和指定类型的字段以及匿名（未命名）字段创建新的元组desc
        /// </summary>
        /// <param name="typeAr">指定此TupleDesc中字段的数量和类型,
        /// 至少包含一个条目</param>
        public TupleDesc(Type[] typeAr)
        {
            tdItems = new List<TDItem>();
            if (typeAr != null)
            {
                for (int i = 0; i < typeAr.Length; i++)
                {
                    TDItem tdItem = new TDItem(typeAr[i], null);
                    tdItems.Add(tdItem);
                }
            }
        }
        /// <summary>
        /// 获取TupleDesc中的Fields的数量
        /// </summary>
        /// <returns></returns>
        public int NumFields()
        {
            return tdItems.Count;
        }
        /// <summary>
        /// 获取此TupleDesc第i个Field的名字
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public String GetFieldName(int i)
        {
            try
            {
                string fieldName = tdItems[i - 1].fieldName;
                return fieldName;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// 获取此TupleDesc第i个Field的类型
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public Type GetFieldType(int i)
        {
            try
            {
                Type fieldType = tdItems[i].fieldType;
                return fieldType;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// 通过FieldName查找Index
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public int FieldNameToIndex(String name)
        {
            try
            {
                int i = 0;
                foreach (var tdItem in tdItems)
                {
                    if (name.Equals(tdItem.fieldName))
                    {
                        return i;
                    }
                    i++;
                }
                return -1;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// 元组大小(固定值)
        /// </summary>
        /// <returns></returns>
        public int GetSize()
        {
            int size = 0;
            foreach (var tdItem in tdItems)
            {
                size += tdItem.fieldType.GetLen();
            }
            return size;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="td1"></param>
        /// <param name="td2"></param>
        /// <returns></returns>

        public static TupleDesc Merge(TupleDesc td1, TupleDesc td2)
        {
            TupleDesc td = new TupleDesc(null, null);
            List<TDItem> tdItems = new List<TDItem>();
            tdItems = td1.tdItems;
            tdItems.AddRange(td2.tdItems);
            td.tdItems = tdItems;
            return td;
        }
        /// <summary>
        /// 比较指定对象与此TupleDesc是否相等。 
        /// 如果两个TupleDesc的大小相同并且该
        /// TupleDesc中的第n个类型等于td中的第n个类型，
        /// 则认为它们相等。
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public override bool Equals(Object o)
        {
            try
            {
                TupleDesc td = o as TupleDesc;
                bool isSize = this.GetSize() == td.GetSize();
                if (isSize == true && this.NumFields() == td.NumFields())
                {
                    for (int i = 0; i < this.NumFields(); i++)
                    {
                        if (tdItems[i].fieldType != td.tdItems[i].fieldType)
                        {
                            return false;
                        }
                    }
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
            return false;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        /// <summary>
        /// 返回描述此描述符的字符串。
        /// 它的格式应为"fieldType[0](fieldName[0])，...，fieldType[M](fieldName[M])"，
        /// 尽管确切的格式并不重要。
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string strDescript = null;
            for (int i = 0; i < this.NumFields(); i++)
            {
                if (i != this.NumFields() - 1)
                {
                    strDescript += string.Format(tdItems[i].fieldType.Name + "[" + i + "]" +
                    "(" + tdItems[i].fieldName + "[" + i + "]" + ")" + ",");
                }
                else
                {
                    strDescript += string.Format(tdItems[i].fieldType.Name + "[" + i + "]" +
                    "(" + tdItems[i].fieldName + "[" + i + "]" + ")");
                }
            }
            return strDescript;
        }
    }
}
