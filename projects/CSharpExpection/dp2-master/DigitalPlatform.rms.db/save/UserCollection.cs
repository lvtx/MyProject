// #define DEBUG_LOCK

using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Xml;
using System.IO;
using System.Diagnostics;

using DigitalPlatform.rms;
using DigitalPlatform.Text;
using DigitalPlatform.Text.SectionPropertyString;
using DigitalPlatform.Xml;
using DigitalPlatform.ResultSet;
using DigitalPlatform.IO;

namespace DigitalPlatform.rms
{
    // �û�����
    public class UserCollection : List<User>
    {
        Thread threadWorker = null;
        AutoResetEvent eventClose = new AutoResetEvent(false);	// true : initial state is signaled 
        AutoResetEvent eventActive = new AutoResetEvent(false);	// �����ź�
        AutoResetEvent eventFinished = new AutoResetEvent(false);	// true : initial state is signaled 

        private int PerTime = 1 * 60 * 1000;	// 1����?


        private ReaderWriterLock m_lock = new ReaderWriterLock();
        private static int m_nTimeOut = 5000;

        internal DatabaseCollection m_dbs = null; // ���ݿ⼯��

        private int GreenMax = 10;   // ��ɫ�ߴ� ������ߴ�����, �����UseCountΪ0�Ķ���; �����ϲ��������
        private int YellowMax = 50;  // ��ɫ�ߴ� ����������ߴ�, �Ϳ�ʼ�������õĶ���
        private int RedMax = 100;   // ��ɫ�ߴ� ���Բ�����������ߴ硣����Ѿ��ﵽ����ߴ磬�����������һ����λ�󣬲��������¶�����뼯��

        public TimeSpan MaxLastUse = new TimeSpan(0, 30, 0);

        // ��ʼ���û����϶���
        // parameters:
        //      userDbs     �ʻ��⼯��
        //      strError    out���������س�����Ϣ
        // return:
        //      -1  ����
        //      0   �ɹ�
        // �ߣ���ȫ��
        public int Initial(DatabaseCollection dbs,
            out string strError)
        {
            strError = "";

            this.m_dbs = dbs;

            //*********���ʻ����ϼ�д��****************
            m_lock.AcquireWriterLock(m_nTimeOut);
#if DEBUG_LOCK
			this.m_dbs.WriteDebugInfo("Initial()�����û����ϼ�д����");
#endif
            try
            {
                // ��ճ�Ա
                this.Clear();

            }
            finally
            {
                m_lock.ReleaseWriterLock();  //��д��
#if DEBUG_LOCK
				this.m_dbs.WriteDebugInfo("Initial()�����û����Ͻ�д����");
#endif
            }

            StartWorkerThread();

            return 0;
        }

        public void Close()
        {
            eventClose.Set();	// ����߳��˳�

            // �ȴ������߳������˳�
            // ��Ϊ�������ڻ�д���ݿ�
            eventFinished.WaitOne(5000, false);

            /*
            // ��д��
            this.m_lock.AcquireWriterLock(m_nLockTimeout);
            try
            {
                // д�������ļ�
                this.Save(this.NewFileName());
            }
            finally
            {
                this.m_lock.ReleaseWriterLock();
            }
             */

        }

        // ���������߳�
        public void StartWorkerThread()
        {
            this.threadWorker =
                new Thread(new ThreadStart(this.ThreadMain));
            this.threadWorker.Start();
        }

        // ������߳�
        public void ActivateWorker()
        {
            this.eventActive.Set();
        }

        // �����߳�
        public void ThreadMain()
        {
            WaitHandle[] events = new WaitHandle[2];

            events[0] = eventClose;
            events[1] = eventActive;

            while (true)
            {
                int index = WaitHandle.WaitAny(events, PerTime, false);

                if (index == WaitHandle.WaitTimeout)
                {
                    eventActive.Reset();
                    // ��ʱ���������
                    this.Shrink();

                }
                else if (index == 0)
                {
                    break;
                }
                else
                {
                    eventActive.Reset();

                    // �õ�֪ͨ�����������
                    this.Shrink();
                }
            }

            eventFinished.Set();
        }

