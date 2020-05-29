using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Diagnostics;
using System.Windows.Forms;
using System.Xml;
using System.IO;

using System.ServiceProcess;

using DigitalPlatform;
using DigitalPlatform.Install;
using DigitalPlatform.GUI;
using DigitalPlatform.IO;
using DigitalPlatform.Xml;
using DigitalPlatform.Text;

namespace dp2ZServer
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : Installer
    {
        // string EncryptKey = "dp2zserver_password_key";

        private System.ServiceProcess.ServiceProcessInstaller ServiceProcessInstaller1;
        private System.ServiceProcess.ServiceInstaller serviceInstaller1;

        public ProjectInstaller()
        {
            InitializeComponent();

            this.ServiceProcessInstaller1 = new System.ServiceProcess.ServiceProcessInstaller();
            this.serviceInstaller1 = new System.ServiceProcess.ServiceInstaller();
            // 
            // ServiceProcessInstaller1
            // 
            this.ServiceProcessInstaller1.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            this.ServiceProcessInstaller1.Password = null;
            this.ServiceProcessInstaller1.Username = null;
            // 
            // serviceInstaller1
            // 
            this.serviceInstaller1.DisplayName = "dp2 Z39.50 Service";
            this.serviceInstaller1.ServiceName = "dp2ZService";
            this.serviceInstaller1.Description = "Z39.50������������ƽ̨��������������ι�˾ http://dp2003.com";
            this.serviceInstaller1.StartType = ServiceStartMode.Automatic;
            /*
            this.serviceInstaller1.ServicesDependedOn = new string[] {
                "W3SVC"};
             * */
            /* ��ΪZ39.50������������������������dp2Library
            this.serviceInstaller1.ServicesDependedOn = new string[] {
                "dp2LibraryService"};
             * */
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
												this.ServiceProcessInstaller1,
												this.serviceInstaller1});

            this.serviceInstaller1.Committed += new InstallEventHandler(serviceInstaller1_Committed);
        }

        void serviceInstaller1_Committed(object sender, InstallEventArgs e)
        {
            try
            {
                ServiceController sc = new ServiceController(this.serviceInstaller1.ServiceName);
                sc.Start();
            }
            catch (Exception ex)
            {
                // �������ǲ�ֹͣ��װ
                MessageBox.Show(ForegroundWindow.Instance,
                    "��װ�Ѿ���ɣ������� '" + this.serviceInstaller1.ServiceName + "' ʧ�ܣ� " + ex.Message);
            }
        }

        static string UnQuote(string strText)
        {
            return strText.Replace("'", "");
        }

        public override void Install(System.Collections.IDictionary stateSaver)
        {
            base.Install(stateSaver);

#if NO
            string strRootDir = UnQuote(this.Context.Parameters["rootdir"]);

            string strDataDir = "";

            bool bWriteInstanceInfo = false;
            bool bUpgrade = false;  // �Ƿ�Ϊ������װ? ��ν������װ���Ƿ�������Ŀ¼�Ѿ�������

            int nRet = 0;
            string strError = "";

            // ebug.Assert(false, "");

            bool bDialogOpened = false;
            InstallParamDlg param_dlg = new InstallParamDlg();
            GuiUtil.AutoSetDefaultFont(param_dlg);
            // ��XML�ļ���װ�����е���Ϣ���Ի���
            // return:
            //      -1  error
            //      0   not load
            //      1   loaded
            LoadExistingInfoFromDp2zserverXmlFile(
               param_dlg,
               strRootDir,
               out strError);

            string strInstanceName = "";
            string[] existing_urls = null;
            string strCertSN = "";
            // ���instance��Ϣ
            // parameters:
            //      urls ��ð󶨵�Urls
            // return:
            //      false   instanceû���ҵ�
            //      true    �ҵ�
            bool bRet = InstallHelper.GetInstanceInfo("dp2ZServer",
                0,
            out strInstanceName,
            out strDataDir,
            out existing_urls,
            out strCertSN);

            strDataDir = strRootDir;

            string strExistingXmlFile = PathUtil.MergePath(strRootDir, "unioncatalog.xml");
            if (File.Exists(strExistingXmlFile) == false)
            {

                param_dlg.ShowDialog(ForegroundWindow.Instance);

                if (param_dlg.DialogResult == DialogResult.Cancel)
                {
                    throw new Exception("��װ������");
                }

                bDialogOpened = true;

                // ����unioncatalog.xml�ļ�
                // return:
                //      -1  error, install faild
                //      0   succeed
                //      1   suceed, but some config ignored
                nRet = WriteUnionCatalogXmlFile(
                    param_dlg,
                    strRootDir,
                    out strError);
                if (nRet == -1)
                {
                    throw new Exception(strError);
                }
            }
            else
                bUpgrade = true;


        END1:

            // if (existing_urls == null || existing_urls.Length == 0)
            {
                string[] default_urls = new string[] {
                    //"net.tcp://localhost:7001/gcatserver/",
                    //"net.pipe://localhost/gcatserver/",
                    "http://localhost/unioncatalog/"
                };

                List<string> urls = new List<string>(existing_urls == null ? new string[0] : existing_urls);
                if (urls.Count == 0)
                {
                    urls.AddRange(default_urls);
                }

                WcfBindingDlg binding_dlg = new WcfBindingDlg();
                GuiUtil.AutoSetDefaultFont(binding_dlg);
                binding_dlg.Text = "��ָ�� UnionCatalogServer ��������ͨѶЭ��";
                binding_dlg.Urls = StringUtil.FromListString(urls);
                binding_dlg.DefaultUrls = default_urls;
                binding_dlg.NetPipeEnabled = false;
                binding_dlg.NetTcpEnabled = false;
                binding_dlg.HttpComment = "������Intranet��Internet";
                binding_dlg.StartPosition = FormStartPosition.CenterScreen;

            REDO_BINDING:
                if (binding_dlg.ShowDialog(ForegroundWindow.Instance) != DialogResult.OK)
                    throw new Exception("�û�ȡ����װ��");

                existing_urls = binding_dlg.Urls;

                // ����������Ʒ��bindings�Ƿ��ͻ
                // return:
                //      -1  ����
                //      0   ����
                //      1    �ظ�
                nRet = InstallHelper.IsGlobalBindingDup(string.Join(";", existing_urls),
                    "dp2ZServer",
                    out strError);
                if (nRet != 0)
                {
                    MessageBox.Show(ForegroundWindow.Instance, "Э���������: " + strError + "\r\n\r\n������ָ��Э���");
                    goto REDO_BINDING;
                }

                bWriteInstanceInfo = true;
            }

            if (bWriteInstanceInfo == true)
            {
                // ����instance��Ϣ
                InstallHelper.SetInstanceInfo(
                "dp2ZServer",
                0,
                "",
                strDataDir,
                existing_urls,
                strCertSN);
            }

            strExistingXmlFile = PathUtil.MergePath(strRootDir, "dp2zserver.xml");
            if (File.Exists(strExistingXmlFile) == false)
            {

                if (bDialogOpened == false)
                {
                    param_dlg.ShowDialog(ForegroundWindow.Instance);

                    if (param_dlg.DialogResult == DialogResult.Cancel)
                    {
                        throw new Exception("��װ������");
                    }

                    bDialogOpened = true;
                }
                // д��dp2zserver.xml�ļ�
                // return:
                //      -1  error, install faild
                //      0   succeed
                nRet = WriteDp2zserverXmlFile(
                    param_dlg,
                    strRootDir,
                    out strError);
                if (nRet == -1)
                {
                    throw new Exception(strError);
                }
            }
#endif
        }

