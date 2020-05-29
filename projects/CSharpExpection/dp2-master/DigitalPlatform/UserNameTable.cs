﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Threading;

namespace DigitalPlatform
{
    /// <summary>
    /// 用户名信息表。用于防范试探密码的攻击
    /// </summary>
    public class UserNameTable
    {
        public string ServerName = "dp2library";

        public int FailCount = 10; // 尝试多少次登录失败后，会被禁止

        public int ShortPauseTicks = 2000;  // 短暂暂停的毫秒数

        internal ReaderWriterLockSlim m_lock = new ReaderWriterLockSlim();

        Hashtable _table = new Hashtable();

        public TimeSpan PauseTime = new TimeSpan(0, 10, 0);  // 一次禁止的时间长度 十分钟

        public UserNameTable(string strServerName)
        {
            this.ServerName = strServerName;
        }

#if NO
        public UserNameInfo GetInfo(string strUserName)
        {
            lock (_table)
            {
                return (UserNameInfo)_table[strUserName];
            }
        }

        public void DoDelay(UserNameInfo info)
        {
            if (info == null)
                return;

            if (info.AttackCount > 10)
                Thread.Sleep(1000 * (int)info.AttackCount);
        }
#endif

        // 登录前的例行检查
        // parameters:
        public int BeforeLogin(string strUserName,
            string strClientIP,
            out string strError)
        {
            strError = "";

            UserNameInfo info = null;
            string strKey = strUserName + "|" + strClientIP;
            this.m_lock.EnterReadLock();
            try
            {
                info = _table[strKey] as UserNameInfo;
                if (info == null)
                    return 0;
            }
            finally
            {
                this.m_lock.ExitReadLock();
            }

            if (info.AttackCount > FailCount)
            {
                if (DateTime.Now < info.RetryTime)
                {
                    strError = "前端 用户名 '" + strUserName + "' IP地址 '" + strClientIP + "' 因登录失败的次数太多，已被 " + this.ServerName + " 列入监控名单，禁止使用 Login() API";
                    Thread.Sleep(ShortPauseTicks);
                    return -1;
                }
            }

            if (DateTime.Now < info.RetryTime)
            {
                strError = "前端 用户名 '" + strUserName + "' IP地址 '" + strClientIP + "' 登录操作被暂时禁止。请于 " + info.RetryTime.ToShortTimeString() + " 以后重试登录";
                Thread.Sleep(ShortPauseTicks);
                return -1;
            }

            return 0;
        }

        // parameters:
        //      nLoginResult    1:成功 0:用户名或密码不正确 -1:出错
        public string AfterLogin(string strUserName,
            string strClientIP,
            int nLoginResult)
        {
            string strLogText = "";

            UserNameInfo info = null;
            string strKey = strUserName + "|" + strClientIP;
            this.m_lock.EnterWriteLock();
            try
            {
                info = _table[strKey] as UserNameInfo;
                if (info == null)
                {
                    if (nLoginResult == 1)
                        return null;

                    info = new UserNameInfo();
                    info.UserName = strUserName;
                    info.ClientIP = strClientIP;
                    info.AttackStart = DateTime.Now;
                    _table[strKey] = info;
                }
                else
                {
                    if (nLoginResult == 1)
                    {
                        _table.Remove(strKey);
                        return null;
                    }
                }

                if (info != null)
                    info.AttackCount++;
            }
            finally
            {
                this.m_lock.ExitWriteLock();
            }

            if (info != null && nLoginResult == 0)
            {
                if (info.AttackCount > FailCount)
                {
                    if (info.RetryTime == new DateTime(0))
                    {
                        info.RetryTime = DateTime.Now + this.PauseTime;
                    }
                    else
                    {
                        // 每多错误一次，追加惩罚十分钟
                        DateTime now = DateTime.Now;
                        if (info.RetryTime > now)   // 上次的结束时间比当前时刻靠后，用上次的结束时间作为基准
                            info.RetryTime += this.PauseTime;    
                        else
                            info.RetryTime = now + this.PauseTime;    // 否则用当前时间作为基准
                    }

                    strLogText = "前端 用户名 '"+strUserName+"' IP地址 '" + strClientIP + "' 从 "+info.AttackStart.ToString()+" 开始试探登录失败达 "+info.AttackCount+" 次，被暂时禁用登录功能，直到 " + info.RetryTime.ToString();
                }

                // 这一句，是为了让前端延缓下一次请求的速度。防范攻击的一种措施
                Thread.Sleep(ShortPauseTicks);
            }

            return strLogText;
        }
    }

    /// <summary>
    /// 一个用户名相关的信息
    /// </summary>
    public class UserNameInfo
    {
        public string UserName = "";    // 用户名
        public string ClientIP = "";    // 前端 IP
        public DateTime AttackStart = new DateTime(0); // 开始攻击的时间
        public DateTime RetryTime = new DateTime(0);    // 许可重新访问的时间。在这个时间以前不允许再使用 Login() API 了
        public long AttackCount = 0;    // 一共发生的攻击次数
    }
}
