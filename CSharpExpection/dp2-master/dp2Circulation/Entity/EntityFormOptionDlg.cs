﻿using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml;

using DigitalPlatform;
using DigitalPlatform.Text;
using DigitalPlatform.Xml;

namespace dp2Circulation
{
    internal partial class EntityFormOptionDlg : Form
    {
        /// <summary>
        /// 框架窗口
        /// </summary>
        // public MainForm MainForm = null;

        public string DisplayStyle = "all"; // 要只显示哪些 page? 目前可用 all / quick_entity

        public EntityFormOptionDlg()
        {
            InitializeComponent();

            this.tabPage_quickEntityRegisterDefault.Tag = "quick_entity";
            this.tabPage_quickIssueRegisterDefault.Tag = "quick_issue";
            this.tabPage_normalEntityRegisterDefault.Tag = "normal_entity";
            this.tabPage_normalIssueRegisterDefault.Tag = "normal_issue";
        }

        private void EntityFormOptionDlg_Load(object sender, EventArgs e)
        {
            if (Program.MainForm != null)
            {
                MainForm.SetControlFont(this, Program.MainForm.DefaultFont);
            }

            string strError = "";

            // 册一般登记
            {
                string strNormalDefault = Program.MainForm.AppInfo.GetString(
                    "entityform_optiondlg",
                    "normalRegister_default",
                    "<root />");
                int nRet = this.entityEditControl_normalRegisterDefault.SetData(strNormalDefault,
                     "",
                     null,
                     out strError);
                if (nRet == -1)
                    MessageBox.Show(this, strError);

                this.entityEditControl_normalRegisterDefault.SetReadOnly("librarian");
                this.entityEditControl_normalRegisterDefault.GetValueTable += new GetValueTableEventHandler(entityEditControl_GetValueTable);

                this.checkBox_normalRegister_simple.Checked = Program.MainForm.AppInfo.GetBoolean(
    "entityform_optiondlg",
    "normalRegister_simple",
    false);
            }

            // 册快速登记
            {
                string strQuickDefault = Program.MainForm.AppInfo.GetString(
                    "entityform_optiondlg",
                    "quickRegister_default",
                    "<root />");
                int nRet = this.entityEditControl_quickRegisterDefault.SetData(strQuickDefault,
                     "",
                     null,
                     out strError);
                if (nRet == -1)
                    MessageBox.Show(this, strError);

                this.entityEditControl_quickRegisterDefault.SetReadOnly("librarian");
                this.entityEditControl_quickRegisterDefault.GetValueTable += new GetValueTableEventHandler(entityEditControl_GetValueTable);


            }

            // 期一般登记
            {
                string strIssueNormalDefault = Program.MainForm.AppInfo.GetString(
                    "entityform_optiondlg",
                    "issue_normalRegister_default",
                    "<root />");
                int nRet = this.issueEditControl_normalRegisterDefault.SetData(
                    strIssueNormalDefault,
                     "",
                     null,
                     out strError);
                if (nRet == -1)
                    MessageBox.Show(this, strError);

                this.issueEditControl_normalRegisterDefault.SetReadOnly("librarian");
                this.issueEditControl_normalRegisterDefault.GetValueTable += new GetValueTableEventHandler(entityEditControl_GetValueTable);
            }

            // 期快速登记
            {
                string strIssueQuickDefault = Program.MainForm.AppInfo.GetString(
                    "entityform_optiondlg",
                    "issue_quickRegister_default",
                    "<root />");
                int nRet = this.issueEditControl_quickRegisterDefault.SetData(
                    strIssueQuickDefault,
                     "",
                     null,
                     out strError);
                if (nRet == -1)
                    MessageBox.Show(this, strError);

                this.issueEditControl_quickRegisterDefault.SetReadOnly("librarian");
                this.issueEditControl_quickRegisterDefault.GetValueTable += new GetValueTableEventHandler(entityEditControl_GetValueTable);
            }


            // 订购一般登记
            {
                string strOrderNormalDefault = Program.MainForm.AppInfo.GetString(
                    "entityform_optiondlg",
                    "order_normalRegister_default",
                    "<root />");
                int nRet = this.orderEditControl_normalRegisterDefault.SetData(
                    strOrderNormalDefault,
                     "",
                     null,
                     out strError);
                if (nRet == -1)
                    MessageBox.Show(this, strError);

                this.orderEditControl_normalRegisterDefault.SetReadOnly("librarian");
                this.orderEditControl_normalRegisterDefault.GetValueTable += new GetValueTableEventHandler(entityEditControl_GetValueTable);
            }

            // 2012/12/26
            // 评注一般登记
            {
                string strCommentNormalDefault = Program.MainForm.AppInfo.GetString(
                    "entityform_optiondlg",
                    "comment_normalRegister_default",
                    "<root />");
                int nRet = this.commentEditControl1.SetData(
                    strCommentNormalDefault,
                     "",
                     null,
                     out strError);
                if (nRet == -1)
                    MessageBox.Show(this, strError);

                this.commentEditControl1.SetReadOnly("librarian");
                this.commentEditControl1.GetValueTable += new GetValueTableEventHandler(entityEditControl_GetValueTable);
            }


            // 校验条码
            this.checkBox_verifyItemBarcode.Checked = Program.MainForm.AppInfo.GetBoolean(
                "entity_form",
                "verify_item_barcode",
                false);

            this.HidePages();
        }