#if NO
        // ����unioncatalog.xml�ļ�
        // return:
        //      -1  error, install faild
        //      0   succeed
        //      1   suceed, but some config ignored
        int WriteUnionCatalogXmlFile(
            InstallParamDlg dlg,
            string strRootDir,
            out string strError)
        {
            strError = "";

            string strXmlFileName = PathUtil.MergePath(strRootDir, "unioncatalog.xml");
            string strOriginXmlFileName = PathUtil.MergePath(strRootDir, "~unioncatalog.xml");

            string strTemp = "";

            XmlDocument dom = new XmlDocument();

            if (File.Exists(strXmlFileName) == true)
            {
                strTemp = strXmlFileName;
                try
                {
                    dom.Load(strXmlFileName);
                }
                catch (FileNotFoundException)
                {
                    dom.LoadXml("<root><libraryServer /></root>");
                }
                catch (Exception ex)
                {
                    strError = "XML�ļ� " + strXmlFileName + " װ�ص�XMLDOMʱ��������: " + ex.Message + "����װ����������޷���ɡ�";
                    return -1;
                }
            }
            else
            {
                strTemp = strOriginXmlFileName;

                try
                {
                    dom.Load(strOriginXmlFileName);
                }
                catch (FileNotFoundException)
                {
                    dom.LoadXml("<root><libraryServer /></root>");
                }
                catch (Exception ex)
                {
                    strError = "XML�ļ� " + strOriginXmlFileName + " װ�ص�XMLDOMʱ��������: " + ex.Message + "����װ����������޷���ɡ�";
                    return -1;
                }
            }

            XmlNode node = dom.DocumentElement.SelectSingleNode("libraryServer");

            // ��һ�Ѿ����ڵ��ļ��ǲ���ȷ��?
            if (node == null)
            {
                strError = "��װǰ�Ѿ����ڵ��ļ� " + strTemp + " ��ʽ����ȷ��ȱ��<libraryServer>Ԫ��";
                return -1;
            }

            Debug.Assert(node != null, "");

            DomUtil.SetAttr(node, "url", dlg.LibraryWsUrl);

            DomUtil.SetAttr(node, "username", dlg.UserName);
            DomUtil.SetAttr(node, "password", EncryptPassword(dlg.Password));

            try
            {
                dom.Save(strXmlFileName);
            }
            catch (Exception ex)
            {
                strError = "XML�ļ� " + strXmlFileName + " ����ʱ��������: " + ex.Message + "����װ����������޷���ɡ�";
                return -1;
            }

            return 0;
        }

