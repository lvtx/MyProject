using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace DigitalPlatform
{
    public partial class ClipboardMonitor : Form
    {
        // for clipboard chain
        IntPtr nextClipboardViewer = (IntPtr)API.INVALID_HANDLE_VALUE;

        bool bChained = false;
        int nPrevent = 0;   // �����ֹ����

        public bool Ignore = false; // �Ƿ�(��ʱ)��ֹ�¼�(ClipboardChanged)֪ͨ���������ݱ仯

        public event ClipboardChangedEventHandle ClipboardChanged = null;

        public ClipboardMonitor()
        {
            InitializeComponent();
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            // TODO: Add custom paint code here

            // Calling the base class OnPaint
            base.OnPaint(pe);
        }

        public bool Chain
        {
            get
            {
                return this.bChained;
            }
            set
            {
                if (this.bChained == value)
                    return;
                if (value == true && this.Created == false)
                    this.CreateControl();

                ChainClipboard(value);
            }
        }

        void ChainClipboard(bool bOn)
        {
            if (bOn == true)
            {
                if (this.bChained == true)
                {
                    Debug.Assert(false, "�Ѿ�chain����");
                    return;
                }

                this.nPrevent++;    // ��ֹ��һ��

                nextClipboardViewer = (IntPtr)API.SetClipboardViewer((int)
                                       this.Handle);
                if (nextClipboardViewer == this.Handle)
                {
                    Debug.Assert(false, "���Լ��������ϵ���һ�����ھ�Ȼ��ͬһ��");
                }
                Debug.Assert(nextClipboardViewer != (IntPtr)API.INVALID_HANDLE_VALUE, "");

                this.bChained = true;

                this.nPrevent--;
            }
            else
            {
                if (this.bChained == true)
                {
                    Debug.Assert(nextClipboardViewer != (IntPtr)API.INVALID_HANDLE_VALUE);

                    API.ChangeClipboardChain(this.Handle, nextClipboardViewer);
                    this.bChained = false;
                }
                else
                {
                    Debug.Assert(false, "�Ѿ�����chain��");
                }
            }

        }


        protected override void
                  WndProc(ref System.Windows.Forms.Message m)
        {

            switch (m.Msg)
            {
                    /*
                case API.WM_CLOSE:
                    if (this.bChained == true)
                        ChainClipboard(false);
                    break;
                     */

                case API.WM_DRAWCLIPBOARD:
                    if (this.bChained == true)
                    {
                        if (this.ClipboardChanged != null && this.Ignore == false)
                        {
                            ClipboardChangedEventArgs e = new ClipboardChangedEventArgs();
                            this.ClipboardChanged(this, e);
                        }

                        // ��һ��API.SetClipboardViewer����, �ᴥ������¼�, �����Ҫ��bChained��������ֹ��һ��
                        API.SendMessage(nextClipboardViewer,
                            m.Msg,
                            m.WParam,
                            m.LParam);
                    }
                    break;

                case API.WM_CHANGECBCHAIN:
                    if (m.WParam == nextClipboardViewer)
                    {
                        nextClipboardViewer = m.LParam;
                    }
                    else
                    {
                        if (this.bChained == true)
                        {
                            API.SendMessage(nextClipboardViewer,
                                m.Msg,
                                m.WParam,
                                m.LParam);
                        }
                    }
                    break;

                default:
                    base.WndProc(ref m);
                    break;
            }
        }
    }

    // ���������ݷ����ı�
    public delegate void ClipboardChangedEventHandle(object sender,
    ClipboardChangedEventArgs e);

    public class ClipboardChangedEventArgs : EventArgs
    {
    }
}
