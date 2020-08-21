using System;
using System.IO;

namespace SimpleDB
{
    public struct INT_TYPE
    {
        public static int GetLen()
        {
            return 4;
        }
        public static Field Parse(BinaryReader br)
        {
            try
            {
                return new IntField(br.ReadInt32());
            }
            catch (IOException ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
    public struct STRING_TYPE
    {
        public const int LEN = 128;
        public static int GetLen()
        {
            return LEN + 4;
        }
        public static Field Parse(StreamReader br)
        {
            return null;
        }
    }
    /// <summary>
    /// 用于获取自定义类型长度的扩展方法
    /// </summary>
    public static class TypeUtil
    {
        public static int GetLen(this Type type)
        {
            switch (type.Name)
            {
                case "INT_TYPE":
                    return 4;
                default:
                    return 0;
            }
        }
        public static Field Parse(this Type type,BinaryReader br)
        {
            switch (type.Name)
            {
                case "INT_TYPE":
                    return INT_TYPE.Parse(br);
                default:
                    return null;
            }
        }

        public static int STRING_LEN(this Type type) { return 128; }
    }
}
