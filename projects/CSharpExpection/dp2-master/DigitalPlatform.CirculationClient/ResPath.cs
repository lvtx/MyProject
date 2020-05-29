﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace DigitalPlatform.CirculationClient
{
    /// <summary>
    /// 表现一个资源树节点的路径中 服务器 和 下级路径 2部分
    /// </summary>
    public class ResPath
    {
        public string Url = "";	// 服务器URL部分
        public string Path = "";	// 服务器内的资源节点路径。第一级是库名

        public ResPath()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public ResPath Clone()
        {
            ResPath newobj = new ResPath();
            newobj.Url = this.Url;
            newobj.Path = this.Path;
            return newobj;
        }

        // 根据树节点构造，把第一级作为Url内容，以下其它级合并起来当作Path，间隔以'/'
        public ResPath(TreeNode node)
        {
            string strFullPath = "";

            while (node != null)
            {
                if (node.Parent == null)
                {
                    dp2ServerNodeInfo info = (dp2ServerNodeInfo)node.Tag;
                    Debug.Assert(info != null, "");
                    Url = info.Url;
                    break;
                }
                else
                {
                    if (strFullPath != "")
                        strFullPath = "/" + strFullPath;
                    strFullPath = node.Text + strFullPath;
                }
                node = node.Parent;
            }
            Path = strFullPath;
        }

        // 以全路径构造
        public ResPath(string strFullPath)
        {
            SetFullPath(strFullPath);
        }

        // 只留下数据库名部分
        public void MakeDbName()
        {
            this.Path = GetDbName(this.Path);
        }

        // 从一个纯路径(不含url部分)中截取库名部分
        public static string GetDbName(string strLongPath)
        {
            int nRet = strLongPath.IndexOf("/");
            if (nRet == -1)
                return strLongPath;
            else
                return strLongPath.Substring(0, nRet);
        }

        // 从一个纯路径(不含url部分)中截取记录id部分
        public static string GetRecordId(string strLongPath)
        {
            int nRet = strLongPath.IndexOf("/");
            if (nRet == -1)
                return strLongPath;
            else
                return strLongPath.Substring(nRet + 1).Trim();
        }

        // 提取纯粹路径中的id部分。例如 this.Path为"数据库/1"，应提取出"1"
        public string GetRecordId()
        {
            string[] aPart = this.Path.Split(new char[] { '/' });

            if (aPart.Length < 2)
                return null;
            return aPart[1];
        }

        // 提取纯粹路径中的object id部分。例如 this.Path为"数据库/1/object/0"，应提取出"1"
        public string GetObjectId()
        {
            string[] aPart = this.Path.Split(new char[] { '/' });

            if (aPart.Length < 4)
                return null;
            return aPart[3];
        }

        // parameters:
        //		strPath	这是服务器URL和库以及其下路径合成的,中间间隔'?'
        public void SetFullPath(string strPath)
        {
            int nRet = strPath.IndexOf('?');

            if (nRet == -1)
            {
                Url = strPath.Trim();
                Path = "";
                return;
            }

            Url = strPath.Substring(0, nRet).Trim();
            Path = strPath.Substring(nRet + 1).Trim();
        }

        // 将反序的全路径灌入本对象
        // parameters:
        //		strPath	这是库以及其下路径 和 服务器URL 合成的,中间间隔'@'
        public void SetReverseFullPath(string strPath)
        {
            int nRet = strPath.IndexOf('@');

            if (nRet == -1)
            {
                Path = strPath.Trim();
                Url = "";
                return;
            }

            Path = strPath.Substring(0, nRet).Trim();
            Url = strPath.Substring(nRet + 1).Trim();
        }

        // 全路径。这是服务器URL和库以及其下路径合成的,中间间隔'?'
        public string FullPath
        {
            get
            {
                if (this.Path != "")
                    return this.Url + "?" + this.Path;
                return this.Url;
            }
            set
            {
                SetFullPath(value);
            }
        }

        public string ReverseFullPath
        {
            get
            {
                return this.Path + " @" + this.Url;
            }
            set
            {
                SetReverseFullPath(value);
            }
        }


        // 把反序全路径加工为正序全路径形态
        public static string GetRegularRecordPath(string strReverseRecordPath)
        {
            int nRet = strReverseRecordPath.IndexOf("@");
            if (nRet == -1)
                return strReverseRecordPath;
            return strReverseRecordPath.Substring(nRet + 1).Trim() + "?" + strReverseRecordPath.Substring(0, nRet).Trim();
        }

        // 把正序全路径形态加工为反序全路径形态
        public static string GetReverseRecordPath(string strRegularRecordPath)
        {
            int nRet = strRegularRecordPath.IndexOf("?");
            if (nRet == -1)
                return strRegularRecordPath;
            return strRegularRecordPath.Substring(nRet + 1).Trim() + " @" + strRegularRecordPath.Substring(0, nRet).Trim();
        }
    }
}
