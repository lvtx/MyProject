using System;
using System.Text;
using System.Collections.Generic;
using System.IO;
namespace SimpleDB
{
    /// <summary>
    /// HeapFileEncoder读取逗号分隔的文本文件或接受元组数组，
    /// 然后将其转换为针对simpledb堆页面的适当格式的二进制数据页面，
    /// 将页面填充到指定的长度，并在数据文件中连续写入。
    /// </summary>
    public class HeapFileEncoder
    {
        static int count = 0;      /// <summary>
        /// 将指定的元组列表（仅具有整数字段）转换为二进制页面文件
        /// 输出文件的格式将在HeapPage和HeapFile中指定
        /// 
        /// <see cref="HeapPage"/>
        /// <see cref="HeapFile"/>
        /// <param name="tuples">元组列表,每个元组都由整数列表表示，该整数是该元组的字段值</param>
        /// <param name="npagebytes">输出文件中每页的字节数</param>
        /// <param name="numFields">每个输入元组中的字段数</param>
        /// </summary>
        public static void Convert(List<List<int>> tuples, FileInfo outFile, int npagebytes, int numFields)
        {
            count += 1;
            string path = string.Format(@"D:\test\tempTable{0}.txt", count);
            FileInfo tempInput = new FileInfo(path);
            FileStream fs = tempInput.OpenWrite();
            //tempInput.deleteOnExit();
            BinaryWriter bw = new BinaryWriter(fs);
            //BufferedWriter bw = new BufferedWriter(new FileWriter(tempInput));
            foreach (List<int> tuple in tuples)
            {
                int writtenFields = 0;
                foreach (int field in tuple)
                {
                    writtenFields++;
                    if (writtenFields > numFields)
                    {
                        throw new Exception("Tuple has more than " + numFields + " fields: (" +
                                Utility.ListToString(tuple) + ")");
                    }
                    byte[] fieldAsByteArray = Encoding.Default.GetBytes(field.ToString());
                    bw.Write(fieldAsByteArray,0,fieldAsByteArray.Length);
                    if (writtenFields < numFields)
                    {
                        byte[] commaAsByteArray = Encoding.Default.GetBytes(",");
                        bw.Write(commaAsByteArray,0,commaAsByteArray.Length);
                    }
                }
                char[] endChar = new char[] { '\n' };
                byte[] endCharAsByteArray = Encoding.Default.GetBytes(endChar);
                bw.Write(endCharAsByteArray,0,endCharAsByteArray.Length);
            }
            bw.Close();
            Convert(tempInput, outFile, npagebytes, numFields);
        }

        public static void Convert(FileInfo inFile, FileInfo outFile, int npagebytes,
                   int numFields)
        {
            Type[] ts = new Type[numFields];
            for (int i = 0; i < ts.Length; i++)
            {
                ts[i] = typeof(INT_TYPE);
            }
            Convert(inFile, outFile, npagebytes, numFields, ts);
        }

        public static void Convert(FileInfo inFile, FileInfo outFile, int npagebytes,
                       int numFields, Type[] typeAr)
        {
            Convert(inFile, outFile, npagebytes, numFields, typeAr, ',');
        }