#endif

#if NO
        // ��XML�ļ���װ�����е���Ϣ���Ի���
        // return:
        //      -1  error
        //      0   not load
        //      1   loaded
        int LoadExistingInfoFromDp2zserverXmlFile(
            InstallParamDlg dlg,
            string strRootDir,
            out string strError)
        {
            strError = "";

            string strXmlFileName = PathUtil.MergePath(strRootDir, "dp2zserver.xml");
            string strOriginXmlFileName = PathUtil.MergePath(strRootDir, "~dp2zserver.xml");

            string strTemp = "";

            XmlDocument dom = new XmlDocument();

            if (File.Exists(strXmlFileName) == true)
            {
                strTemp = strXmlFileName;
                try
                {
                    dom.Load(strXmlFileName);
                }
                catch (FileNotFoundException)
                {
                    dom.LoadXml("<root><libraryserver /></root>");
                }
                catch (Exception ex)
                {
                    strError = "XML�ļ� " + strXmlFileName + " װ�ص�XMLDOMʱ��������: " + ex.Message + "����װ����������޷���ɡ�";
                    return -1;
                }
            }
            else
            {
                strTemp = strOriginXmlFileName;

                try
                {
                    dom.Load(strOriginXmlFileName);
                }
                catch (FileNotFoundException)
                {
                    dom.LoadXml("<root><libraryserver /></root>");
                }
                catch (Exception ex)
                {
                    strError = "XML�ļ� " + strOriginXmlFileName + " װ�ص�XMLDOMʱ��������: " + ex.Message + "����װ����������޷���ɡ�";
                    return -1;
                }
            }

            XmlNode node = dom.DocumentElement.SelectSingleNode("libraryserver");

            // ��һ�Ѿ����ڵ��ļ��ǲ���ȷ��?
            if (node == null)
            {
                strError = "��װǰ�Ѿ����ڵ��ļ� " + strTemp + " ��ʽ����ȷ��";
                return -1;
            }

            Debug.Assert(node != null, "");

            string strUserName = DomUtil.GetAttr(node, "username");
            string strPassword = DomUtil.GetAttr(node, "password");
            strPassword = DecryptPasssword(strPassword);

            string strAnonymousUserName = DomUtil.GetAttr(node, "anonymousUserName");
            string strAnonymousPassword = DomUtil.GetAttr(node, "anonymousPassword");
            strAnonymousPassword = DecryptPasssword(strAnonymousPassword);

            string strUrl = DomUtil.GetAttr(node, "url");

            dlg.UserName = strUserName;
            dlg.Password = strPassword;
            dlg.AnonymousUserName = strAnonymousUserName;
            dlg.AnonymousPassword = strAnonymousPassword;

            if (String.IsNullOrEmpty(strUrl) == false)
                dlg.LibraryWsUrl = strUrl;

            return 1;
        }
