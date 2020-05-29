using System;
using System.Collections.Generic;
using System.Text;

using DigitalPlatform;
using DigitalPlatform.CirculationClient;
using DigitalPlatform.LibraryClient.localhost;
using DigitalPlatform.LibraryClient;
using DigitalPlatform.Text;

namespace dp2Catalog
{
    // TODO: �ѷ�������Ϣ�ۼ���������Ϊһ�����飬���������������Ϣ
    public class dp2ServerInfoCollection : List<dp2ServerInfo>
    {
        // ���һ������������Ϣ
        // ����ǰstop��Ҫ��OnStop +=
        public dp2ServerInfo GetServerInfo(
            Stop stop,
            bool bUseNewChannel,
            // LibraryChannelCollection Channels,
            string strServerName,
            string strServerUrl,
            bool bTestMode,
            out string strError)
        {
            strError = "";

            // �ȿ����Ƿ��Ѿ�����
            dp2ServerInfo info = this.Find(strServerUrl);

            if (info != null)
            {
                if (info.BiblioDbProperties != null)
                    return info;
            }
            else
            {
                // ��������ڣ�����ͼ����
                info = new dp2ServerInfo();
                info.TestMode = bTestMode;
                info.Url = strServerUrl;
                info.Name = strServerName;

                this.Add(info);
            }

            int nRet = info.Build(strServerName,
                strServerUrl,
                stop,
                bUseNewChannel,
#if OLD_CHANNEL
                Channels,
#endif
                out strError);
            if (nRet == -1)
            {
                info.BiblioDbProperties = null;
                return null;
            }

            return info;
        }

        // �����ض�URL������
        dp2ServerInfo Find(string strServerUrl)
        {
            for (int i = 0; i < this.Count; i++)
            {
                dp2ServerInfo info = this[i];
                if (info.Url == strServerUrl)
                    return info;
            }

            return null;
        }

    }


    // ��������Ϣ
    public class dp2ServerInfo
    {
        public string Url = ""; // URL
        public string Name = "";    // ������ʾ�ķ�������

        public List<BiblioDbProperty> BiblioDbProperties = null;

        public List<UtilDbProperty> UtilDbProperties = null;

        /// <summary>
        /// ��ǰ���ӵ� dp2Library �汾��
        /// </summary>
        public string Version = "0.0";  // 0 ��ʾ2.1���¡�2.1������ʱ�ž��еĻ�ȡ�汾�Ź���

        public bool TestMode = false;   // �Ƿ�Ϊ����ģʽ