        // ֻ���û������в����û�����
        // parameters:
        //      strName     �û���
        //      user        out�����������û�����
        //      strError    out���������س�����Ϣ
        // return:
        //      -1  ����
        //      0   δ�ҵ�
        //      1   �ҵ�
        // �ߣ���ȫ
        private int GetUserFromCollection(string strName,
            out User user,
            out string strError)
        {
            user = null;
            strError = "";

            //*********���ʻ����ϼӶ���*****************
            this.m_lock.AcquireReaderLock(m_nTimeOut); //�Ӷ���
#if DEBUG_LOCK
			this.m_dbs.WriteDebugInfo("GetUserFromCollection()�����ʻ����ϼӶ�����");
#endif
            try
            {
                foreach (User oneUser in this)
                {
                    if (oneUser.Name == strName)
                    {
                        user = oneUser;
                        return 1;
                    }
                }

                return 0;
            }
            finally
            {
                //*****���ʻ����Ͻ����*******
                this.m_lock.ReleaseReaderLock();
#if DEBUG_LOCK
                this.m_dbs.WriteDebugInfo("GetUserFromCollection()�����ʻ����Ͻ������");
#endif
            }
        }

        // ����û�����
        // �ȴ��û��������ң�û���ٴ��û��⼯��������
        // ע��,�����������User����ɹ�, ���Ѿ�Ϊ���ü�����һ,������Ҫ�������Ժ���User����ʱ, ��Ҫ���ǽ���������ü�����һ
        // parameters:
        //      bIncreament �Ƿ�˳��Ϊ��������һ
        //      strName     �û���
        //      user        out�����������û�����
        //      strError    out���������س�����Ϣ
        // return:
        //      -1  ����
        //      0   δ�ҵ�
        //      1   �ҵ�
        // �ߣ���ȫ
        public int GetUser(
            bool bIncreament,
            string strName,
            out User user,
            out string strError)
        {
            user = null;
            strError = "";

            // ֻ���û������в����û�����
            // parameters:
            //      strName     �û���
            //      user        out�����������û�����
            //      strError    out���������س�����Ϣ
            // return:
            //      -1  ����
            //      0   δ�ҵ�
            //      1   �ҵ�
            // �ߣ���ȫ
            int nRet = this.GetUserFromCollection(strName,
                out user,
                out strError);
            if (nRet == -1)
                return -1;

            if (nRet == 1)
            {
                if (bIncreament == true)
                    user.PlusOneUse();

                user.Activate();    // �������ʹ��ʱ��

                return 1;
            }

            // return:
            //		-1	����
            //		0	δ�ҵ��ʻ�
            //		1	�ҵ���
            nRet = this.m_dbs.ShearchUser(strName,
                out user,
                out strError);
            if (nRet == -1)
                return -1;

            if (nRet == 0)
                return 0;

            Debug.Assert(user != null, "��ʱuser������Ϊnull");

            // ������ϸ������ý���
            if (this.RedMax <= 0)
            {
                user.container = this;  // ����Containerָ�뻹�����õ�
                return 1;
            }

            // �����ݿ�����, �����뼯��

            //*********���ʻ����ϼ�д��****************
            m_lock.AcquireWriterLock(m_nTimeOut);
#if DEBUG_LOCK
            this.m_dbs.WriteDebugInfo("GetUser()�����ʻ����ϼ�д����");
#endif
            try
            {

                // �ﵽ��ɫ�ߴ�
                if (this.Count >= this.RedMax)
                {
                    // �������������λ���������¶������
                    int delta = this.Count - this.RedMax + 1;

                    this.RemoveUserObjects(delta);
                }

                this.Add(user);
                user.PlusOneUse();  // ��Ϊ���Ƕ����ط����ϣ������������Ҫ���Ӽ��������Ǳ�����ǰ�ļ���ֵ�Ѿ��޴Ӳ鿼�����������Ը�1
                user.container = this;

                user.Activate();    // �������ʹ��ʱ��

                this.ActivateWorker();  // ֪ͨ�����̣߳���Ҫ����ߴ���

                return 1;
            }
            finally
            {
                m_lock.ReleaseWriterLock();  //��д��
#if DEBUG_LOCK
                this.m_dbs.WriteDebugInfo("GetUser()�����ʻ����Ͻ�д����");
#endif
            }
        }

        // ��¼
        // parameters:
        //      strUserName �û���
        //      strPassword ����
        //      user        out�����������û�����
        //      strError    out���������س�����Ϣ
        // return:
        //		-1	����
        //		0	�û��������ڣ������벻��ȷ
        //      1   �ɹ�
        // �ߣ���ȫ
        public int Login(string strUserName,
            string strPassword,
            out User user,
            out string strError)
        {
            user = null;
            strError = "";

            // Ϊ���ü�һ
            // return:
            //      -1  ����
            //      0   δ�ҵ�
            //      1   �ҵ�
            // �ߣ���ȫ
            int nRet = this.GetUser(
                true,
                strUserName,
                out user,
                out strError);
            if (nRet == -1)
                return -1;

            if (nRet == 1)
            {
                Debug.Assert(user != null, "��ʱuser������Ϊnull��");
                string strSHA1Password = Cryptography.GetSHA1(strPassword);
                if (user.SHA1Password == strSHA1Password)
                {
                    return 1;
                }
            }

            return 0;
        }

