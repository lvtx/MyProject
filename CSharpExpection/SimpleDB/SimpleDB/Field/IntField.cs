using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDB
{
    /// <summary>
    /// 存储单个整数的字段实例
    /// </summary>
    public class IntField : Field
    {
        private static readonly long serialVersionUID = 1L;
        private readonly int value;
        public IntField(int i)
        {
            try
            {
                this.value = i;
            }
            catch (Exception ex)
            {
                Console.WriteLine("IntField初始化捕获:{0}",ex);
            }
        }
        public int GetValue()
        {
            return value;
        }
        public bool Compare(Predicate<OperatingSystem> op, Field value)
        {
            return false;
        }

        public void Serialize(BinaryWriter dos)
        {
            try
            {
                dos.Write(value);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public override string ToString()
        {
            return value.ToString();
        }
        public override bool Equals(object obj)
        {
            return ((IntField)obj).value == value;
        }
        public override int GetHashCode()
        {
            return value;
        }
        public Type CustomGetType()
        {
            return typeof(INT_TYPE);
        }
    }
}