        // ��ñ�Ŀ�������б�
        // ����ǰstop��Ҫ��OnStop +=
        // parameters:
        //      bUseNewChannel  �Ƿ�ʹ���µ�Channel�������==false����ʾ����ʹ����ǰ��
        public int Build(string strName,
            string strServerUrl,
            Stop stop,
            bool bUseNewChannel,
            // LibraryChannelCollection Channels,
            out string strError)
        {
            strError = "";
            int nRet = 0;

            this.Url = strServerUrl;
            this.Name = strName;

#if OLD_CHANNEL
            LibraryChannel Channel = null;
            
            if (bUseNewChannel == false)
                Channel = Channels.GetChannel(strServerUrl);
            else
                Channel = Channels.NewChannel(strServerUrl);
#endif
            LibraryChannel channel = Program.MainForm.GetChannel(strServerUrl);

            if (stop != null)
            {
                stop.SetMessage("���ڻ�ñ�Ŀ������ ...");
                /*
                stop.Initial("���ڻ�ñ�Ŀ������ ...");
                stop.BeginLoop();
                 * */
            }

            try
            {
                string version = "0.0";
                // return:
                //      -1  error
                //      0   dp2Library�İ汾�Ź��͡�������Ϣ��strError��
                //      1   dp2Library�汾�ŷ���Ҫ��
                nRet = LibraryChannel.GetServerVersion(
                    channel,
                    stop,
                    out version,
                    out strError);
                if (nRet != 1)
                    return -1;
                this.Version = version;

                if (this.TestMode == true && StringUtil.CompareVersion(this.Version, "2.34") < 0)
                {
                    strError = "dp2 ǰ�˵�����ģʽֻ���������ӵ� dp2library �汾Ϊ 2.34 ����ʱ����ʹ�� (��ǰ dp2library �汾Ϊ " + this.Version.ToString() + ")";
                    return -1;
                }

                this.BiblioDbProperties = new List<BiblioDbProperty>();

                string strValue = "";
                long lRet = channel.GetSystemParameter(stop,
                    "biblio",
                    "dbnames",
                    out strValue,
                    out strError);
                if (lRet == -1)
                {
                    strError = "��Է����� " + channel.Url + " ��ñ�Ŀ�����б���̷�������" + strError;
                    goto ERROR1;
                }

                string[] biblioDbNames = strValue.Split(new char[] { ',' });

                for (int i = 0; i < biblioDbNames.Length; i++)
                {
                    BiblioDbProperty property = new BiblioDbProperty();
                    property.DbName = biblioDbNames[i];
                    this.BiblioDbProperties.Add(property);
                }

                // ����﷨��ʽ
                lRet = channel.GetSystemParameter(stop,
                    "biblio",
                    "syntaxs",
                    out strValue,
                    out strError);
                if (lRet == -1)
                {
                    strError = "��Է����� " + channel.Url + " ��ñ�Ŀ�����ݸ�ʽ�б���̷�������" + strError;
                    goto ERROR1;
                }

                string[] syntaxs = strValue.Split(new char[] { ',' });

                if (syntaxs.Length != this.BiblioDbProperties.Count)
                {
                    strError = "��Է����� " + channel.Url + " ��ñ�Ŀ����Ϊ " + this.BiblioDbProperties.Count.ToString() + " ���������ݸ�ʽΪ " + syntaxs.Length.ToString() + " ����������һ��";
                    goto ERROR1;
                }

                // �������ݸ�ʽ
                for (int i = 0; i < this.BiblioDbProperties.Count; i++)
                {
                    this.BiblioDbProperties[i].Syntax = syntaxs[i];
                }


                ///

                // ��ö�Ӧ��ʵ�����
                lRet = channel.GetSystemParameter(stop,
                    "item",
                    "dbnames",
                    out strValue,
                    out strError);
                if (lRet == -1)
                {
                    strError = "��Է����� " + channel.Url + " ���ʵ������б���̷�������" + strError;
                    goto ERROR1;
                }

                string[] itemdbnames = strValue.Split(new char[] { ',' });

                if (itemdbnames.Length != this.BiblioDbProperties.Count)
                {
                    strError = "��Է����� " + channel.Url + " ��ñ�Ŀ����Ϊ " + this.BiblioDbProperties.Count.ToString() + " ������ʵ�����Ϊ " + itemdbnames.Length.ToString() + " ����������һ��";
                    goto ERROR1;
                }

                // �������ݸ�ʽ
                for (int i = 0; i < this.BiblioDbProperties.Count; i++)
                {
                    this.BiblioDbProperties[i].ItemDbName = itemdbnames[i];
                }


                // ��ö�Ӧ���ڿ���
                lRet = channel.GetSystemParameter(stop,
                    "issue",
                    "dbnames",
                    out strValue,
                    out strError);
                if (lRet == -1)
                {
                    strError = "��Է����� " + channel.Url + " ���ʵ������б���̷�������" + strError;
                    goto ERROR1;
                }

                string[] issuedbnames = strValue.Split(new char[] { ',' });

                if (issuedbnames.Length != this.BiblioDbProperties.Count)
                {
                    return 0; // TODO: ��ʱ�����档�Ƚ��������û���������dp2libraryws 2007/10/19�Ժ�İ汾�������پ���
                    /*
                    strError = "��Է����� " + Channel.Url + " ��ñ�Ŀ����Ϊ " + this.BiblioDbProperties.Count.ToString() + " �������ڿ���Ϊ " + issuedbnames.Length.ToString() + " ����������һ��";
                    goto ERROR1;
                     * */
                }

                // �������ݸ�ʽ
                for (int i = 0; i < this.BiblioDbProperties.Count; i++)
                {
                    this.BiblioDbProperties[i].IssueDbName = issuedbnames[i];
                }

                // ���ʵ�ÿ���Ϣ

                {
                    this.UtilDbProperties = new List<UtilDbProperty>();

                    lRet = channel.GetSystemParameter(stop,
                        "utilDb",
                        "dbnames",
                        out strValue,
                        out strError);
                    if (lRet == -1)
                    {
                        strError = "��Է����� " + channel.Url + " ���ʵ�ÿ����б���̷�������" + strError;
                        goto ERROR1;
                    }

                    string[] utilDbNames = strValue.Split(new char[] { ',' });

                    for (int i = 0; i < utilDbNames.Length; i++)
                    {
                        UtilDbProperty property = new UtilDbProperty();
                        property.DbName = utilDbNames[i];
                        this.UtilDbProperties.Add(property);
                    }

                    // �������
                    lRet = channel.GetSystemParameter(stop,
                        "utilDb",
                        "types",
                        out strValue,
                        out strError);
                    if (lRet == -1)
                    {
                        strError = "��Է����� " + channel.Url + " ���ʵ�ÿ����ݸ�ʽ�б���̷�������" + strError;
                        goto ERROR1;
                    }

                    string[] types = strValue.Split(new char[] { ',' });

                    if (types.Length != this.UtilDbProperties.Count)
                    {
                        strError = "��Է����� " + channel.Url + " ���ʵ�ÿ���Ϊ " + this.UtilDbProperties.Count.ToString() + " ����������Ϊ " + types.Length.ToString() + " ����������һ��";
                        goto ERROR1;
                    }

                    // �������ݸ�ʽ
                    for (int i = 0; i < this.UtilDbProperties.Count; i++)
                    {
                        this.UtilDbProperties[i].Type = types[i];
                    }

                }


                // MessageBox.Show(this, Convert.ToString(lRet) + " : " + strError);
            }
            finally
            {
                if (stop != null)
                {
                    /*
                    stop.EndLoop();
                    stop.Initial("");
                     * */
                }

                Program.MainForm.ReturnChannel(channel);
#if OLD_CHANNEL
                if (bUseNewChannel == true)
                {
                    Channels.RemoveChannel(Channel);
                    Channel = null;
                }
#endif
            }

            return 0;
        ERROR1:
            return -1;
        }




    }


    // ��Ŀ������
    public class BiblioDbProperty
    {
        public string DbName = "";  // ��Ŀ����
        public string Syntax = "";  // ��ʽ�﷨
        public string ItemDbName = "";  // ��Ӧ��ʵ�����

        public string IssueDbName = ""; // ��Ӧ���ڿ��� 2007/10/19
    }

    // ʵ�ÿ�����
    public class UtilDbProperty
    {
        public string DbName = "";  // ����
        public string Type = "";  // ���ͣ���;
    }
}