        /// <summery>
        /// 将指定的输入文本文件转换为二进制页面文件
        /// 假设输入文件的格式为(请注意，仅支持整数字段):
        /// int,...,int\n
        /// 其中每一行代表一个元组。
        /// 输出文件的格式将在HeapPage和HeapFile中指定
        /// 
        /// <see cref="HeapPage"/>
        /// <see cref="HeapFile"/>
        /// <paramref name="inFile">从中读取数据</paramref>
        /// <paramref name="outFile">将数据写入文件</paramref>
        /// <paramref name="npagebytes">输出文件中每页的字节数</paramref>
        /// <paramref name="numFields">每个输入行/输出元组中的字段数</paramref>
        /// </summery>
        public static void Convert(FileInfo inFile, FileInfo outFile, int npagebytes,
                       int numFields, Type[] typeAr, char fieldSeparator)
        {
            // 一个元组占用的字节数
            int nrecbytes = 0;
            for (int i = 0; i < numFields; i++)
            {
                nrecbytes += typeAr[i].GetLen();
            }
            int nrecords = (npagebytes * 8) / (nrecbytes * 8 + 1);  //floor comes for free

            //  per record, we need one bit; there are nrecords per page, so we need
            // nrecords bits, i.e., ((nrecords/32)+1) integers.
            // 每条记录，我们需要一位； 每页有n条记录，因此我们需要n条记录位，
            // 即((nrecords / 32)+1)个整数。
            int nheaderbytes = (nrecords / 8);
            if (nheaderbytes * 8 < nrecords)
                nheaderbytes++;  //ceiling
            int nheaderbits = nheaderbytes * 8;

            BinaryReader br = new BinaryReader(inFile.OpenRead());
            //BufferedReader br = new BufferedReader(new FileReader(inFile));
            FileStream os = outFile.OpenWrite();
            //FileOutputStream os = new FileOutputStream(outFile);

            // our numbers probably won't be much larger than 1024 digits
            // 我们的数字可能不会比1024位大很多
            char[] buf = new char[1024];
            
            int curpos = 0;//当前的位置
            int recordcount = 0;
            int npages = 0;
            int fieldNo = 0;

            MemoryStream headerMS = new MemoryStream(nheaderbytes);
            BinaryWriter headerBW = new BinaryWriter(headerMS);
            //ByteArrayOutputStream headerBAOS = new ByteArrayOutputStream(nheaderbytes);
            //DataOutputStream headerStream = new DataOutputStream(headerBAOS);
            MemoryStream pageMS = new MemoryStream(npagebytes);
            BinaryWriter pageBW = new BinaryWriter(pageMS);
            //ByteArrayOutputStream pageBAOS = new ByteArrayOutputStream(npagebytes);
            //DataOutputStream pageStream = new DataOutputStream(pageBAOS);
            //Console.WriteLine("标题占用的字节为{0}byte",nheaderbytes);
            bool done = false;
            bool first = true;
            while (!done)
            {
                int c = br.ReadByte();

                // Ignore Windows/Notepad special line endings
                // 忽略Windows/记事本特殊行的结尾
                if (c == '\r')
                    continue;

                if (c == '\n')
                {
                    if (first)
                        continue;
                    recordcount++;
                    first = true;
                }
                else
                    first = false;
                if (c == fieldSeparator || c == '\n' || c == '\r')
                {
                    string s = new string(buf, 0, curpos);
                    if (typeAr[fieldNo] == typeof(INT_TYPE))
                    {
                        try
                        {
                            //Console.WriteLine(int.Parse(s.Trim()));
                            pageBW.Write(int.Parse(s.Trim()));
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("BAD LINE : " + s);
                        }
                    }
                    else if (typeAr[fieldNo] == typeof(STRING_TYPE))
                    {
                        s = s.Trim();
                        int overflow = STRING_TYPE.LEN - s.Length;
                        if (overflow < 0)
                        {
                            //如果字符串长度过长 截断前128字节
                            string news = s.Substring(0, STRING_TYPE.LEN);
                            s = news;
                        }
                        pageBW.Write(s.Length);
                        // pageStream.writeInt(s.Length);
                        pageBW.Write(Encoding.Default.GetBytes(s));
                        // pageStream.writeBytes(s);
                        // 将长度不足128字节的补上0
                        while (overflow-- > 0)
                            pageBW.Write((byte)0);
                    }
                    curpos = 0;
                    if (c == '\n')
                        fieldNo = 0;
                    else
                        fieldNo++;
                }
                else
                {
                    buf[curpos++] = (char)c;
                    continue;
                }
                if (br.PeekChar() == -1)
                {
                    curpos--;
                    done = true;
                }
                // if we wrote a full page of records, or if we're done altogether,
                // write out the header of the page.
                //
                // in the header, write a 1 for bits that correspond to records we've
                // written and 0 for empty slots.
                //
                // when we're done, also flush the page to disk, but only if it has
                // records on it.  however, if this file is empty, do flush an empty
                // page to disk.
                // 如果我们写了一整页的记录，或者全部都写了，请写出页的header标记
                // 在头文件中，对应于我们所写记录的位写1，空槽写0
                // 当我们完成时，也将页刷新到磁盘，但仅当它有记录。
                // 但是，如果该文件为空，请将一个空页刷新到磁盘
                if (recordcount >= nrecords
                    || done && recordcount > 0
                    || done && npages == 0)
                {
                    int i = 0;
                    byte headerbyte = 0;

                    for (i = 0; i < nheaderbits; i++)
                    {
                        if (i < recordcount)
                            headerbyte |= (byte)(1 << (i % 8));

                        if (((i + 1) % 8) == 0)
                        {
                            headerBW.Write(headerbyte);
                            headerbyte = 0;
                        }
                    }

                    if (i % 8 > 0)
                        headerBW.Write(headerbyte);

                    // pad the rest of the page with zeroes

                    for (i = 0; i < (npagebytes - (recordcount * nrecbytes + nheaderbytes)); i++)
                        pageBW.Write((byte)0);

                    // write header and body to file
                    headerMS.WriteTo(os);
                    pageMS.WriteTo(os);

                    // reset header and body for next page
                    headerMS = new MemoryStream(nheaderbytes);
                    headerBW = new BinaryWriter(headerMS);
                    pageMS = new MemoryStream(npagebytes);
                    pageBW = new BinaryWriter(pageMS);

                    recordcount = 0;
                    npages++;
                }
            }
            br.Close();
            os.Close();
        }
    }
}
