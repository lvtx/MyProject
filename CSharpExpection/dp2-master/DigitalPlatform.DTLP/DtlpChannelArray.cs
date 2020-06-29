using System;
using System.Collections;
using System.Windows.Forms;

using DigitalPlatform.Xml;

namespace DigitalPlatform.DTLP
{
    /*
	// ͨѶ����ʱ��
	// return:
	//		0	����
	//		1	ֹͣ
	public delegate int Delegate_Idle(HostEntry entry);
    */

    /*
	// ���ȱʡ�ʻ���Ϣ
	// return:
	//		2	already login succeed
	//		1	dialog return OK
	//		0	dialog return Cancel
	//		-1	other error
	public delegate int Delegate_AskAccountInfo(DtlpChannel channel, 
	string strPath,
	out IWin32Window owner,	// �����Ҫ���ֶԻ������ﷵ�ضԻ��������Form
	out string strUserName,
	out string strPassword);
     * */

	/// <summary>
	/// Summary description for ChannelArray.
	/// </summary>
	public class DtlpChannelArray : ArrayList
	{
		public ApplicationInfo appInfo = null;

		// ���ȱʡ�˻�������Ϣ�Ļص�������ַ
		// public Delegate_AskAccountInfo procAskAccountInfo = null;

        public event AskDtlpAccountInfoEventHandle AskAccountInfo = null;


		// ͨѶ����״̬�ص�����
		// public Delegate_Idle procIdle = null;
        public event DtlpIdleEventHandler Idle = null;

        public bool GUI = true;

		public DtlpChannelArray()
		{
			//
			// TODO: Add constructor logic here
			//
		}

        public bool DoIdle(object sender)
        {
            System.Threading.Thread.Sleep(1);	// ����CPU��Դ���Ⱥķ�

            bool bDoEvents = true;
            if (this.Idle != null)
            {
                DtlpIdleEventArgs e = new DtlpIdleEventArgs();
                this.Idle(sender, e);
                if (e.Stop == true)
                    return true;
                bDoEvents = e.bDoEvents;
            }

            if (bDoEvents == true)
            {
                try
                {
                    Application.DoEvents();	// ���ý������Ȩ
                }
                catch
                {
                }
            }

            System.Threading.Thread.Sleep(1);	// ����CPU��Դ���Ⱥķ�

            return false;
        }

		// ����һ����ͨ��
		public DtlpChannel CreateChannel(int usrid)
		{
			DtlpChannel channel = new DtlpChannel();

			channel.Container = this;
			channel.m_lUsrID = usrid;
			channel.InitialHostArray();
	
			this.Add(channel);

			return channel;
		}

		public bool DestroyChannel(DtlpChannel channel)
		{
			channel.Cancel();
			this.Remove(channel);
			return true;
		}

        // �Ƿ��Ѿ��ҽ����¼���
        public bool HasAskAccountInfoEventHandler
        {
            get
            {
                return (this.AskAccountInfo != null);
            }
        }

        // �����¼�
        public void CallAskAccountInfo(object obj,
            AskDtlpAccountInfoEventArgs e)
        {
            if (this.AskAccountInfo == null)
                return;

            this.AskAccountInfo(obj, e);
        }

	}

    // �¼�: ѯ���ʻ���Ϣ
    public delegate void AskDtlpAccountInfoEventHandle(object sender,
    AskDtlpAccountInfoEventArgs e);

    public class AskDtlpAccountInfoEventArgs : EventArgs
    {
        // �������
        public DtlpChannel Channel = null;
        public string Path = "";

        // �������
        public IWin32Window Owner = null;	// �����Ҫ���ֶԻ������ﷵ�ضԻ��������Form
        public string UserName = "";
        public string Password = "";

        public int Result = 0;
        // Result:
        //		2	already login succeed
        //		1	dialog return OK
        //		0	dialog return Cancel
        //		-1	other error
        public string ErrorInfo = "";
    }

    public delegate void DtlpIdleEventHandler(object sender,
DtlpIdleEventArgs e);


    public class DtlpIdleEventArgs : EventArgs
    {
        public bool bDoEvents = true;
        public bool Stop = false; 
    }
}