#endif

        public override void Commit(System.Collections.IDictionary savedState)
        {
            base.Commit(savedState);

            string strRootDir = UnQuote(this.Context.Parameters["rootdir"]);

#if NO1111111111111
            // ����dp2zserver.xml�ļ�����

            int nRet = 0;
            string strError = "";

                    // д��XML�ļ�
        // return:
        //      -1  error, install faild
        //      0   succeed
        //      1   suceed, but some config ignored
            nRet = WriteXmlFile(strRootDir, 
                out strError);
            if (nRet == -1)
            {
                MessageBox.Show(ForegroundWindow.Instance,
                    strError);
                strError = "dp2ZServer��װδ���: " + strError;
            }
            else 
            {

                strError = "dp2ZServer��װ�ɹ���";
            }
#endif

            // �����¼���־Ŀ¼

            // Create the source, if it does not already exist.
            if (!EventLog.SourceExists("dp2ZServer"))
            {
                EventLog.CreateEventSource("dp2ZServer", "DigitalPlatform");
            }

            EventLog Log = new EventLog();
            Log.Source = "dp2ZServer";
            Log.WriteEntry("dp2ZServer��װ�ɹ���", EventLogEntryType.Information);
        }

#if NO
        // д��sp2zserver.xml�ļ�
        // return:
        //      -1  error, install faild
        //      0   succeed
        int WriteDp2zserverXmlFile(
            InstallParamDlg dlg,
            string strRootDir,
            out string strError)
        {
            strError = "";

            string strXmlFileName = PathUtil.MergePath(strRootDir, "dp2zserver.xml");
            string strOriginXmlFileName = PathUtil.MergePath(strRootDir, "~dp2zserver.xml");

            bool bExist = true;

            string strTemp = "";

            XmlDocument dom = new XmlDocument();

            if (File.Exists(strXmlFileName) == true)
            {
                strTemp = strXmlFileName;
                try
                {
                    dom.Load(strXmlFileName);
                }
                catch (FileNotFoundException)
                {
                    dom.LoadXml("<root><libraryserver /></root>");
                    bExist = false;
                }
                catch (Exception ex)
                {
                    strError = "XML�ļ� " + strXmlFileName + " װ�ص�XMLDOMʱ��������: " + ex.Message + "����װ����������޷���ɡ�";
                    return -1;
                }
            }
            else
            {
                strTemp = strOriginXmlFileName;

                bExist = false;

                try
                {
                    dom.Load(strOriginXmlFileName);
                }
                catch (FileNotFoundException)
                {
                    dom.LoadXml("<root><libraryserver /></root>");
                }
                catch (Exception ex)
                {
                    strError = "XML�ļ� " + strOriginXmlFileName + " װ�ص�XMLDOMʱ��������: " + ex.Message + "����װ����������޷���ɡ�";
                    return -1;
                }
            }


            XmlNode node = dom.DocumentElement.SelectSingleNode("libraryserver");

            // ��һ�Ѿ����ڵ��ļ��ǲ���ȷ��?
            if (node == null)
            {
                strError = "��װǰ�Ѿ����ڵ��ļ� " + strTemp + " ��ʽ����ȷ��";
                return -1;
                /*
                dom.LoadXml("<root><libraryserver /></root>");
                bExist = false;
                XmlNode node = dom.DocumentElement.SelectSingleNode("libraryserver");
                 * */
            }

            Debug.Assert(node != null, "");

#if NO1111111111111
            string strUserName = DomUtil.GetAttr(node, "username");
            string strPassword = DomUtil.GetAttr(node, "password");
            strPassword = DecryptPasssword(strPassword);

            string strAnonymousUserName = DomUtil.GetAttr(node, "anonymousUserName");
            string strAnonymousPassword = DomUtil.GetAttr(node, "anonymousPassword");
            strAnonymousPassword = DecryptPasssword(strAnonymousPassword);

            string strUrl = DomUtil.GetAttr(node, "url");

            InstallParamDlg dlg = new InstallParamDlg();
            InstallHelper.AutoSetDefaultFont(dlg);
            dlg.UserName = strUserName;
            dlg.Password = strPassword;
            dlg.AnonymousUserName = strAnonymousUserName;
            dlg.AnonymousPassword = strAnonymousPassword;

            if (String.IsNullOrEmpty(strUrl) == false)
                dlg.LibraryWsUrl = strUrl;

            dlg.ShowDialog(ForegroundWindow.Instance);

            if (dlg.DialogResult == DialogResult.Cancel)
            {
                if (bExist == true)
                    return 1;

                strError = "��������ָ�� dp2library �����ʻ� �� ������¼�ʻ�����װ��ɺ�����Ҫ�ֶ����� " + strXmlFileName + " �����ļ�������ϵͳ�����޷���������";
                return -1;
            }
#endif

            DomUtil.SetAttr(node, "url", dlg.LibraryWsUrl);

            DomUtil.SetAttr(node, "username", dlg.UserName);
            DomUtil.SetAttr(node, "password", EncryptPassword(dlg.Password));

            DomUtil.SetAttr(node, "anonymousUserName",
                String.IsNullOrEmpty(dlg.AnonymousUserName) == true ? null : dlg.AnonymousUserName);

            if (String.IsNullOrEmpty(dlg.AnonymousUserName) == true)
                DomUtil.SetAttr(node, "anonymousPassword", null);
            else
                DomUtil.SetAttr(node, "anonymousPassword", EncryptPassword(dlg.AnonymousPassword));

            try
            {
                dom.Save(strXmlFileName);
            }
            catch (Exception ex)
            {
                strError = "XML�ļ� " + strXmlFileName + " ����ʱ��������: " + ex.Message + "����װ����������޷���ɡ�";
                return -1;
            }

            return 0;
        }
