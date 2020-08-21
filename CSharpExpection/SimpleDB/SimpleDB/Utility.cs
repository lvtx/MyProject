﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SimpleDB
{
    public class Utility
    {
        /// <summary>
        /// 获取len长度的Type数组
        /// </summary>
        /// <param name="len"></param>
        /// <returns></returns>
        public static Type[] GetTypes(int len)
        {
            Type[] types = new Type[len];
            for (int i = 0; i < len; ++i)
                types[i] = typeof(INT_TYPE);
            return types;
        }

        /// <summary>
        /// 获取len长度的字符串数组
        /// </summary>
        /// <param name="len"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public static String[] GetStrings(int len, String val)
        {
            String[] strings = new String[len];
            for (int i = 0; i < len; ++i)
                strings[i] = val + i;
            return strings;
        }

        /// <summary>
        /// 其中n个字段的类型为int，
        /// 每个字段的名称均为name + n（name1，name2等）
        /// </summary>
        /// <param name="n"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static TupleDesc GetTupleDesc(int n, String name)
        {
            return new TupleDesc(GetTypes(n), GetStrings(n, name));
        }

        /// <summary>
        /// 返回N个类型为int的Field
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public static TupleDesc GetTupleDesc(int n)
        {
            return new TupleDesc(GetTypes(n));
        }
        /**
 * A utility method to create a new HeapFile with a single empty page,
 * assuming the path does not already exist. If the path exists, the file
 * will be overwritten. The new table will be added to the Catalog with
 * the specified number of columns as IntFields.
 */
        public static HeapFile CreateEmptyHeapFile(String path, int cols)
        {
            FileInfo f = new FileInfo(path);
            // touch the file
            BinaryWriter br = new BinaryWriter(File.OpenWrite(f.FullName));
            br.Write(new byte[0]);
            br.Close();

            HeapFile hf = OpenHeapFile(cols, f);
            HeapPageId pid = new HeapPageId(hf.GetId(), 0);

            HeapPage page = null;
            try
            {
                page = new HeapPage(pid, HeapPage.CreateEmptyPageData());
            }
            catch (IOException)
            {
                // this should never happen for an empty page; bail;
                throw new Exception("failed to create empty page in HeapFile");
            }

            hf.WritePage(page);
            return hf;
        }

        /** Opens a HeapFile and adds it to the catalog.
         *
         * @param cols number of columns in the table.
         * @param f location of the file storing the table.
         * @return the opened table.
         */
        public static HeapFile OpenHeapFile(int cols, FileInfo f)
        {
            // create the HeapFile and add it to the catalog
            TupleDesc td = GetTupleDesc(cols);
            HeapFile hf = new HeapFile(f, td);
            Database.GetCatalog().AddTable(hf, Guid.NewGuid().ToString());
            return hf;
        }

        public static HeapFile OpenHeapFile(int cols, String colPrefix, FileInfo f)
        {
            // create the HeapFile and add it to the catalog
            TupleDesc td = GetTupleDesc(cols, colPrefix);
            HeapFile hf = new HeapFile(f, td);
            Database.GetCatalog().AddTable(hf, Guid.NewGuid().ToString());
            return hf;
        }

        public static String ListToString(List<int> list)
        {
            string output = "";
            foreach (int i in list)
            {
                if (output.Length > 0) output += "\t";
                output += i;
            }
            return output;
        }
    }
}