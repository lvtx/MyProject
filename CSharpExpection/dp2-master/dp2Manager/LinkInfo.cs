using System;
using System.IO;
using System.Collections;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;


using DigitalPlatform.IO;
using DigitalPlatform.Text;
using DigitalPlatform.rms.Client;

namespace dp2Manager
{
    /// <summary>
    /// ������Ŀ¼�ͱ���Ŀ¼������Ϣ
    /// </summary>
    [Serializable()]
    public class LinkInfoCollection : ArrayList, IDisposable
    {
        [NonSerialized]
        string m_strFileName = "";

        [NonSerialized]
        bool m_bChanged = false;

        [NonSerialized]
        public RmsChannelCollection Channels = null;

        public bool Changed
        {
            get
            {
                return m_bChanged;
            }
            set
            {
                m_bChanged = value;
            }
        }

        public string FileName
        {
            get
            {
                return m_strFileName;
            }
            set
            {
                this.m_strFileName = value;
            }
        }

        public void Dispose()
        {
            if (this.Channels != null)
                this.Channels.Dispose();
        }

        // ������ �ַ���������
        public LinkInfo this[string strServerPath]
        {
            get
            {
                return this.GetLinkInfo(strServerPath);
            }
        }

        LinkInfo GetLinkInfo(string strServerPath)
        {
            LinkInfo linkInfo = null;
            for (int i = 0; i < this.Count; i++)
            {
                linkInfo = (LinkInfo)this[i];
                if (String.Compare(linkInfo.ServerPath, strServerPath, true) == 0)
                    return linkInfo;
            }

            return null;
        }

        // ���ļ���װ�ش���һ��ServerCollection����
        // parameters:
        //		bIgnorFileNotFound	�Ƿ��׳�FileNotFoundException�쳣��
        //							���==true������ֱ�ӷ���һ���µĿ�ServerCollection����
        // Exception:
        //			FileNotFoundException	�ļ�û�ҵ�
        //			SerializationException	�汾Ǩ��ʱ���׳���
        public static LinkInfoCollection Load(
            string strFileName,
            bool bIgnorFileNotFound)
        {
            LinkInfoCollection infos = null;

            try
            {
                using(Stream stream = File.Open(strFileName, FileMode.Open))
                {
                    BinaryFormatter formatter = new BinaryFormatter();

                    infos = (LinkInfoCollection)formatter.Deserialize(stream);
                    infos.m_strFileName = strFileName;

                    for (int i = 0; i < infos.Count; i++)
                    {
                        LinkInfo info = (LinkInfo)infos[i];
                        info.Container = infos;
                    }

                    return infos;
                }
            }
            catch (FileNotFoundException ex)
            {
                if (bIgnorFileNotFound == false)
                    throw ex;

                infos = new LinkInfoCollection();
                infos.m_strFileName = strFileName;

                // �õ�����һ���µĿն������
                return infos;
            }
        }

        // ���浽�ļ�
        // parameters:
        //		strFileName	�ļ��������==null,��ʾʹ��װ��ʱ������Ǹ��ļ���
        public void Save(string strFileName)
        {
            if (m_bChanged == false)
                return;

            if (strFileName == null)
                strFileName = m_strFileName;

            if (strFileName == null)
            {
                throw (new Exception("LinkInfoCollection.Save()û��ָ�������ļ���"));
            }

            Stream stream = File.Open(strFileName,
                FileMode.Create);

            BinaryFormatter formatter = new BinaryFormatter();

            formatter.Serialize(stream, this);
            stream.Close();
        }

        public int Link(out string strError)
        {
            strError = "";

            for (int i = 0; i < this.Count; i++)
            {
                LinkInfo info = (LinkInfo)this[i];
                int nRet = info.Link(out strError);
                if (nRet == -1)
                    return -1;
            }

            return 0;
        }

        public void Add(LinkInfo info)
        {
            info.Container = this;
            base.Add(info);
        }

    }

    [Serializable()]
    public class LinkInfo
    {
        public string ServerPath = "";
        public string LocalPath = "";

        public string[] Names = null;

        [NonSerialized]
        int m_nDownloading = 0;

        [NonSerialized]
        FileSystemWatcher watcher = null;

        [NonSerialized]
        public LinkInfoCollection Container = null;

        public void Stop()
        {
            watcher = null;
        }