        void entityEditControl_GetValueTable(object sender, GetValueTableEventArgs e)
        {
            string strError = "";
            string[] values = null;
            int nRet = Program.MainForm.GetValueTable(e.TableName,
                e.DbName,
                out values,
                out strError);
            if (nRet == -1)
                MessageBox.Show(this, strError);
            e.values = values;
        }

        private void EntityFormOptionDlg_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.entityEditControl_normalRegisterDefault.GetValueTable -= new GetValueTableEventHandler(entityEditControl_GetValueTable);
            this.entityEditControl_quickRegisterDefault.GetValueTable -= new GetValueTableEventHandler(entityEditControl_GetValueTable);

            this.issueEditControl_normalRegisterDefault.GetValueTable -= new GetValueTableEventHandler(entityEditControl_GetValueTable);
            this.issueEditControl_quickRegisterDefault.GetValueTable -= new GetValueTableEventHandler(entityEditControl_GetValueTable);

            this.orderEditControl_normalRegisterDefault.GetValueTable -= new GetValueTableEventHandler(entityEditControl_GetValueTable);

            this.commentEditControl1.GetValueTable -= new GetValueTableEventHandler(entityEditControl_GetValueTable);
        }

        private void button_OK_Click(object sender, EventArgs e)
        {
            string strError = "";

            // 检查各个字段内容是否正确
            // return:
            //      -1  有错
            //      0   正确
            if (this.orderEditControl_normalRegisterDefault.VerifyFields(out strError) == -1)
            {
                strError = "一般订购缺省值定义有误：" + strError;
                goto ERROR1;
            }

            // 册一般登记
            {
                string strNormalDefault = "";
                this.entityEditControl_normalRegisterDefault.ParentId = "?";
                int nRet = this.entityEditControl_normalRegisterDefault.GetData(
                    true,
                    out strNormalDefault,
                    out strError);
                if (nRet == -1)
                    MessageBox.Show(this, strError);
                else
                {
                    Program.MainForm.AppInfo.SetString(
                        "entityform_optiondlg",
                        "normalRegister_default",
                        strNormalDefault);
                }

                Program.MainForm.AppInfo.SetBoolean(
"entityform_optiondlg",
"normalRegister_simple",
this.checkBox_normalRegister_simple.Checked);
            }

            // 册快速登记
            {
                string strQuickDefault = "";

                this.entityEditControl_quickRegisterDefault.ParentId = "?";
                int nRet = this.entityEditControl_quickRegisterDefault.GetData(
                    true,
                    out strQuickDefault,
                    out strError);
                if (nRet == -1)
                    MessageBox.Show(this, strError);
                else
                {
                    Program.MainForm.AppInfo.SetString(
                        "entityform_optiondlg",
                        "quickRegister_default",
                        strQuickDefault);
                }
            }

            // 期一般登记
            {
                string strIssueNormalDefault = "";
                this.issueEditControl_normalRegisterDefault.ParentId = "?";
                int nRet = this.issueEditControl_normalRegisterDefault.GetData(
                    true,
                    out strIssueNormalDefault,
                    out strError);
                if (nRet == -1)
                    MessageBox.Show(this, strError);
                else
                {
                    Program.MainForm.AppInfo.SetString(
                        "entityform_optiondlg",
                        "issue_normalRegister_default",
                        strIssueNormalDefault);
                }
            }

            // 期快速登记
            {
                string strIssueQuickDefault = "";

                this.issueEditControl_quickRegisterDefault.ParentId = "?";
                int nRet = this.issueEditControl_quickRegisterDefault.GetData(
                    true,
                    out strIssueQuickDefault,
                    out strError);
                if (nRet == -1)
                    MessageBox.Show(this, strError);
                else
                {
                    Program.MainForm.AppInfo.SetString(
                        "entityform_optiondlg",
                        "issue_quickRegister_default",
                        strIssueQuickDefault);
                }
            }

            // 订购一般登记
            {
                string strOrderNormalDefault = "";
                this.orderEditControl_normalRegisterDefault.ParentId = "?";
                int nRet = this.orderEditControl_normalRegisterDefault.GetData(
                    true,
                    out strOrderNormalDefault,
                    out strError);
                if (nRet == -1)
                    MessageBox.Show(this, strError);
                else
                {
                    Program.MainForm.AppInfo.SetString(
                        "entityform_optiondlg",
                        "order_normalRegister_default",
                        strOrderNormalDefault);
                }
            }

            // 评注一般登记
            {
                string strCommentNormalDefault = "";
                this.commentEditControl1.ParentId = "?";
                int nRet = this.commentEditControl1.GetData(
                    true,
                    out strCommentNormalDefault,
                    out strError);
                if (nRet == -1)
                    MessageBox.Show(this, strError);
                else
                {
                    Program.MainForm.AppInfo.SetString(
                        "entityform_optiondlg",
                        "comment_normalRegister_default",
                        strCommentNormalDefault);
                }
            }
            Program.MainForm.AppInfo.SetBoolean(
                "entity_form",
                "verify_item_barcode",
                this.checkBox_verifyItemBarcode.Checked);

            this.DialogResult = DialogResult.OK;
            this.Close();
            return;
        ERROR1:
            MessageBox.Show(this, strError);
        }

