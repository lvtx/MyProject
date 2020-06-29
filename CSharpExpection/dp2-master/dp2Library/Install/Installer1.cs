using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using System.Security.AccessControl;
using System.Diagnostics;
using System.Text;
using System.ServiceProcess;

using Microsoft.Win32;

using DigitalPlatform;
using DigitalPlatform.GUI;
using DigitalPlatform.Install;

using DigitalPlatform.Xml;
using DigitalPlatform.Text;
using DigitalPlatform.IO;
using DigitalPlatform.rms.Client;
using DigitalPlatform.LibraryServer;

namespace dp2Library
{
    [RunInstaller(true)]
    public partial class Installer1 : Installer
    {
        private System.ServiceProcess.ServiceProcessInstaller ServiceProcessInstaller1;
        private System.ServiceProcess.ServiceInstaller serviceInstaller1;

        public Installer1()
        {
            InitializeComponent();

            this.ServiceProcessInstaller1 = new System.ServiceProcess.ServiceProcessInstaller();
            this.serviceInstaller1 = new System.ServiceProcess.ServiceInstaller();
            // 
            // ServiceProcessInstaller1
            // 
            this.ServiceProcessInstaller1.Account = System.ServiceProcess.ServiceAccount.LocalSystem;  // LocalSystem
            this.ServiceProcessInstaller1.Password = null;
            this.ServiceProcessInstaller1.Username = null;
            // 
            // serviceInstaller1
            // 
            this.serviceInstaller1.DisplayName = "dp2 Library Service";
            this.serviceInstaller1.ServiceName = "dp2LibraryService";
            this.serviceInstaller1.Description = "dp2ͼ���Ӧ�÷�����������ƽ̨��������������ι�˾ http://dp2003.com";
            this.serviceInstaller1.StartType = ServiceStartMode.Automatic;
            /* // dp2Library��dp2Kernel���Բ���ͬһ̨���������Բ����趨������ϵ
            this.serviceInstaller1.ServicesDependedOn = new string[] {
                "dp2KernelService"};
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

            string strParameter = this.Context.Parameters["rootdir"];
            if (string.IsNullOrEmpty(strParameter) == true)
                return;

#if NO
            string strRootDir = UnQuote(this.Context.Parameters["rootdir"]);

            InstanceDialog dlg = new InstanceDialog();
            GuiUtil.AutoSetDefaultFont(dlg);

            dlg.SourceDir = strRootDir;
            dlg.StartPosition = FormStartPosition.CenterScreen;
            dlg.ShowDialog(ForegroundWindow.Instance);

            if (dlg.DialogResult == DialogResult.Cancel)
                throw new InstallException("�û�ȡ����װ��");

            if (dlg.Changed == true)
            {
                // �����޸�

            }
#endif
        }

        public override void Commit(System.Collections.IDictionary savedState)
        {
            base.Commit(savedState);

#if NO
            string strParameter = this.Context.Parameters["rootdir"];
            if (string.IsNullOrEmpty(strParameter) == true)
                return;
#endif

            // �����¼���־Ŀ¼
            if (!EventLog.SourceExists("dp2Library"))
            {
                EventLog.CreateEventSource("dp2Library", "DigitalPlatform");
            }
            EventLog Log = new EventLog();
            Log.Source = "dp2Library";

            // string strRootDir = UnQuote(this.Context.Parameters["rootdir"]);

            Log.WriteEntry("dp2library ��װ�ɹ���", EventLogEntryType.Information);
        }

        public override void Rollback(System.Collections.IDictionary savedState)
        {
            base.Rollback(savedState);

#if NO
            int nRet = 0;
            string strError = "";

            string strRootDir = UnQuote(this.Context.Parameters["rootdir"]);

            string strDataDir = (string)savedState["datadir"];
            string strDataDir_newly = (string)savedState["datadir_newly"];

            if (String.IsNullOrEmpty(strDataDir) == false
                && strDataDir_newly == "yes")
            {
            REDO_DELETE_DATADIR:
                // ɾ������Ŀ¼
                try
                {
                    Directory.Delete(strDataDir, true);
                }
                catch (Exception ex)
                {
                    DialogResult temp_result = MessageBox.Show(ForegroundWindow.Instance,
                        "ɾ������Ŀ¼'" + strDataDir + "'����" + ex.Message + "\r\n\r\n�Ƿ�����?\r\n\r\n(Retry: ����; Cancel: �����ԣ���������ж�ع���)",
                        "install dp2library -- �ع�",
                        MessageBoxButtons.RetryCancel,
                        MessageBoxIcon.Question,
                        MessageBoxDefaultButton.Button1);

                    if (temp_result == DialogResult.Retry)
                        goto REDO_DELETE_DATADIR;
                }
            }
#endif
        }

        public override void Uninstall(System.Collections.IDictionary savedState)
        {
            // Debug.Assert(false, "");

            base.Uninstall(savedState);

            string strParameter = this.Context.Parameters["rootdir"];
            if (string.IsNullOrEmpty(strParameter) == true)
                return;


#if NO
            String strRootDir = UnQuote(strParameter);

            DialogResult result;

            string strText = "�Ƿ���ȫж�أ�\r\n\r\n"
                + "����'��'�����ȫ��ʵ��������Ŀ¼ɾ�������еĿ�������Ϣ��ʧ�����е�ʵ����Ϣ��ʧ���Ժ�װʱ��Ҫ���°�װ����Ŀ¼�����ݿ⡣\r\n\r\n"
                + "����'��'����ɾ������Ŀ¼����ж��ִ�г����´ΰ�װʱ���Լ���ʹ���Ѵ��ڵĿ�������Ϣ��������װǰ��ж��Ӧѡ���";
            result = MessageBox.Show(ForegroundWindow.Instance,
                strText,
                "ж�� dp2Library",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2);
            if (result == DialogResult.Yes)
            {
                InstanceDialog dlg = new InstanceDialog();
                GuiUtil.AutoSetDefaultFont(dlg);
                dlg.Text = "����ж������ʵ��������Ŀ¼";
                dlg.Comment = "����ʵ������ȫ��ж�ء�����ϸȷ�ϡ�һ��ж�أ�ȫ������Ŀ¼��ʵ����Ϣ����ɾ���������޷��ָ���";
                dlg.UninstallMode = true;
                dlg.SourceDir = strRootDir;
                dlg.StartPosition = FormStartPosition.CenterScreen;
                dlg.ShowDialog(ForegroundWindow.Instance);

                if (dlg.DialogResult == DialogResult.Cancel)
                {
                    MessageBox.Show(ForegroundWindow.Instance,
                        "�ѷ���ж��ȫ��ʵ��������Ŀ¼������ж���˿�ִ�г���");
                }
            }
#endif
        }
    }
}