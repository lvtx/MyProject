using System;
using System.Windows.Forms;
using System.Drawing;

namespace DigitalPlatform.Xml
{
	//������TextBox
    public class MyEdit : TextBox
    {
        public XmlEditor XmlEditor = null;

        // ��ʼ��С�༭�ؼ�
        // parameters:
        //      xmlEditor   XmlEditor����
        // return:
        //      -1  ����
        //      0   �ɹ�
        public int Initial(XmlEditor xmlEditor,
            out string strError)
        {
            strError = "";
            this.XmlEditor = xmlEditor;

            this.ImeMode = ImeMode.Off;
            this.BorderStyle = BorderStyle.None;
            this.BackColor = this.XmlEditor.BackColorDefaultForEditable;
            this.Font = this.XmlEditor.FontTextDefault;
            this.Multiline = true;
            this.XmlEditor.Controls.Add(this);
            return 0;
        }


        // �ӹ�Ctrl+���ּ�
        protected override bool ProcessDialogKey(
            Keys keyData)
        {

            // Ctrl + A �Զ�¼�빦��
            if ((keyData & Keys.Control) == Keys.Control
                && (keyData & (~Keys.Control)) == Keys.A)   // 2007/5/15 �޸ģ�ԭ��������CTRL+C��CTRL+A�������ã�CTRL+C�Ǹ����á�
                // && (keyData & Keys.A) == Keys.A)
            {
                if (this.XmlEditor != null)
                {
                    GenerateDataEventArgs ea = new GenerateDataEventArgs();
                    this.XmlEditor.OnGenerateData(ea);
                    return true;
                }
            }

            return false;
        }


        // �����ʱ
        protected override void OnMouseWheel(MouseEventArgs e)
        {
            this.XmlEditor.MyOnMouseWheel(e);
        }

        // ��ý���
        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);