        private void button_Cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void checkBox_normalRegister_simple_CheckedChanged(object sender, EventArgs e)
        {
            this.entityEditControl_normalRegisterDefault.DisplayMode = this.checkBox_normalRegister_simple.Checked == true ? "simple" : "full";
        }

        List<Control> _freeControls = new List<Control>();

        void DisposeFreeControls()
        {
            ControlExtention.DisposeFreeControls(_freeControls);
        }

        void HidePages()
        {
            if (StringUtil.IsInList("all", this.DisplayStyle) == true)
                return;

            for (int i = 0; i < this.tabControl_main.TabPages.Count; i++)
            {
                TabPage page = this.tabControl_main.TabPages[i];
                string strPageName = page.Tag as string;
                if (StringUtil.IsInList(strPageName, this.DisplayStyle) == false)
                {
                    this.tabControl_main.TabPages.Remove(page);
                    ControlExtention.AddFreeControl(_freeControls, page);
                    i--;
                }
            }
        }

        // 设置默认值模板的字段值
        // parameters:
        //      strFieldName    quickRegister_default/normalRegister_default/issue_quickRegister_default/issue_normalRegister_default 等
        public static void SetFieldValue(string strEntry, 
            string strFieldName,
            string strValue)
        {
            string strNormalDefault = Program.MainForm.AppInfo.GetString(
    "entityform_optiondlg",
    strEntry,
    "<root />");
            XmlDocument dom = new XmlDocument();
            dom.LoadXml(strNormalDefault);

            DomUtil.SetElementText(dom.DocumentElement, strFieldName, strValue);

            Program.MainForm.AppInfo.SetString(
    "entityform_optiondlg",
    strEntry,
    dom.DocumentElement.OuterXml);
        }

        // 获得默认值模板的字段值
        public static string GetFieldValue(string strEntry,
            string strFieldName)
        {
            string strNormalDefault = Program.MainForm.AppInfo.GetString(
    "entityform_optiondlg",
    strEntry,
    "<root />");
            XmlDocument dom = new XmlDocument();
            dom.LoadXml(strNormalDefault);

            return DomUtil.GetElementText(dom.DocumentElement, strFieldName);
        }
    }
}