        // �ǳ�
        // return:
        //      -1  ����
        //      0   δ�ҵ�
        //      1   �ҵ������Ӽ��������
        public int Logout(
            string strName,
            out string strError)
        {
            // return:
            //      -1  ����
            //      0   δ�ҵ�
            //      1   �ҵ������Ӽ��������
            return ReleaseUser(
                strName,
                out strError);
        }

        // �ͷ�һ���ڴ�User��������ü���
        // �ߣ���ȫ
        // return:
        //      -1  ����
        //      0   δ�ҵ�
        //      1   �ҵ������Ӽ��������
        public int ReleaseUser(
            string strName,
            out string strError)
        {
            User user = null;
            strError = "";

            // ֻ���û������в����û�����
            // parameters:
            //      strName     �û���
            //      user        out�����������û�����
            //      strError    out���������س�����Ϣ
            // return:
            //      -1  ����
            //      0   δ�ҵ�
            //      1   �ҵ�
            // �ߣ���ȫ
            int nRet = this.GetUserFromCollection(strName,
                out user,
                out strError);
            if (nRet == -1)
                return -1;

            if (nRet == 0)
            {
                // �����в�����
                return 0;
            }

            Debug.Assert(user != null, "��ʱuser������Ϊnull");

            user.MinusOneUse();
            this.ActivateWorker();

            return 1;

            /* �����Ҫ, �����Ӽ�����ɾ��?
            //*********���ʻ����ϼ�д��****************
            m_lock.AcquireWriterLock(m_nTimeOut);
#if DEBUG_LOCK
            this.m_dbs.WriteDebugInfo("GetUser()�����ʻ����ϼ�д����");
#endif
            try
            {
             * if ???
                this.Remove(user);
                return 1;
            }
            finally
            {
                m_lock.ReleaseWriterLock();  //��д��
#if DEBUG_LOCK
                this.m_dbs.WriteDebugInfo("GetUser()�����ʻ����Ͻ�д����");
#endif
            }
             */
        }

        // �����ڴ��е��ʻ�����
        // �ߣ���ȫ
        // parameters:
        //      strRecPath  �ʻ���¼·��
        // return:
        //      0   not found
        //      1   found and removed
        public int RefreshUserSafety(
            string strRecPath)
        {
            //***************���ʻ����ϼ�д��*****************
            m_lock.AcquireWriterLock(m_nTimeOut);
#if DEBUG_LOCK
			this.m_dbs.WriteDebugInfo("RefreshUser()�����ʻ����ϼ�д����");
#endif
            try
            {
                foreach (User user in this)
                {
                    if (user.RecPath == strRecPath)
                    {
                        this.Remove(user);
                        return 0;
                    }
                }
                return 0;
            }
            finally
            {
                //***********���ʻ����Ͻ�д��*******************
                m_lock.ReleaseWriterLock();
#if DEBUG_LOCK
				this.m_dbs.WriteDebugInfo("RefreshUser()�����ʻ����Ͻ�д����");
#endif
            }
        }



        /*
        // ��ȡ���ݿ��м�¼������User����
        public int RefreshUser(
            string strRecordPath,
            User user,
            out string strError)
        {
            strError = "";

            // ����һ��DpPsthʵ��
            DbPath path = new DbPath(strRecordPath);

            // �ҵ�ָ���ʻ����ݿ�,��Ϊ���ݿ����п��ܲ���id��������DatabaseCollection.GetDatabase()
            Database db = this.m_dbs.GetDatabase(path.Name); //this.GetUserDatabaseByID(path.Name);
            if (db == null)
            {
                strError = "δ�ҵ���Ϊ'" + path.Name + "'�ʻ��⡣";
                return -1;
            }

            // ���ʻ������ҵ���¼
            string strXml;
            int nRet = db.GetXmlDataSafety(path.ID,
                out strXml,
                out strError);
            if (nRet <= -1)
                return -1;

            //���ص�dom
            XmlDocument dom = new XmlDocument();
            //dom.PreserveWhitespace = true; //��PreserveWhitespaceΪtrue
            try
            {
                dom.LoadXml(strXml);
            }
            catch (Exception ex)
            {
                strError = "����·��Ϊ'" + strRecordPath + "'���ʻ���¼��domʱ����,ԭ��:" + ex.Message;
                return -1;
            }

            int nOldUseCount = user.UseCount;

            nRet = user.Initial(strRecordPath,
                dom,
                db,
                out strError);
            if (nRet == -1)
                return -1;

            user.m_nUseCount = nOldUseCount;

            return 1;
        }
         */