#endif


#if NO111111111111
        // д��XML�ļ�
        // return:
        //      -1  error, install faild
        //      0   succeed
        //      1   suceed, but some config ignored
        int WriteXmlFile(out string strError)
        {
            strError = "";

            // Debug.Assert(false, "");

            string strDirectory = Environment.SystemDirectory;
            strDirectory = PathUtil.MergePath(strDirectory, "dp2zserver");


            string strXmlFileName = PathUtil.MergePath(strDirectory, "dp2zserver.xml");
            string strOriginXmlFileName = PathUtil.MergePath(strDirectory, "~dp2zserver.xml");

            bool bExist = true;

            string strTemp = "";

            XmlDocument dom = new XmlDocument();

            if (File.Exists(strXmlFileName) == true)
            {
                strTemp = strXmlFileName;
                try
                {
                    dom.Load(strXmlFileName);
                }
                catch (FileNotFoundException)
                {
                    dom.LoadXml("<root><libraryserver /></root>");
                    bExist = false;
                }
                catch (Exception ex)
                {
                    strError = "XML�ļ� " + strXmlFileName + " װ�ص�XMLDOMʱ��������: " + ex.Message + "����װ����������޷���ɡ�";
                    return -1;
                }
            }
            else
            {
                strTemp = strOriginXmlFileName;

                bExist = false;

                try
                {
                    dom.Load(strOriginXmlFileName);
                }
                catch (FileNotFoundException)
                {
                    dom.LoadXml("<root><libraryserver /></root>");
                }
                catch (Exception ex)
                {
                    strError = "XML�ļ� " + strOriginXmlFileName + " װ�ص�XMLDOMʱ��������: " + ex.Message + "����װ����������޷���ɡ�";
                    return -1;
                }
            }


            XmlNode node = dom.DocumentElement.SelectSingleNode("libraryserver");

            // ��һ�Ѿ����ڵ��ļ��ǲ���ȷ��?
            if (node == null)
            {
                strError = "��װǰ�Ѿ����ڵ��ļ� " + strTemp + " ��ʽ����ȷ��";
                return -1;
                /*
                dom.LoadXml("<root><libraryserver /></root>");
                bExist = false;
                XmlNode node = dom.DocumentElement.SelectSingleNode("libraryserver");
                 * */
            }

            Debug.Assert(node != null, "");

            string strUserName = DomUtil.GetAttr(node, "username");
            string strPassword = DomUtil.GetAttr(node, "password");
            strPassword = DecryptPasssword(strPassword);

            string strAnonymousUserName = DomUtil.GetAttr(node, "anonymousUserName");
            string strAnonymousPassword = DomUtil.GetAttr(node, "anonymousPassword");
            strAnonymousPassword = DecryptPasssword(strAnonymousPassword);

            string strUrl = DomUtil.GetAttr(node, "url");

            InstallParamDlg dlg = new InstallParamDlg();
            InstallHelper.AutoSetDefaultFont(dlg);
            dlg.UserName = strUserName;
            dlg.Password = strPassword;
            dlg.AnonymousUserName = strAnonymousUserName;
            dlg.AnonymousPassword = strAnonymousPassword;

            if (String.IsNullOrEmpty(strUrl) == false)
                dlg.LibraryWsUrl = strUrl;

            dlg.ShowDialog(ForegroundWindow.Instance);

            if (dlg.DialogResult == DialogResult.Cancel)
            {
                if (bExist == true)
                    return 1;

                strError = "��������ָ�� dp2library �����ʻ� �� ������¼�ʻ�����װ��ɺ�����Ҫ�ֶ����� "+strXmlFileName+" �����ļ�������ϵͳ�����޷���������";
                return -1;
            }

            DomUtil.SetAttr(node, "url", dlg.LibraryWsUrl);

            DomUtil.SetAttr(node, "username", dlg.UserName);
            DomUtil.SetAttr(node, "password", EncryptPassword(dlg.Password));

            DomUtil.SetAttr(node, "anonymousUserName", 
                String.IsNullOrEmpty(dlg.AnonymousUserName) == true ? null : dlg.AnonymousUserName);

            if (String.IsNullOrEmpty(dlg.AnonymousUserName) == true)
                DomUtil.SetAttr(node, "anonymousPassword", null);
            else
                DomUtil.SetAttr(node, "anonymousPassword", EncryptPassword(dlg.AnonymousPassword));

            try
            {
                dom.Save(strXmlFileName);
            }
            catch (Exception ex)
            {
                strError = "XML�ļ� " + strXmlFileName + " ����ʱ��������: " + ex.Message + "����װ����������޷���ɡ�";
                return -1;
            }

            return 0;
        }

#endif

#if NO
        public string DecryptPasssword(string strEncryptedText)
        {
            if (String.IsNullOrEmpty(strEncryptedText) == false)
            {
                try
                {
                    string strPassword = Cryptography.Decrypt(
        strEncryptedText,
        EncryptKey);
                    return strPassword;
                }
                catch
                {
                    return "errorpassword";
                }

            }

            return "";
        }

        public string EncryptPassword(string strPlainText)
        {
            return Cryptography.Encrypt(strPlainText, this.EncryptKey);
        }
#endif
    }
}