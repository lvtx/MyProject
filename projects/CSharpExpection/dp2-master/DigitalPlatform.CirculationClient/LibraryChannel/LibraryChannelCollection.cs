using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

using DigitalPlatform.LibraryClient;

namespace DigitalPlatform.CirculationClient
{
    /// <summary>
    /// dp2Library ��ǰ��ʵ�ÿ⡣Ŀǰ�� dp2Circulatio / dp2Catalog /dp2OPAC ������
    /// </summary>
    [System.Runtime.CompilerServices.CompilerGenerated]
    class NamespaceDoc
    {
    }

    // LibraryChannel���󼯺�
    // URL�Ƕ����Ψһ�Ա�־
    /// <summary>
    /// ͨѶͨ�����ϡ�Ҳ���� LibraryChannel ����ļ���
    /// </summary>
    public class LibraryChannelCollection : List<LibraryChannel>, IDisposable
    {
        /// <summary>
        /// ��¼ǰ�¼�
        /// </summary>
        public event BeforeLoginEventHandle BeforeLogin;

        /// <summary>
        /// ��¼���¼�
        /// </summary>
        public event AfterLoginEventHandle AfterLogin;

        public void Dispose()
        {
            this.Close();
            BeforeLogin = null;
            AfterLogin = null;
        }

        internal ReaderWriterLockSlim m_lock = new ReaderWriterLockSlim();
        internal static int m_nLockTimeout = 5000;	// 5000=5��

        // ����һ���µ�LibraryChannel���󣬶�������ǰ��û����ͬURL�Ķ���
        /// <summary>
        /// ����һ���µ� LibraryChannel ���󣬲��ܵ�ǰ��������ǰ��û����ͬ URL �Ķ���
        /// </summary>
        /// <param name="strUrl">dp2Library �������� URL</param>
        /// <returns>�´����� LibraryChannel ����</returns>
        public LibraryChannel NewChannel(string strUrl)
        {
            LibraryChannel channel = new LibraryChannel();
            channel.Url = strUrl;
            channel.BeforeLogin -= new BeforeLoginEventHandle(channel_BeforeLogin);
            channel.BeforeLogin += new BeforeLoginEventHandle(channel_BeforeLogin);

            channel.AfterLogin -= new AfterLoginEventHandle(channel_AfterLogin);
            channel.AfterLogin += new AfterLoginEventHandle(channel_AfterLogin);

            this.m_lock.EnterWriteLock();
            try
            {
                this.Add(channel);
            }
            finally
            {
                this.m_lock.ExitWriteLock();
            }

            return channel;
        }

        // 2015/1/1
        void channel_AfterLogin(object sender, AfterLoginEventArgs e)
        {
            // ֱ��ת��������
            if (this.AfterLogin != null)
                this.AfterLogin(sender, e);
        }

        // ɾ��һ��channel
        /// <summary>
        /// ɾ����ǰ�����е�һ�� LibraryChannel ����
        /// </summary>
        /// <param name="channel">Ҫɾ���� LibraryChannel ����</param>
        public void RemoveChannel(LibraryChannel channel)
        {
            this.m_lock.EnterWriteLock();
            try
            {
                int index = this.IndexOf(channel);
                if (index == -1)
                    return;

                channel.BeforeLogin -= new BeforeLoginEventHandle(channel_BeforeLogin);
                channel.AfterLogin -= new AfterLoginEventHandle(channel_AfterLogin);
                this.Remove(channel);
            }
            finally
            {
                this.m_lock.ExitWriteLock();
            }
        }

        // ��ú�URL��ص�һ��LibraryChannel����
        // ������󲻴��ڣ����Զ�����һ��
        /// <summary>
        /// ��ú�ָ�� URL ��ص�һ�� LibraryChannel ����������󲻴��ڣ����Զ�����һ���������뼯��
        /// </summary>
        /// <param name="strUrl">dp2Library �������� URL</param>
        /// <returns>LibraryChannel ����</returns>
        public LibraryChannel GetChannel(string strUrl)
        {
            this.m_lock.EnterWriteLock();
            try
            {
                LibraryChannel channel = this._findChannel(strUrl);

                if (channel != null)
                    return channel;

                // ���û���ҵ�
                channel = new LibraryChannel();
                channel.Url = strUrl;
                channel.BeforeLogin -= new BeforeLoginEventHandle(channel_BeforeLogin);
                channel.BeforeLogin += new BeforeLoginEventHandle(channel_BeforeLogin);

                channel.AfterLogin -= new AfterLoginEventHandle(channel_AfterLogin);
                channel.AfterLogin += new AfterLoginEventHandle(channel_AfterLogin);

                this.Add(channel);
                return channel;
            }
            finally
            {
                this.m_lock.ExitWriteLock();
            }
        }

        void channel_BeforeLogin(object sender,
            BeforeLoginEventArgs e)
        {
            // ֱ��ת��������
            if (this.BeforeLogin != null)
                this.BeforeLogin(sender, e);
        }

        // ����ָ��URL��LibraryChannel����
        /*public*/ LibraryChannel _findChannel(string strUrl)
        {
            foreach (LibraryChannel channel in this)
            {
                if (channel.Url == strUrl)
                    return channel;
            }

            return null;
        }

        /// <summary>
        /// ����ָ�� URL �� LibraryChannel ����
        /// </summary>
        /// <param name="strUrl">dp2Library �������� URL</param>
        /// <returns>LibraryChannel ����</returns>
        public LibraryChannel FindChannel(string strUrl)
        {
            this.m_lock.EnterReadLock();
            try
            {
                return _findChannel(strUrl);
            }
            finally
            {
                this.m_lock.ExitReadLock();
            }
        }

        /// <summary>
        /// �ӵ�ǰ�����йر�����ͨѶͨ����ɾ������ͨѶͨ������
        /// </summary>
        public void Close()
        {
            this.Clear(true);
        }

        /// <summary>
        /// ���ȫ��ͨ������
        /// </summary>
        /// <param name="bClose">���ǰ�Ƿ�ر�ͨ������</param>
        public void Clear(bool bClose = true)
        {
            this.m_lock.EnterWriteLock();
            try
            {
                if (bClose == true)
                {
                    foreach (LibraryChannel channel in this)
                    {
                        channel.Close();
                    }
                }
                base.Clear();
            }
            finally
            {
                this.m_lock.ExitWriteLock();
            }
        }
    }
}