            this.XmlEditor.Enabled = true;
        }

        // ʧȥ����
        protected override void OnLostFocus(EventArgs e)
        {
            this.XmlEditor.Flush();
            base.OnLostFocus(e);
        }


        // �����������
        // ������Ƶ����ֶ�ʱ�������������Ƶ���һ��Item
        // ������Ƶ����ֵ�ʱ�������������Ƶ���һ��Item
        protected override void OnKeyDown(KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Up:
                    {
                        // ������Ƶ����ֶ�ʱ�������������Ƶ���һ��Item

                        // �õ�Edit�Ĳ����λ��
                        int x, y;
                        API.GetEditCurrentCaretPos(this,
                            out x,
                            out y);

                        // ���λ��Ϊ0
                        if (y == 0)
                        {
                            POINT point = new POINT();
                            point.x = 0;
                            point.y = 0;
                            bool bRet = API.GetCaretPos(ref point);  //�õ������

                            // �õ���ǰ��Item����һ��Item
                            Item frontItem = ItemUtil.GetNearItem(this.XmlEditor.m_selectedItem,
                                MoveMember.Front);

                            if (frontItem != null)
                            {
                                // ��Ϊ��ǰ��Item
                                //this.m_selectedItem = frontItem;
                                this.XmlEditor.SetCurText(frontItem, null);
                                this.XmlEditor.SetActiveItem(frontItem);
                                if (this.XmlEditor.m_curText != null)
                                {
                                    // �ֹ�����һ�£�ȷ���ƶ�����Ӧ��λ��
                                    point.y = this.ClientSize.Height - 2;
                                    API.SendMessage(this.Handle,
                                        API.WM_LBUTTONDOWN,
                                        new UIntPtr(API.MK_LBUTTON),	//	UIntPtr wParam,
                                        API.MakeLParam(point.x, point.y));

                                    API.SendMessage(this.Handle,
                                        API.WM_LBUTTONUP,
                                        new UIntPtr(API.MK_LBUTTON),	//	UIntPtr wParam,
                                        API.MakeLParam(point.x, point.y));
                                }
                                e.Handled = true;
                                this.XmlEditor.Invalidate();
                            }
                        }
                    }
                    break;
                case Keys.Down:
                    {
                        // ������Ƶ����ֵ�ʱ�������������Ƶ���һ��Item

                        int x, y;
                        API.GetEditCurrentCaretPos(this,
                            out x,
                            out y);

                        // �õ���ǰedit���к�
                        int nTemp = API.GetEditLines(this);


                        // ���Ǹ�Ԫ��
                        if (y >= nTemp - 1 && (!(this.XmlEditor.m_selectedItem is VirtualRootItem)))
                        {
                            POINT point = new POINT();
                            point.x = 0;
                            point.y = 0;
                            bool bRet = API.GetCaretPos(ref point);

                            // �õ���һ��Item
                            Item behindItem = ItemUtil.GetNearItem(this.XmlEditor.m_selectedItem,
                                MoveMember.Behind);

                            if (behindItem != null)
                            {
                                //this.m_selectedItem = behindItem; 
                                this.XmlEditor.SetCurText(behindItem, null);
                                this.XmlEditor.SetActiveItem(behindItem);



                                if (this.XmlEditor.m_curText != null)
                                {
                                    // ģ�ⵥ��һ��
                                    point.y = 1;
                                    API.SendMessage(this.Handle,
                                        API.WM_LBUTTONDOWN,
                                        new UIntPtr(API.MK_LBUTTON),	//	UIntPtr wParam,
                                        API.MakeLParam(point.x, point.y));

                                    API.SendMessage(this.Handle,
                                        API.WM_LBUTTONUP,
                                        new UIntPtr(API.MK_LBUTTON),	//	UIntPtr wParam,
                                        API.MakeLParam(point.x, point.y));
                                }
                                e.Handled = true;

                                this.XmlEditor.Invalidate();
                            }
                        }
                    }
                    break;
                case Keys.Left:
                    break;
                case Keys.Right:
                    break;
                default:
                    break;
            }
        }


        // �����ִ��������ʱ�����������
        protected override void OnKeyUp(KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Left:
                case Keys.Right:
                case Keys.Up:
                case Keys.Down:
                    break;

                default:
                    {
                        bool bChanged = false;

                        //���Edit�������ֱ�������
                        API.SendMessage(this.Handle,
                            API.EM_LINESCROLL,
                            0,
                            (int)1000);
                        while (true)
                        {
                            int nFirstLine = API.GetEditFirstVisibleLine(this); //�õ�edit�ɼ��ĵ�һ��

                            if (nFirstLine != 0) //������0�����������
                            {
                                bChanged = true;
                                this.Size = new Size(this.Size.Width,
                                    this.Size.Height + 10);
                            }
                            else
                            {
                                break;
                            }
                        }

                        if (bChanged)
                        {
                            this.AfterEditControlHeightChanged(this.XmlEditor.m_curText);
                        }

                    }
                    break;
            }
        }



        // edit�߶ȷ����仯
        private void AfterEditControlHeightChanged(Visual visual)
        {
            if (visual == null)
                return;
            int nOldHeight = 0;
            nOldHeight = visual.Rect.Height;

            //����������item
            // ??????����δ���������ݱ��xml���׳�����
            //this.EditControlTextToVisual();

            //���¼���
            int nRetWidth, nRetHeight;
            visual.Layout(visual.Rect.X,
                visual.Rect.Y,
                visual.Rect.Width,
                this.Size.Height,// + visual.TopBlank + visual.BottomBlank + 2*visual.BorderHorzHeight ,//visual.rect .Height ,
                this.XmlEditor.nTimeStampSeed++,
                out nRetWidth,
                out nRetHeight,
                LayoutMember.Layout);


            // ???????�赱ǰedit�Ĵ�С��λ��



            int nNewHeight = visual.Rect.Height;

            int nDelta = nNewHeight - nOldHeight;

            if (nDelta != 0)
            {
                //�ϼ�
                Visual containerVisual = visual.container;
                nNewHeight += containerVisual.TotalRestHeight;

                if (containerVisual != null)
                {
                    containerVisual.Layout(containerVisual.Rect.X,
                        containerVisual.Rect.Y,
                        containerVisual.Rect.Width,
                        nNewHeight,
                        this.XmlEditor.nTimeStampSeed,
                        out nRetWidth,
                        out nRetHeight,
                        LayoutMember.EnLargeHeight | LayoutMember.Up);
                }
                //Visual.ChangeSibling (visual,-1,nNewHeight);
            }

            if (nDelta != 0
                && this.XmlEditor.m_curText != null)
            {
                //�������ȣ�ʹ��ScrollWindowEx
            }

            if (nDelta != 0)
            {
                this.XmlEditor.AfterDocumentChanged(ScrollBarMember.Vert);
            }

            this.XmlEditor.Invalidate();

            //Ҳ��Ҫ���Ż�������
        }

    }
}