        public int Link(out string strError)
        {
            strError = "";
            if (this.LocalPath == "")
            {
                strError = "����Ŀ¼��δ����,�޷���������";
                return -1;
            }

            try
            {
                PathUtil.TryCreateDir(this.LocalPath);
            }
            catch (Exception ex)
            {
                strError = "����Ŀ¼ '" + this.LocalPath + "' ʱ�����쳣: " + ex.Message;
                return -1;
            }

            watcher = new FileSystemWatcher();

            watcher.Path = this.LocalPath;

            /* Watch for changes in LastAccess and LastWrite times, and 
               the renaming of files or directories. */
            watcher.NotifyFilter = NotifyFilters.LastWrite;

            watcher.Filter = "*.*";

            // Add event handlers.
            watcher.Changed += new FileSystemEventHandler(OnChanged);

            // Begin watching.
            watcher.EnableRaisingEvents = true;

            return 0;
        }

        private void OnChanged(object source, FileSystemEventArgs e)
        {

            // ��ֹ���������ض�������ν������
            if (m_nDownloading > 0)
                return;

            if ((e.ChangeType & WatcherChangeTypes.Changed) != WatcherChangeTypes.Changed)
                return;

            string strPureName = PathUtil.PureName(e.FullPath);

            if (this.Names != null)
            {
                bool bFound = false;
                for (int i = 0; i < this.Names.Length; i++)
                {
                    if (String.Compare(strPureName, this.Names[i], true) == 0)
                    {
                        bFound = true;
                        break;
                    }
                }
                if (bFound == false)
                    return;	// �����ڷ�Χ�ڵ��ļ�
            }
            else
            {
                return;
            }

            // Specify what is done when a file is changed, created, or deleted.
            // MessageBox.Show("File: " +  e.FullPath + " " + e.ChangeType);

            Debug.Assert(this.Container != null, "");

            ResPath respath = new ResPath(this.ServerPath);

            RmsChannel channel = this.Container.Channels.GetChannel(respath.Url);

            Debug.Assert(channel != null, "Channels.GetChannel() �쳣");


            Stream stream = null;

            try
            {
                stream = File.Open(e.FullPath,
                    FileMode.Open,
                    FileAccess.Read, FileShare.ReadWrite);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }

            string strError = "";

            try
            {
                // ���������ļ�
                byte[] baOutputTimestamp = null;
                string strOutputPath = "";
                string strStyle = "ignorechecktimestamp";

                string strRange = "";
                if (stream != null && stream.Length != 0)
                {
                    Debug.Assert(stream.Length != 0, "test");
                    strRange = "0-" + Convert.ToString(stream.Length - 1);
                }

                string strServerPath = respath.Path + "/" + PathUtil.PureName(e.FullPath);

                long lRet = channel.DoSaveResObject(strServerPath,
                    stream,
                    (stream != null && stream.Length != 0) ? stream.Length : 0,
                    strStyle,
                    null,	// strMetadata,
                    strRange,
                    true,
                    null,	// this.TimeStamp,	// timestamp,
                    out baOutputTimestamp,
                    out strOutputPath,
                    out strError);


                if (lRet == -1)
                {
                    goto ERROR1;
                }

                // this.TimeStamp = baOutputTimestamp;

            }
            finally
            {
                stream.Close();
            }

            return;

        ERROR1:
            MessageBox.Show(strError);
            return;
        }

        public int DownloadFilesToLocalDir(out string strError)
        {
            strError = "";

            // �г������ļ�

            Debug.Assert(this.Container != null, "");

            m_nDownloading++;

            try
            {

                ResPath respath = new ResPath(this.ServerPath);

                RmsChannel channel = this.Container.Channels.GetChannel(respath.Url);

                Debug.Assert(channel != null, "Channels.GetChannel() �쳣");

                string[] items = null;

                long lRet = channel.DoDir(respath.Path,
                    "zh",
                    null,   // ����Ҫ�����������Ե�����
                    ResTree.RESTYPE_FILE,
                    out items,
                    out strError);
                if (lRet == -1)
                    return -1;

                this.Names = items;
                Container.Changed = true;

                for (int i = 0; i < items.Length; i++)
                {
                    string strName = items[i];

                    string strServerPath = respath.Path + "/" + strName;

                    byte[] baTimeStamp = null;
                    string strOutputPath;
                    string strMetaData;

                    // string strStyle = "attachment,data,timestamp,outputpath";

                    string strLocalFileName = this.LocalPath + "/" + strName;

                    lRet = channel.GetRes(
                        strServerPath,
                        strLocalFileName,
                        null,	// stop
                        out strMetaData,
                        out baTimeStamp,
                        out strOutputPath,
                        out strError);
                    if (lRet == -1)
                        return -1;


                }


                return 0;
            }
            finally
            {
                m_nDownloading--;
            }
        }

    }
}
