﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

using DigitalPlatform.LibraryClient.localhost;
using DigitalPlatform.Text;
using DigitalPlatform.CirculationClient;
using DigitalPlatform;

namespace dp2Circulation
{
#if NO
    /// <summary>
    /// 快速获得书目记录信息
    /// </summary>
    internal class BiblioLoader : IEnumerable 
    {
        List<string> m_recpaths = new List<string>();

        public List<string> RecPaths
        {
            get
            {
                return this.m_recpaths;
            }
            set
            {
                this.m_recpaths = value;
            }
        }

        public string Format
        {
            get;
            set;
        }

        public LibraryChannel Channel
        {
            get;
            set;
        }

        public Stop Stop
        {
            get;
            set;
        }

        public GetBiblioInfoStyle GetBiblioInfoStyle
        {
            get;
            set;
        }

        public IEnumerator GetEnumerator()
        {
            List<string> format_list = new List<string>();
            int nContentIndex = format_list.Count;
            int nTimestampIndex = -1;
            int nMetadataIndex = -1;
            format_list.Add(this.Format);
            // if ((this.GetBiblioInfoStyle & dp2Circulation.GetBiblioInfoStyle.Timestamp) != 0)
            if (this.GetBiblioInfoStyle.HasFlag(GetBiblioInfoStyle.Timestamp) == true)  // 新用法
            {
                nTimestampIndex = format_list.Count;
                format_list.Add("timestamp");
            }
            if ((this.GetBiblioInfoStyle & dp2Circulation.GetBiblioInfoStyle.Metadata) != 0)
            {
                nMetadataIndex = format_list.Count;
                format_list.Add("metadata");
            }

            string[] formats = new string[format_list.Count];
            format_list.CopyTo(formats);

            List<string> batch = new List<string>();
            for (int index = 0; index < m_recpaths.Count; index++)
            {
                string s = m_recpaths[index];
                batch.Add(s);

                // 每100个一批，或者最后一次
                if (batch.Count >= 100 ||
                    (index == m_recpaths.Count - 1 && batch.Count > 0))
                {
                REDO:
                    string strCommand = "@path-list:" + StringUtil.MakePathList(batch);

                    string[] results = null;
                    byte[] timestamp = null;
                    string strError = "";
                    long lRet = Channel.GetBiblioInfos(
                        this.Stop,
                        strCommand,
                        "",
                        formats,
                        out results,
                        out timestamp,
                        out strError);
                    if (lRet == -1)
                    {
                        throw new ChannelException(Channel.ErrorCode, strError);
                    }

                    if (lRet == 0)
                    {
                        if (lRet == 0 && String.IsNullOrEmpty(strError) == true)
                        {
                            foreach (string path in batch)
                            {
                                BiblioItem item = new BiblioItem();
                                item.RecPath = path;
                                item.ErrorCode = ErrorCode.NotFound;
                                item.ErrorInfo = "书目记录 '" + path + "' 不存在";
                                yield return item;
                            }
                            goto CONTINUE;
                        }


                        // 如果results.Length表现正常，其实还可以继续处理
                        if (results != null && results.Length > 0)
                        {
                        }
                        else
                        {
                            strError = "获得书目记录 '" + StringUtil.MakePathList(batch) + "' 时发生错误: " + strError;
                            throw new Exception(strError);
                        }
                    }

                    if (results == null)
                    {
                        strError = "results == null";
                        throw new Exception(strError);
                    }

                    for (int i = 0; i < results.Length / formats.Length; i++)
                    {
                        BiblioItem item = new BiblioItem();
                        item.RecPath = batch[i];
                        if (nContentIndex != -1)
                            item.Content = results[i * formats.Length + nContentIndex];
                        if (nTimestampIndex != -1)
                            item.Timestamp = ByteArray.GetTimeStampByteArray(results[i * formats.Length + nTimestampIndex]);
                        if (nMetadataIndex != -1)
                            item.Metadata = results[i * formats.Length + nMetadataIndex];
                        if (string.IsNullOrEmpty(item.Content) == true)
                        {
                            item.ErrorCode = ErrorCode.NotFound;
                            item.ErrorInfo = "书目记录 '" + item.RecPath + "' 不存在";
                        }
                        yield return item;

                    }

                CONTINUE:
                    if (batch.Count > results.Length / formats.Length)
                    {
                        // 有本次没有获取到的记录
                        batch.RemoveRange(0, results.Length / formats.Length);
                        if (index == m_recpaths.Count - 1)
                            goto REDO;  // 当前已经是最后一轮了，需要继续做完

                        // 否则可以留给下一轮处理
                    }
                    else
                        batch.Clear();
                }
            }
        }
    }

    /// <summary>
    /// 书目信息事项
    /// </summary>
    public class BiblioItem
    {
        /// <summary>
        /// 记录路径
        /// </summary>
        public string RecPath = "";
        /// <summary>
        /// 记录内容
        /// </summary>
        public string Content = "";
        /// <summary>
        /// 时间戳
        /// </summary>
        public byte[] Timestamp = null;
        /// <summary>
        /// 记录元数据
        /// </summary>
        public string Metadata = "";

        /// <summary>
        /// 错误码
        /// </summary>
        public ErrorCode ErrorCode = ErrorCode.NoError;
        /// <summary>
        /// 错误信息字符串
        /// </summary>
        public string ErrorInfo = "";
    }

    // 
    /// <summary>
    /// 获取书目信息的风格。要获取哪些附加信息?
    /// </summary>
    [Flags]
    public enum GetBiblioInfoStyle
    {
        /// <summary>
        /// 不获取 Timestamp 和 Metadata 部分
        /// </summary>
        None = 0,   // 不获取 Timestamp 和 Metadata 部分
        /// <summary>
        /// 要获取 Timestamp 部分
        /// </summary>
        Timestamp = 0x01,   // 要获取 Timestamp 部分
        /// <summary>
        /// 要获取 Metadata 部分
        /// </summary>
        Metadata = 0x02,    // 要获取 Metadata 部分
    }

    /// <summary>
    /// dp2Library 通讯访问异常
    /// </summary>
    public class ChannelException : Exception
    {
        /// <summary>
        /// 错误码
        /// </summary>
        public ErrorCode ErrorCode = ErrorCode.NoError;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="error"></param>
        /// <param name="strText"></param>
        public ChannelException(ErrorCode error,
            string strText) : base(strText)
        {
            this.ErrorCode = error;
        }
    }

#endif
}