        // �����Ҫ�������ڴ��е��ʻ��������ݿ�
        // �ߣ���ȫ
        // return:
        //		-1  ����
        //      -4  ��¼������
        //		0   �ɹ�
        public int SaveUserIfNeed(string strRecPath,
            out string strError)
        {
            strError = "";

            //***************���ʻ����ϼ�д��*****************
            m_lock.AcquireWriterLock(m_nTimeOut);
#if DEBUG_LOCK
			this.m_dbs.WriteDebugInfo("SaveUserSafety()�����ʻ����ϼ�д����");
#endif
            try
            {
                foreach (User user in this)
                {
                    if (user.RecPath == strRecPath)
                    {
                        // return:
                        //		-1  ����
                        //      -4  ��¼������
                        //		0   �ɹ�
                        int nRet = user.SaveChanges(out strError);
                        if (nRet <= -1)
                            return nRet;
                    }
                }
                return 0;
            }
            finally
            {
                //***********���ʻ����Ͻ�д��*******************
                m_lock.ReleaseWriterLock();
#if DEBUG_LOCK
				this.m_dbs.WriteDebugInfo("SaveUserSafety()�����ʻ����Ͻ�д����");
#endif
            }
        }

        // ����ڴ��û���������Χ�����Ƴ�ָ���Ķ���
        // parameters:
        //      user    �� ������
        //      strError    out���������س�����Ϣ
        // �ߣ���ȫ
        public void Shrink()
        {
            if (this.Count < this.GreenMax)
                return; // С����ɫ�ߴ磬�������ؽ���

            int nCount = 0;
            //***************���ʻ����ϼ�д��*****************
            m_lock.AcquireWriterLock(m_nTimeOut);
#if DEBUG_LOCK
			this.m_dbs.WriteDebugInfo("Shrink()�����ʻ����ϼ�д����");
#endif
            try
            {
                for (int i = 0; i < this.Count; i++)
                {
                    User user = this[i];

                    // usecount==0���
                    int nRet = Interlocked.Increment(ref user.m_nUseCount);
                    Interlocked.Decrement(ref user.m_nUseCount);
                    if (nRet <= 1)
                    {
                        this.RemoveAt(i);
                        continue;
                    }

                    // �������ʹ��ʱ���Ƿ񳬹�����
                    TimeSpan delta = DateTime.Now - user.m_timeLastUse;
                    if (delta > this.MaxLastUse)
                    {
                        this.RemoveAt(i);
                        continue;
                    }
                }

                nCount = this.Count;

            }
            finally
            {
                //***********���ʻ����Ͻ�д��*******************
                m_lock.ReleaseWriterLock();
#if DEBUG_LOCK
				this.m_dbs.WriteDebugInfo("Shrink()�����ʻ����Ͻ�д����");
#endif
            }

            // �������ߴ�
            if (nCount > this.YellowMax)
            {
                // ��ѡ��delta�����
                int delta = nCount - this.YellowMax;

                this.RemoveUserObjects(delta);

                /*
                m_lock.AcquireWriterLock(m_nTimeOut);
#if DEBUG_LOCK
			this.m_dbs.WriteDebugInfo("Shrink()�����ʻ����ϼ�д����");
#endif
                try
                {
                    for (int i = 0; i < delta; i++)
                    {
                        this.Remove(users[i]);
                    }
                }
                finally
                {
                    //***********���ʻ����Ͻ�д��*******************
                    m_lock.ReleaseWriterLock();
#if DEBUG_LOCK
				this.m_dbs.WriteDebugInfo("Shrink()�����ʻ����Ͻ�д����");
#endif
                }
                 */

            }


        }

        // �������ɸ�����õ�User����Ӽ������Ƴ�
        // �̰߳�ȫ
        void RemoveUserObjects(int nRemoveCount)
        {
            List<User> users = this.SortRecentUse();

            m_lock.AcquireWriterLock(m_nTimeOut);
#if DEBUG_LOCK
			this.m_dbs.WriteDebugInfo("Shrink()�����ʻ����ϼ�д����");
#endif
            try
            {
                for (int i = 0; i < nRemoveCount; i++)
                {
                    this.Remove(users[i]);
                }
            }
            finally
            {
                //***********���ʻ����Ͻ�д��*******************
                m_lock.ReleaseWriterLock();
#if DEBUG_LOCK
				this.m_dbs.WriteDebugInfo("Shrink()�����ʻ����Ͻ�д����");
#endif
            }
        }

        // ����޸�ʱ��Ƚ���
        public class UserComparer : IComparer<User>
        {
            DateTime m_now = DateTime.Now;

            // �ϵ���ǰ��
            int IComparer<User>.Compare(User x, User y)
            {
                // �������ʹ��ʱ��
                TimeSpan delta1 = m_now - x.m_timeLastUse;
                TimeSpan delta2 = m_now - y.m_timeLastUse;

                long lRet = delta1.Ticks - delta2.Ticks;
                if (lRet < 0)
                    return -1*-1;
                if (lRet == 0)
                    return 0;
                return -1*1;
            }

        }

        // Ϊ��̭�㷨��������
        public List<User> SortRecentUse()
        {
            // ���Ƴ�����
            List<User> aItem = new List<User>();

            // �Ӷ���
            this.m_lock.AcquireReaderLock(m_nTimeOut);
            try
            {
                aItem.AddRange(this);
            }
            finally
            {
                this.m_lock.ReleaseReaderLock();
            }

            UserComparer comp = new UserComparer();

            aItem.Sort(comp);

            return aItem;
        }

        /*
        // ���û����������һ���û�
        // parameters:
        //      user    �û�����
        //      strError    out���������س�����Ϣ
        // return:
        //      -1  ����
        //      0   �ɹ�
        // �ߣ�����ȫ��ȫ
        public int RemoveUser(User user,
            out string strError)
        {
            strError = "";

            Debug.Assert(user != null, "RemoveUser()���ô���user����ֵ����Ϊnull��");

            int nIndex = this.IndexOf(user);
            if (nIndex == -1)
            {
                strError = "RemoveUser()��user��Ȼ���Ǽ����еĳ�Ա���쳣��";
                return -1;
            }

            this.RemoveAt(nIndex);

            return 0;
        }
         */

        // ϵͳ����Ա�޸��û�����
        // parameters:
        //      user        ��ǰ�ʻ�
        //      strChangedUserName  ���޸��û���
        //      strNewPassword  ������
        //      strError    out���������س�����Ϣ
        // return:
        //      -1  ����
        //      -4  ��¼������
        //      -6  Ȩ�޲���
        //		0   �ɹ�
        public int ChangePassword(User user,
            string strChangedUserName,
            string strNewPassword,
            out string strError)
        {
            strError = "";

            User changedUser = null;

            // return:
            //		-1	����
            //		0	δ�ҵ��ʻ�
            //		1	�ҵ���
            // �ߣ���ȫ
            int nRet = this.GetUser(
                false,
                strChangedUserName,
                out changedUser,
                out strError);
            if (nRet == -1)
                return -1;

            if (nRet == 0)
            {
                strError = "û���ҵ�����Ϊ'" + strChangedUserName + "'���û�";
                return -1;
            }

            Debug.Assert(changedUser != null, "��ʱuserChanged���󲻿���Ϊnull,�����������ChangePassword()������");

            DbPath path = new DbPath(changedUser.RecPath);

            Database db = this.m_dbs.GetDatabase(path.Name);
            if (db == null)
            {
                strError = "δ�ҵ��ʻ�'" + strChangedUserName + "'���������ݿ⣬�쳣��";
                return -1;
            }
            // ???????�ϲ��Ͽ���������Կ���
            string strDbName = db.GetCaption("zh-cn");

            string strExistRights = "";
            bool bHasRight = user.HasRights(strDbName,
                ResType.Database,
                "changepassword",
                out strExistRights);
            if (bHasRight == false)
            {
                strError = "�����ʻ���Ϊ'" + user.Name + "'�����ʻ���Ϊ'" + strChangedUserName + "'�����������ݿ�'" + strDbName + "'û��'�޸ļ�¼����(changepassword)'��Ȩ�ޣ�Ŀǰ��Ȩ��ֵΪ'" + strExistRights + "'��";
                return -6;
            }

            // return:
            //      -1  ����
            //      -4  ��¼������
            //		0   �ɹ�
            return changedUser.ChangePassword(strNewPassword,
                out strError);
        }
    }

}
