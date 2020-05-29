﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using System.Diagnostics;
using System.Configuration.Install;
using System.Security.Cryptography.X509Certificates;

using DigitalPlatform.Install;
using DigitalPlatform.GUI;
using DigitalPlatform.Xml;
using DigitalPlatform.IO;
using DigitalPlatform.Text;
using DigitalPlatform.rms.Client;

namespace dp2Library
{
    public partial class InstanceDialog : Form
    {
        public bool UninstallMode = false;

        public string SourceDir = "";   // 安装的程序文件目录
        public bool Changed = false;

        const int COLUMN_NAME = 0;
        const int COLUMN_ERRORINFO = 1;
        const int COLUMN_DATADIR = 2;
        const int COLUMN_BINDINGS = 3;

        private MessageBalloon m_firstUseBalloon = null;

        // string strCertificatSN = "";

        public InstanceDialog()
        {
            InitializeComponent();
        }

        private void InstanceDialog_Load(object sender, EventArgs e)
        {
            // Debug.Assert(false, "");

            // 卸载状态
            if (UninstallMode == true)
            {
                this.button_OK.Text = "卸载";
                this.button_newInstance.Visible = false;
                this.button_deleteInstance.Visible = false;
                this.button_modifyInstance.Visible = false;
                // this.button_certificate.Visible = false;
            }

            
            string strError = "";
            int nRet = FillInstanceList(out strError);
            if (nRet == -1)
                MessageBox.Show(this, strError);
            else
            {
                // 安装状态
                if (UninstallMode == false
                    && this.listView_instance.Items.Count == 0)
                {
                    // 提示创建第一个实例
                    ShowMessageTip();
                }
            }

            /*
            this.strCertificatSN = InstallHelper.GetProductString(
                "dp2Library",
                "cert_sn");
             * */

            listView_instance_SelectedIndexChanged(null, null);
        }

        void ShowMessageTip()
        {
            m_firstUseBalloon = new MessageBalloon();
            m_firstUseBalloon.Parent = this.button_newInstance;
            m_firstUseBalloon.Title = "安装 dp2Library 图书馆应用服务器";
            m_firstUseBalloon.TitleIcon = TooltipIcon.Info;
            m_firstUseBalloon.Text = "请按此按钮创建第一个实例";

            m_firstUseBalloon.Align = BalloonAlignment.BottomRight;
            m_firstUseBalloon.CenterStem = false;
            m_firstUseBalloon.UseAbsolutePositioning = false;
            m_firstUseBalloon.Show();
        }

        void HideMessageTip()
        {
            if (m_firstUseBalloon == null)
                return;

            m_firstUseBalloon.Dispose();
            m_firstUseBalloon = null;
        }

        private void button_OK_Click(object sender, EventArgs e)
        {
            string strError = "";
            int nRet = 0;

            HideMessageTip();

            // 全部卸载
            if (this.UninstallMode == true)
            {
                nRet = this.DeleteAllInstanceAndDataDir(out strError);
                if (nRet == -1)
                    MessageBox.Show(this, strError);

                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                this.Close();
                return;
            }

            // 进行检查
            // return:
            //      -1  发现错误
            //      0   放弃整个保存操作
            //      1   一切顺利
            nRet = DoVerify(out strError);
            if (nRet == -1 || nRet == 0)
                goto ERROR1;

            nRet = DoModify(out strError);
            if (nRet == -1)
                goto ERROR1;


            /*
            InstallHelper.SetProductString(
    "dp2Library",
    "cert_sn",
    this.strCertificatSN);
             * */


            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
            return;
        ERROR1:
            MessageBox.Show(this, strError);
        }

        private void button_Cancel_Click(object sender, EventArgs e)
        {
            HideMessageTip();

            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }

        public string Comment
        {
            get
            {
                return this.textBox_Comment.Text;
            }
            set
            {
                this.textBox_Comment.Text = value;
            }
        }

        // 获得一个目前尚未被使用过的instancename值
        string GetNewInstanceName(int nStart)
        {
            REDO:
            string strResult = "instance" + nStart.ToString();
            for (int i = 0; i < this.listView_instance.Items.Count; i++)
            {
                ListViewItem item = this.listView_instance.Items[i];
                string strInstanceName = ListViewUtil.GetItemText(item, COLUMN_NAME);

                if (string.Compare(strResult, strInstanceName, true) == 0)
                {
                    nStart++;
                    goto REDO;
                }
            }

            return strResult;
        }

        private void button_newInstance_Click(object sender, EventArgs e)
        {
            HideMessageTip();


            OneInstanceDialog new_instance_dlg = new OneInstanceDialog();
            GuiUtil.AutoSetDefaultFont(new_instance_dlg);
            new_instance_dlg.Text = "创建一个新实例";
            new_instance_dlg.IsNew = true;
            if (this.listView_instance.Items.Count == 0)
            {
            }
            else
            {
                new_instance_dlg.InstanceName = GetNewInstanceName(this.listView_instance.Items.Count + 1);
            }

            new_instance_dlg.VerifyInstanceName += new VerifyEventHandler(new_instance_dlg_VerifyInstanceName);
            new_instance_dlg.VerifyDataDir += new VerifyEventHandler(new_instance_dlg_VerifyDataDir);
            new_instance_dlg.VerifyBindings += new VerifyEventHandler(new_instance_dlg_VerifyBindings);
            new_instance_dlg.LoadXmlFileInfo += new LoadXmlFileInfoEventHandler(new_instance_dlg_LoadXmlFileInfo);

            new_instance_dlg.StartPosition = FormStartPosition.CenterScreen;
            if (new_instance_dlg.ShowDialog(this) == System.Windows.Forms.DialogResult.Cancel)
                return;

            ListViewItem item = new ListViewItem();
            ListViewUtil.ChangeItemText(item, COLUMN_NAME, new_instance_dlg.InstanceName);
            ListViewUtil.ChangeItemText(item, COLUMN_DATADIR, new_instance_dlg.DataDir);
            ListViewUtil.ChangeItemText(item, COLUMN_BINDINGS, new_instance_dlg.Bindings.Replace("\r\n", ";"));
            this.listView_instance.Items.Add(item);

            new_instance_dlg.LineInfo.Changed = true;
            item.Tag = new_instance_dlg.LineInfo;

            ListViewUtil.SelectLine(item, true);

            this.Changed = true;
        }

        void new_instance_dlg_LoadXmlFileInfo(object sender, LoadXmlFileInfoEventArgs e)
        {
            Debug.Assert(String.IsNullOrEmpty(e.DataDir) == false, "");

            string strError = "";
            LineInfo info = new LineInfo();
            // return:
            //      -1  error
            //      0   file not found
            //      1   succeed
            int nRet = info.Build(e.DataDir,
                out strError);
            if (nRet == -1)
            {
                e.ErrorInfo = strError;
                return;
            }

            Debug.Assert(nRet == 1, "");

            e.LineInfo = info;
        }

        void new_instance_dlg_VerifyBindings(object sender, VerifyEventArgs e)
        {
            string strError = "";
            // return:
            //      -1  出错
            //      0   不重
            //      1    重复
            int nRet = IsBindingDup(e.Value,
                (ListViewItem)null,
                out strError);
            if (nRet != 0)
            {
                e.ErrorInfo = strError;
                return;
            }

            nRet = InstallHelper.IsGlobalBindingDup(e.Value,
                "dp2Library",
                out strError);
            if (nRet != 0)
            {
                e.ErrorInfo = strError;
                return;
            }
        }

        void new_instance_dlg_VerifyDataDir(object sender, VerifyEventArgs e)
        {
            bool bRet = IsDataDirDup(e.Value,
                (ListViewItem)null);
            if (bRet == true)
                e.ErrorInfo = "数据目录 '" + e.Value + "' 和已存在的其他实例发生了重复";
        }

        void new_instance_dlg_VerifyInstanceName(object sender, VerifyEventArgs e)
        {
            bool bRet = IsInstanceNameDup(e.Value,
                (ListViewItem)null);
            if (bRet == true)
                e.ErrorInfo = "实例名 '" + e.Value + "' 和已存在的其他实例发生了重复";
        }

        ListViewItem m_currentEditItem = null;

        private void button_modifyInstance_Click(object sender, EventArgs e)
        {
            string strError = "";

            HideMessageTip();


            if (this.listView_instance.SelectedItems.Count == 0)
            {
                strError = "尚未选择要修改的事项";
                goto ERROR1;
            }

            ListViewItem item = this.listView_instance.SelectedItems[0];
            this.m_currentEditItem = item;

            OneInstanceDialog modify_instance_dlg = new OneInstanceDialog();
            GuiUtil.AutoSetDefaultFont(modify_instance_dlg);
            modify_instance_dlg.Text = "修改一个实例";
            modify_instance_dlg.InstanceName = ListViewUtil.GetItemText(item, COLUMN_NAME);
            modify_instance_dlg.DataDir = ListViewUtil.GetItemText(item, COLUMN_DATADIR);
            modify_instance_dlg.Bindings = ListViewUtil.GetItemText(item, COLUMN_BINDINGS).Replace(";", "\r\n");
            modify_instance_dlg.LineInfo = (LineInfo)item.Tag;

            modify_instance_dlg.VerifyInstanceName += new VerifyEventHandler(modify_instance_dlg_VerifyInstanceName);
            modify_instance_dlg.VerifyDataDir += new VerifyEventHandler(modify_instance_dlg_VerifyDataDir);
            modify_instance_dlg.VerifyBindings += new VerifyEventHandler(modify_instance_dlg_VerifyBindings);
            modify_instance_dlg.LoadXmlFileInfo += new LoadXmlFileInfoEventHandler(modify_instance_dlg_LoadXmlFileInfo);

            modify_instance_dlg.StartPosition = FormStartPosition.CenterScreen;
            if (modify_instance_dlg.ShowDialog(this) == System.Windows.Forms.DialogResult.Cancel)
                return;

            ListViewUtil.ChangeItemText(item, COLUMN_NAME, modify_instance_dlg.InstanceName);
            ListViewUtil.ChangeItemText(item, COLUMN_DATADIR, modify_instance_dlg.DataDir);
            ListViewUtil.ChangeItemText(item, COLUMN_BINDINGS, modify_instance_dlg.Bindings.Replace("\r\n", ";"));
            modify_instance_dlg.LineInfo.Changed = true;
            item.Tag = modify_instance_dlg.LineInfo;

            ListViewUtil.SelectLine(item, true);

            this.Changed = true;
            return;
        ERROR1:
            MessageBox.Show(this, strError);
            return;
        }

        void modify_instance_dlg_LoadXmlFileInfo(object sender, LoadXmlFileInfoEventArgs e)
        {
            Debug.Assert(String.IsNullOrEmpty(e.DataDir) == false, "");

            string strError = "";
            LineInfo info = new LineInfo();
            // return:
            //      -1  error
            //      0   file not found
            //      1   succeed
            int nRet = info.Build(e.DataDir,
                out strError);
            if (nRet == -1)
            {
                e.ErrorInfo = strError;
                return;
            }

            Debug.Assert(nRet == 1, "");

            e.LineInfo = info;
        }

        void modify_instance_dlg_VerifyBindings(object sender, VerifyEventArgs e)
        {
            string strError = "";
            // return:
            //      -1  出错
            //      0   不重
            //      1    重复
            int nRet = IsBindingDup(e.Value,
            this.m_currentEditItem,
            out strError);
            if (nRet != 0)
            {
                e.ErrorInfo = strError;
                return;
            }

            nRet = InstallHelper.IsGlobalBindingDup(e.Value,
                "dp2Library",
                out strError);
            if (nRet != 0)
            {
                e.ErrorInfo = strError;
                return;
            }
        }

        void modify_instance_dlg_VerifyDataDir(object sender, VerifyEventArgs e)
        {
            bool bRet = IsDataDirDup(e.Value,
                this.m_currentEditItem);
            if (bRet == true)
                e.ErrorInfo = "数据目录 '" + e.Value + "' 和已存在的其他实例发生了重复";
        }

        void modify_instance_dlg_VerifyInstanceName(object sender, VerifyEventArgs e)
        {
            bool bRet = IsInstanceNameDup(e.Value,
                this.m_currentEditItem);
            if (bRet == true)
                e.ErrorInfo = "实例名 '"+e.Value+"' 和已存在的其他实例发生了重复";
        }

        // return:
        //      false   不重
        //      true    重复
        bool IsInstanceNameDup(string strInstanceName,
            ListViewItem exclude_item)
        {
            foreach (ListViewItem item in this.listView_instance.Items)
            {
                if (item == exclude_item)
                    continue;
                string strCurrent = ListViewUtil.GetItemText(item, COLUMN_NAME);
                if (String.Compare(strInstanceName, strCurrent, true) == 0)
                    return true;
            }

            return false;
        }

        // return:
        //      false   不重
        //      true    重复
        bool IsDataDirDup(string strDataDir,
            ListViewItem exclude_item)
        {
            foreach (ListViewItem item in this.listView_instance.Items)
            {
                if (item == exclude_item)
                    continue;
                string strCurrent = ListViewUtil.GetItemText(item, COLUMN_DATADIR);
                if (String.IsNullOrEmpty(strCurrent) == true)
                    continue;

                if (PathUtil.IsEqual(strDataDir, strCurrent) == true)
                    return true;
            }

            return false;
        }


        // return:
        //      -1  出错
        //      0   不重
        //      1    重复
        int IsBindingDup(string strBindings,
            ListViewItem exclude_item,
            out string strError)
        {
            strError = "";
            int nRet = 0;

            if (string.IsNullOrEmpty(strBindings) == true)
                return 0;

            string[] bindings = strBindings.Replace("\r\n", ";").Split(new char[] { ';' });
            if (bindings.Length == 0)
                return 0;


            // 先检查strBinding里面是不是内部有重复
            if (bindings.Length > 1)
            {
                for (int i = 0; i < bindings.Length; i++)
                {
                    string strStart = bindings[i];
                    // 抽掉自己
                    List<string> temps = StringUtil.FromStringArray(bindings);
                    temps.RemoveAt(i);
                    // 检查数组中的哪个url和strOneBinding端口、地址冲突
                    // return:
                    //      -2  不冲突
                    //      -1  出错
                    //      >=0 发生冲突的url在数组中的下标
                    nRet = InstallHelper.IsBindingDup(strStart,
                        StringUtil.FromListString(temps),
                        out strError);
                    if (nRet == -1)
                        return -1;
                    if (nRet >= 0)
                    {
                        strError = "当前绑定集合 '" + strBindings + "' 中内部事项之间发生了冲突: " + strError;
                        return 1;
                    }
                }
            }

            // 对照其他事项的bindings检查是不是重复了
            foreach (ListViewItem item in this.listView_instance.Items)
            {
                if (item == exclude_item)
                    continue;
                string strCurrentBindings = ListViewUtil.GetItemText(item, COLUMN_BINDINGS);
                if (String.IsNullOrEmpty(strCurrentBindings) == true)
                    continue;

                string strInstanceName = ListViewUtil.GetItemText(item, COLUMN_NAME);

                string [] current_bindings = strCurrentBindings.Split(new char []{';'});

                for (int i = 0; i < bindings.Length; i++)
                {
                    string strStart = bindings[i];

                    // 检查数组中的哪个url和strOneBinding端口、地址冲突
                    // return:
                    //      -2  不冲突
                    //      -1  出错
                    //      >=0 发生冲突的url在数组中的下标
                    nRet = InstallHelper.IsBindingDup(strStart,
                        current_bindings,
                        out strError);
                    if (nRet == -1)
                        return -1;
                    if (nRet >= 0)
                    {
                        strError = "当前绑定集合和已存在的实例 '"+strInstanceName+"' 的绑定集合之间发生了冲突: " + strError;
                        return 1;
                    }
                }
            }

            return 0;
        }



        // 删除一个实例
        private void button_deleteInstance_Click(object sender, EventArgs e)
        {
            string strError = "";

            HideMessageTip();


            if (this.listView_instance.SelectedItems.Count == 0)
            {
                strError = "尚未选择要删除的事项";
                goto ERROR1;
            }

            DialogResult result = MessageBox.Show(this,
    "确实要删除所选择的 "+this.listView_instance.SelectedItems.Count.ToString()+" 个实例?",
    "InstanceDialog",
    MessageBoxButtons.YesNo,
    MessageBoxIcon.Question,
    MessageBoxDefaultButton.Button2);
            if (result != DialogResult.Yes)
            {
                return;
            }

            List<string> datadirs = new List<string>();
            foreach (ListViewItem item in this.listView_instance.SelectedItems)
            {
                string strDataDir = ListViewUtil.GetItemText(item, COLUMN_DATADIR);
                if (String.IsNullOrEmpty(strDataDir) == true)
                    continue;

                if (Directory.Exists(strDataDir) == false)
                    continue;

                datadirs.Add(strDataDir);
            }

            ListViewUtil.DeleteSelectedItems(this.listView_instance);

            this.Changed = true;

            // 如果数据目录已经存在，提示是否连带删除数据目录
            if (datadirs.Count > 0)
            {
                strError = "";
                result = MessageBox.Show(this,
    "所选定的实例信息已经删除。\r\n\r\n要删除它们所对应的下列数据目录么?\r\n" + StringUtil.MakePathList(datadirs, "\r\n"),
    "InstanceDialog",
    MessageBoxButtons.YesNo,
    MessageBoxIcon.Question,
    MessageBoxDefaultButton.Button1);
                if (result == DialogResult.Yes)
                {
                    foreach (string strDataDir in datadirs)
                    {
                        string strTempError = "";
                        // return:
                        //      -1  出错。包括出错后重试然后放弃
                        //      0   成功
                        int nRet = DeleteDataDir(strDataDir,
            out strTempError);
                        if (nRet == -1)
                            strError += strTempError + "\r\n";
                    }
                    if (String.IsNullOrEmpty(strError) == false)
                        goto ERROR1;
                }
            }

            return;
        ERROR1:
            MessageBox.Show(this, strError);
            return;
        }

        // return:
        //      -1  出错。包括出错后重试然后放弃
        //      0   成功
        static int DeleteDataDir(string strDataDir,
            out string strError)
        {
            strError = "";
        REDO_DELETE_DATADIR:
            try
            {
                Directory.Delete(strDataDir, true);
                return 0;
            }
            catch (Exception ex)
            {
                strError = "删除数据目录 '" + strDataDir + "' 时出错: " + ex.Message;
            }

            DialogResult temp_result = MessageBox.Show(ForegroundWindow.Instance,
strError + "\r\n\r\n是否重试?",
"删除数据目录 '" + strDataDir + "'",
MessageBoxButtons.RetryCancel,
MessageBoxIcon.Question,
MessageBoxDefaultButton.Button1);
            if (temp_result == DialogResult.Retry)
                goto REDO_DELETE_DATADIR;

            return -1;
        }

        // 根据已有的配置，填充InstanceList
        int FillInstanceList(out string strError)
        {
            strError = "";

            this.listView_instance.Items.Clear();

            int nErrorCount = 0;
            for (int i = 0; ; i++)
            {
                string strInstanceName = "";
                string strDataDir = "";
                string strCertificatSN = "";
                string[] existing_urls = null;
                string strSerialNumber = "";
                bool bRet = InstallHelper.GetInstanceInfo("dp2Library",
                    i,
                    out strInstanceName,
                    out strDataDir,
                    out existing_urls,
                    out strCertificatSN,
                    out strSerialNumber);
                if (bRet == false)
                    break;

                ListViewItem item = new ListViewItem();
                ListViewUtil.ChangeItemText(item, COLUMN_NAME, strInstanceName);
                ListViewUtil.ChangeItemText(item, COLUMN_DATADIR, strDataDir);
                ListViewUtil.ChangeItemText(item, COLUMN_BINDINGS, string.Join(";", existing_urls));
                this.listView_instance.Items.Add(item);
                LineInfo info = new LineInfo();
                item.Tag = info;

                info.CertificateSN = strCertificatSN;
                info.SerialNumber = strSerialNumber;

                // return:
                //      -1  error
                //      0   file not found
                //      1   succeed
                int nRet = info.Build(strDataDir,
                    out strError);
                if (nRet == -1)
                {
                    ListViewUtil.ChangeItemText(item, COLUMN_ERRORINFO, strError);
                    item.BackColor = Color.Red;
                    item.ForeColor = Color.White;

                    nErrorCount++;
                }
            }

            if (nErrorCount > 0)
                this.listView_instance.Columns[COLUMN_ERRORINFO].Width = 200;
            else
                this.listView_instance.Columns[COLUMN_ERRORINFO].Width = 0;

            return 0;
        }

        static bool HasDataDirDup(string strDataDir,
            List<string> dirs)
        {
            foreach (string strDir in dirs)
            {
                if (PathUtil.IsEqual(strDir, strDataDir) == true)
                    return true;
            }

            return false;
        }

        // 进行检查
        // return:
        //      -1  发现错误
        //      0   放弃整个保存操作
        //      1   一切顺利
        int DoVerify(out string strError)
        {
            strError = "";

            List<string> instance_names = new List<string>();
            List<string> data_dirs = new List<string>();

            // 检查实例名、数据目录是否重复
            for (int i = 0; i < this.listView_instance.Items.Count; i++)
            {
                ListViewItem item = this.listView_instance.Items[i];
                LineInfo info = (LineInfo)item.Tag;
                string strDataDir = ListViewUtil.GetItemText(item, COLUMN_DATADIR);
                string strInstanceName = ListViewUtil.GetItemText(item, COLUMN_NAME);
                string strBindings = ListViewUtil.GetItemText(item, COLUMN_BINDINGS);

                if (HasDataDirDup(strDataDir, data_dirs) == true)
                {
                    strError = "行 "+(i+1).ToString()+" 的数据目录 '"+strDataDir+"' 和前面某行的数据目录发生了重复";
                    return -1;
                }

                if (instance_names.IndexOf(strInstanceName) != -1)
                {
                    strError = "行 " + (i + 1).ToString() + " 的实例名 '" + strInstanceName + "' 和前面某行的实例名发生了重复";
                    return -1;
                }

                data_dirs.Add(strDataDir);
                instance_names.Add(strInstanceName);

                if (String.IsNullOrEmpty(strDataDir) == true)
                {
                    strError = "第 " + (i + 1).ToString() + " 行的数据目录尚未设置";
                    return -1;
                }

                if (String.IsNullOrEmpty(strBindings) == true)
                {
                    strError = "第 " + (i + 1).ToString() + " 行的协议绑定尚未设置";
                    return -1;
                }
            }

            // TODO: 检查绑定之间的端口是否冲突
            for (int i = 0; i < this.listView_instance.Items.Count; i++)
            {
                ListViewItem item = this.listView_instance.Items[i];
                LineInfo info = (LineInfo)item.Tag;
                string strInstanceName = ListViewUtil.GetItemText(item, COLUMN_NAME);
                string strBindings = ListViewUtil.GetItemText(item, COLUMN_BINDINGS);

                // return:
                //      -1  出错
                //      0   不重
                //      1    重复
                int nRet = IsBindingDup(strBindings,
            item,
            out strError);
                if (nRet != 0)
                {
                    strError = "实例名为 '"+strInstanceName+"' (第 " + (i + 1).ToString() + " 行)的协议绑定发生错误或者冲突: " + strError;
                    return -1;
                }

                nRet = InstallHelper.IsGlobalBindingDup(strBindings,
                    "dp2Library",
                    out strError);
                if (nRet != 0)
                {
                    strError = "实例名为 '" + strInstanceName + "' (第 " + (i + 1).ToString() + " 行)的协议绑定发生错误或者冲突: " + strError;
                    return -1;
                }
            }

            // 警告XML文件格式不正确、XML文件未找到的错误
            for (int i = 0; i < this.listView_instance.Items.Count; i++)
            {
                ListViewItem item = this.listView_instance.Items[i];
                LineInfo info = (LineInfo)item.Tag;
                string strDataDir = ListViewUtil.GetItemText(item, COLUMN_DATADIR);

                if (info.XmlFileNotFound == true)
                {
                    string strText = "实例 '"+item.Text+"' 的数据目录 '" + strDataDir + "' 中没有找到 library.xml 文件。\r\n\r\n要对这个数据目录进行全新安装么?\r\n\r\n(是)进行全新安装 (否)不进行任何修改和安装 (取消)放弃全部保存操作";
                    DialogResult result = MessageBox.Show(
                        this,
        strText,
        "setup_dp2library",
        MessageBoxButtons.YesNoCancel,
        MessageBoxIcon.Question,
        MessageBoxDefaultButton.Button1);
                    if (result == System.Windows.Forms.DialogResult.No)
                    {
                        info.Changed = false;
                    }
                    if (result == System.Windows.Forms.DialogResult.Yes)
                    {
                        info.XmlFileNotFound = false;
                        info.Changed = true;
                    }
                    if (result == System.Windows.Forms.DialogResult.Cancel)
                    {
                        strError = "放弃全部保存操作";
                        return 0;
                    }
                }

                if (info.XmlFileContentError == true)
                {
                    string strText = "实例 '" + item.Text + "' 的数据目录 '" + strDataDir + "' 中已经存在的 library.xml 文件(XML)格式不正确。程序无法对它进行读取操作\r\n\r\n要对这个数据目录进行全新安装么? 这将刷新整个目录(包括database.xml文件)到最初状态\r\n\r\n(是)进行全新安装 (否)不进行任何修改和安装 (取消)放弃全部保存操作";
                    DialogResult result = MessageBox.Show(
                        this,
        strText,
        "setup_dp2library",
        MessageBoxButtons.YesNoCancel,
        MessageBoxIcon.Question,
        MessageBoxDefaultButton.Button1);
                    if (result == System.Windows.Forms.DialogResult.No)
                    {
                        info.Changed = false;
                    }
                    if (result == System.Windows.Forms.DialogResult.Yes)
                    {
                        info.Changed = true;
                        info.XmlFileNotFound = false;
                        info.XmlFileContentError = false;
                        // TODO: 是否要进行备份?
                        File.Delete(PathUtil.MergePath(strDataDir, "library.xml"));
                    }
                    if (result == System.Windows.Forms.DialogResult.Cancel)
                    {
                        strError = "放弃全部保存操作";
                        return 0;
                    }
                }

            }

            return 1;
        }

        // 兑现修改。
        // 创建数据目录。创建或者修改library.xml文件
        int DoModify(out string strError)
        {
            strError = "";
            int nRet = 0;

            for (int i = 0; i < this.listView_instance.Items.Count; i++)
            {
                ListViewItem item = this.listView_instance.Items[i];
                LineInfo info = (LineInfo)item.Tag;
                string strDataDir = ListViewUtil.GetItemText(item, COLUMN_DATADIR);
                string strInstanceName = ListViewUtil.GetItemText(item, COLUMN_NAME);
                string strBindings = ListViewUtil.GetItemText(item, COLUMN_BINDINGS);

                if (String.IsNullOrEmpty(strDataDir) == true)
                {
                    strError = "第 "+(i+1).ToString()+" 行的数据目录尚未设置";
                    return -1;
                }

                if (String.IsNullOrEmpty(strBindings) == true)
                {
                    strError = "第 " + (i + 1).ToString() + " 行的协议绑定尚未设置";
                    return -1;
                }

                // 探测数据目录，是否已经存在数据，是不是属于升级情形
                // return:
                //      -1  error
                //      0   数据目录不存在
                //      1   数据目录存在，但是xml文件不存在
                //      2   xml文件已经存在
                nRet = DetectDataDir(strDataDir,
            out strError);
                if (nRet == -1)
                    return -1;

                if (nRet == 2)
                {
                    // 进行升级检查

                    // 检查xml文件的版本。看看是否有必要提示升级
                    // return:
                    //      -1  error
                    //      0   没有version，即为V1格式
                    //      1   < 2.0
                    //      2   == 2.0
                    //      3   > 2.0
                    nRet = DetectXmlFileVersion(strDataDir,
            out strError);
                    if (nRet == -1)
                        return -1;
                    if (nRet < 2)
                    {
                        // 提示升级安装
                        // 从以前的rmsws数据目录升级
                        string strText = "数据目录 '" + strDataDir + "' 中已经存在以前的 V1 图书馆应用服务器版本遗留下来的数据文件。\r\n\r\n确实要利用这个数据目录来进行升级安装么?\r\n(注意：如果利用以前dp2libraryws的数据目录来进行升级安装，则必须先行卸载dp2libraryws，以避免它和(正在安装的)dp2Library同时运行引起冲突)\r\n\r\n(是)继续进行升级安装 (否)暂停安装，以重新指定数据目录";
                        DialogResult result = MessageBox.Show(
                            this,
            strText,
            "setup_dp2library",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question,
            MessageBoxDefaultButton.Button1);
                        /*
                        if (result == DialogResult.Cancel)
                        {
                            strError = "用户放弃指定数据目录。安装未完成。";
                            return -1;
                        }
                         * */
                        if (result == DialogResult.No)
                        {
                            strError = "请利用“修改”按钮重新指定实例 '" + item.Text + "' 的数据目录";
                            return -1;
                        }

                        // 刷新cfgs目录

                        info.Upgrade = true;
                    }

                    // 覆盖数据目录中的templates子目录
                    // parameters:
                    //      strRootDir  根目录
                    //      strDataDir    数据目录
                    nRet = OverwriteTemplates(
                        strDataDir,
                        out strError);
                    if (nRet == -1)
                    {
                        // 报错，但是不停止安装
                        MessageBox.Show(ForegroundWindow.Instance,
                            strError);
                    }

                }
                else
                {
                    // 需要进行最新安装
                    nRet = CreateNewDataDir(strDataDir,
    out strError);
                    if (nRet == -1)
                        return -1;
                }


                // 兑现修改
                if (info.Changed == true
                    || info.Upgrade == true)
                {
                    // 保存信息到library.xml文件中
                    // return:
                    //      -1  error
                    //      0   succeed
                    nRet = info.SaveToXml(strDataDir,
                        out strError);
                    if (nRet == -1)
                        return -1;

                    if (info.Upgrade == true)
                    {
                        nRet = UpdateXmlFileVersion(strDataDir,
                            out strError);
                        if (nRet == -1)
                        {
                            strError = "刷新database.xml文件<version>元素的时候出错: " + strError;
                            return -1;
                        }


                        // 覆盖数据目录中的cfgs子目录
                        Debug.Assert(String.IsNullOrEmpty(this.SourceDir) == false, "");
                        string strTempDataDir = PathUtil.MergePath(this.SourceDir, "temp");

                        // 1) 先备份原来的cfgs子目录
                        string strSourceDir = PathUtil.MergePath(strDataDir, "cfgs");
                        string strTargetDir = PathUtil.MergePath(strDataDir, "v1_cfgs_backup");
                        if (Directory.Exists(strTargetDir) == false)
                        {
                            MessageBox.Show(ForegroundWindow.Instance,
            "安装程序将升级位于数据目录 '" + strSourceDir + "' 中的配置文件。原有文件将自动备份在目录 '" + strTargetDir + "' 中。");
                            nRet = PathUtil.CopyDirectory(strSourceDir,
            strTargetDir,
            true,
            out strError);
                            if (nRet == -1)
                            {
                                strError = "备份目录 '" + strSourceDir + "' 到 '" + strTargetDir + "' 时发生错误：" + strError;
                                MessageBox.Show(ForegroundWindow.Instance,
                                    strError);
                            }
                        }


                        strSourceDir = PathUtil.MergePath(strTempDataDir, "cfgs");
                        strTargetDir = PathUtil.MergePath(strDataDir, "cfgs");
                    REDO:
                        try
                        {
                            nRet = PathUtil.CopyDirectory(strSourceDir,
                strTargetDir,
                true,
                out strError);
                        }
                        catch (Exception ex)
                        {
                            strError = "拷贝目录 '" + strSourceDir + "' 到配置文件目录 '" + strTargetDir + "' 发生错误：" + ex.Message;
                            DialogResult temp_result = MessageBox.Show(ForegroundWindow.Instance,
strError + "\r\n\r\n是否重试?",
"InstanceDialog",
MessageBoxButtons.RetryCancel,
MessageBoxIcon.Question,
MessageBoxDefaultButton.Button1);
                            if (temp_result == DialogResult.Retry)
                                goto REDO;
                            throw new InstallException(strError);
                        }

                        if (nRet == -1)
                        {
                            strError = "拷贝目录 '" + strSourceDir + "' 到配置文件目录 '" + strTargetDir + "' 发生错误：" + strError;
                            throw new InstallException(strError);
                        }

                        info.UpdateCfgsDir = false; // 避免重复做
                    }

                    /*
                    // 在注册表中写入instance信息
                    InstallHelper.SetInstanceInfo(
                        "dp2Library",
                        i,
                        strInstanceName,
                        strDataDir,
                        strBindings.Split(new char []{';'}),
                        info.CertificateSN);
                     * */

                    info.Changed = false;
                    info.Upgrade = false;
                }

                // 2011/7/3
                if (info.UpdateCfgsDir == true)
                {
                    // 覆盖数据目录中的cfgs子目录
                    Debug.Assert(String.IsNullOrEmpty(this.SourceDir) == false, "");
                    string strTempDataDir = PathUtil.MergePath(this.SourceDir, "temp");

                    string strSourceDir = PathUtil.MergePath(strTempDataDir, "cfgs");
                    string strTargetDir = PathUtil.MergePath(strDataDir, "cfgs");
                REDO:
                    try
                    {
                        nRet = PathUtil.CopyDirectory(strSourceDir,
    strTargetDir,
    true,
    out strError);
                    }
                    catch (Exception ex)
                    {
                        strError = "拷贝目录 '" + strSourceDir + "' 到配置文件目录 '" + strTargetDir + "' 发生错误：" + ex.Message;
                        DialogResult temp_result = MessageBox.Show(ForegroundWindow.Instance,
strError + "\r\n\r\n是否重试?",
"InstanceDialog",
MessageBoxButtons.RetryCancel,
MessageBoxIcon.Question,
MessageBoxDefaultButton.Button1);
                        if (temp_result == DialogResult.Retry)
                            goto REDO;
                        throw new InstallException(strError);
                    }

                    if (nRet == -1)
                    {
                        strError = "拷贝目录 '" + strSourceDir + "' 到配置文件目录 '" + strTargetDir + "' 发生错误：" + strError;
                        throw new InstallException(strError);
                    }
                }

                // 在注册表中写入instance信息
                // 因为可能插入或者删除任意实例，那么注册表事项需要全部重写
                InstallHelper.SetInstanceInfo(
                    "dp2Library",
                    i,
                    strInstanceName,
                    strDataDir,
                    strBindings.Split(new char[] { ';' }),
                    info.CertificateSN,
                    info.SerialNumber);
            }

            // 删除注册表中多余的instance信息
            for (int i = this.listView_instance.Items.Count; ; i++)
            {
                // 删除Instance信息
                // return:
                //      false   instance没有找到
                //      true    找到，并已经删除
                bool bRet = InstallHelper.DeleteInstanceInfo(
                    "dp2Library",
                    i);
                if (bRet == false)
                    break;
            }

            return 0;
        }

        // 覆盖数据目录中的templates子目录
        // parameters:
        //      strRootDir  根目录
        //      strDataDir    数据目录
        public int OverwriteTemplates(string strDataDir,
            out string strError)
        {
            strError = "";

            int nRet = 0;

            Debug.Assert(String.IsNullOrEmpty(this.SourceDir) == false, "");

            string strTemplatesSourceDir = PathUtil.MergePath(this.SourceDir, "temp\\templates");
            string strTemplatesTargetDir = PathUtil.MergePath(strDataDir, "templates");

            PathUtil.CreateDirIfNeed(strTemplatesTargetDir);

            nRet = PathUtil.CopyDirectory(strTemplatesSourceDir,
                strTemplatesTargetDir,
                false,  // 拷贝前不删除原来的目录
                out strError);
            if (nRet == -1)
            {
                strError = "拷贝临时模板目录 '" + strTemplatesSourceDir + "' 到数据目录之模板目录 '" + strTemplatesTargetDir + "' 时发生错误：" + strError;
                // throw new InstallException(strError);
                return -1;
            }

            return 0;
        }



        // 探测数据目录，是否已经存在数据，是不是属于升级情形
        // return:
        //      -1  error
        //      0   数据目录不存在
        //      1   数据目录存在，但是xml文件不存在
        //      2   xml文件已经存在
        public static int DetectDataDir(string strDataDir,
            out string strError)
        {
            strError = "";

            DirectoryInfo di = new DirectoryInfo(strDataDir);
            if (di.Exists == false)
                return 0;

            string strExistingLibraryFileName = PathUtil.MergePath(strDataDir,
                "library.xml");
            if (File.Exists(strExistingLibraryFileName) == true)
                return 2;

            return 1;
        }

        // 创建数据目录，并复制进基本内容
        int CreateNewDataDir(string strDataDir,
            out string strError)
        {
            strError = "";

            PathUtil.CreateDirIfNeed(strDataDir);

            // 要求在temp内准备要安装的数据文件(初次安装而不是升级安装)
            Debug.Assert(String.IsNullOrEmpty(this.SourceDir) == false, "");
            string strTempDataDir = PathUtil.MergePath(this.SourceDir, "temp");

            int nRet = PathUtil.CopyDirectory(strTempDataDir,
    strDataDir,
    true,
    out strError);
            if (nRet == -1)
            {
                strError = "拷贝临时目录 '" + strTempDataDir + "' 到数据目录 '" + strDataDir + "' 时发生错误：" + strError;
                return -1;
            }

            return 0;
        }

        // 检查xml文件的版本。看看是否有必要提示升级
        // return:
        //      -1  error
        //      0   没有version，即为V1格式
        //      1   < 2.0
        //      2   == 2.0
        //      3   > 2.0
        static int DetectXmlFileVersion(string strDataDir,
            out string strError)
        {
            strError = "";

            string strFilename = PathUtil.MergePath(strDataDir, "library.xml");
            XmlDocument dom = new XmlDocument();
            try
            {
                dom.Load(strFilename);
            }
            catch (FileNotFoundException ex)
            {
                strError = ex.Message;
                return -1;
            }
            catch (Exception ex)
            {
                strError = "加载文件 " + strFilename + " 到 XMLDOM 时出错：" + ex.Message;
                return -1;
            }

            XmlNode nodeVersion = dom.DocumentElement.SelectSingleNode("version");
            if (nodeVersion == null)
            {
                strError = "文件 " + strFilename + " 为V1格式";
                return 0;
            }

            string strVersion = nodeVersion.InnerText;
            if (String.IsNullOrEmpty(strVersion) == true)
            {
                return 0;
            }

            double version = 0;
            try
            {
                version = Convert.ToDouble(strVersion);
            }
            catch (Exception)
            {
                strError = "文件 " + strFilename + " 中<version>元素内容 '"+strVersion+"' 不合法";
                return -1;
            }

            if (version < 2.0)
            {
                return 1;
            }

            if (version == 2.0)
                return 2;
            Debug.Assert(version > 2.0, "");

            return 3;
        }

        static int UpdateXmlFileVersion(string strDataDir,
    out string strError)
        {
            strError = "";

            string strFilename = PathUtil.MergePath(strDataDir, "library.xml");
            XmlDocument dom = new XmlDocument();
            try
            {
                dom.Load(strFilename);
            }
            catch (FileNotFoundException ex)
            {
                strError = ex.Message;
                return -1;
            }
            catch (Exception ex)
            {
                strError = "加载文件 " + strFilename + " 到 XMLDOM 时出错：" + ex.Message;
                return -1;
            }

            string strVersion = DomUtil.GetElementText(dom.DocumentElement,
                "version");
            bool bUpdate = false;
            if (string.IsNullOrEmpty(strVersion) == false)
            {
                double version = 0;
                try
                {
                    version = Convert.ToDouble(strVersion);
                }
                catch (Exception)
                {
                    strError = "文件 " + strFilename + " 中<version>元素内容 '" + strVersion + "' 不合法";
                    return -1;
                }
                if (version < 2.0)
                {
                    bUpdate = true;
                }
            }
            else
                bUpdate = true;

            if (bUpdate == true)
            {
                DomUtil.SetElementText(dom.DocumentElement,
                    "version",
                    "2.0");
            }

            dom.Save(strFilename);

            return 3;
        }

#if NO
        // 修改root用户记录文件
        // parameters:
        //      strUserName 如果为null，表示不修改用户名
        //      strPassword 如果为null，表示不修改密码
        //      strRights   如果为null，表示不修改权限
        static int ModifyRootUser(string strDataDir,
            string strUserName,
            string strPassword,
            string strRights,
            out string strError)
        {
            strError = "";

            if (strUserName == null
                && strPassword == null
                && strRights == null)
                return 0;

            string strFileName = PathUtil.MergePath(strDataDir, "userdb\\0000000001.xml");

            XmlDocument dom = new XmlDocument();
            try
            {
                dom.Load(strFileName);
            }
            catch (Exception ex)
            {
                strError = "装载root用户记录文件 " + strFileName + " 到DOM时发生错误: " + ex.Message;
                return -1;
            }

            string strOldUserName = "";
            if (strUserName != null)
            {
                strOldUserName = DomUtil.GetElementText(dom.DocumentElement,
                    "name");
                DomUtil.SetElementText(dom.DocumentElement, 
                    "name", 
                    strUserName);
            }

            if (strPassword != null)
            {
                DomUtil.SetElementText(dom.DocumentElement, "password",
                    Cryptography.GetSHA1(strPassword));
            }

            if (strRights != null)
            {
                XmlNode nodeServer = dom.DocumentElement.SelectSingleNode("server");
                if (nodeServer == null)
                {
                    Debug.Assert(false, "不可能的情况");
                    strError = "root用户记录文件 " + strFileName + " 格式错误: 根元素下没有<server>元素";
                    return -1;
                }

                DomUtil.SetAttr(nodeServer, "rights", strRights);
            }

            dom.Save(strFileName);

            // 2011/3/29
            // 修改keys_name.xml文件
            if (strUserName != null
                && strUserName != strOldUserName)
            {
                strFileName = PathUtil.MergePath(strDataDir, "userdb\\keys_name.xml");

                dom = new XmlDocument();
                try
                {
                    dom.Load(strFileName);
                }
                catch (Exception ex)
                {
                    strError = "装载用户keys文件 " + strFileName + " 到DOM时发生错误: " + ex.Message;
                    return -1;
                }

                XmlNode node = dom.DocumentElement.SelectSingleNode("key/keystring[text()='" + strOldUserName + "']");
                if (node == null)
                {
                    strError = "更新用户keys文件时出错：" + "根下 key/keystring 文本值为 '"+strOldUserName+"' 的元素没有找到";
                    return -1;
                }
                node.InnerText = strUserName;
                dom.Save(strFileName);
            }

            return 0;
        }

#endif

#if NO
        // 获得root用户信息
        // return:
        //      -1  error
        //      0   succeed
        static int GetRootUserInfo(string strDataDir,
            out string strUserName,
            out string strRights,
            out string strError)
        {
            strError = "";
            strUserName = "";
            strRights = "";

            string strFileName = PathUtil.MergePath(strDataDir, "userdb\\0000000001.xml");

            XmlDocument dom = new XmlDocument();
            try
            {
                dom.Load(strFileName);
            }
            catch (Exception ex)
            {
                strError = "装载root用户记录文件 " + strFileName + " 到DOM时发生错误: " + ex.Message;
                return -1;
            }

            strUserName = DomUtil.GetElementText(dom.DocumentElement, "name");
            XmlNode nodeServer = dom.DocumentElement.SelectSingleNode("server");
            if (nodeServer == null)
            {
                Debug.Assert(false, "不可能的情况");
                strError = "root用户记录文件 " + strFileName + " 格式错误: 根元素下没有<server>元素";
                return -1;
            }

            strRights = DomUtil.GetAttr(nodeServer, "rights");
            return 0;
        }

#endif

        int DeleteAllInstanceAndDataDir(out string strError)
        {
            strError = "";

            int nRet = 0;

            for (int i = 0; i < this.listView_instance.Items.Count; i++)
            {
                ListViewItem item = this.listView_instance.Items[i];
                LineInfo info = (LineInfo)item.Tag;
                string strDataDir = ListViewUtil.GetItemText(item, COLUMN_DATADIR);
                string strInstanceName = ListViewUtil.GetItemText(item, COLUMN_NAME);

                string strFilename = PathUtil.MergePath(strDataDir, "library.xml");
                XmlDocument dom = new XmlDocument();
                try
                {
                    dom.Load(strFilename);


                    // 删除应用服务器在dp2Kernel内核中创建的数据库
                    // return:
                    //      -1  出错
                    //      0   用户放弃删除
                    //      1   已经删除
                    nRet = DeleteKernelDatabases(
                        strInstanceName,
                        strFilename,
                        out strError);
                    if (nRet == -1)
                        MessageBox.Show(this, strError);
                }
                catch (FileNotFoundException)
                {
                }
                catch (Exception ex)
                {
                    strError = "加载文件 " + strFilename + " 到 XMLDOM 时出错：" + ex.Message;
                    MessageBox.Show(this, strError);
                }

                if (string.IsNullOrEmpty(strDataDir) == false)
                {
                REDO_DELETE_DATADIR:
                    // 删除数据目录
                    try
                    {
                        Directory.Delete(strDataDir, true);
                    }
                    catch (Exception ex)
                    {
                        DialogResult temp_result = MessageBox.Show(ForegroundWindow.Instance,
    "删除实例 '" + strInstanceName + "' 的数据目录 '" + strDataDir + "' 出错：" + ex.Message + "\r\n\r\n是否重试?\r\n\r\n(Retry: 重试; Cancel: 不重试，继续后续卸载过程)",
    "卸载 dp2Library",
    MessageBoxButtons.RetryCancel,
    MessageBoxIcon.Question,
    MessageBoxDefaultButton.Button1);
                        if (temp_result == DialogResult.Retry)
                            goto REDO_DELETE_DATADIR;

                        strError += "删除实例 '" + strInstanceName + "' 的数据目录 '" + strDataDir + "' 时出错：" + ex.Message + "\r\n";
                    }
                }
            }

            // 删除注册表中所有instance信息
            for (int i = 0; ; i++)
            {
                // 删除Instance信息
                // return:
                //      false   instance没有找到
                //      true    找到，并已经删除
                bool bRet = InstallHelper.DeleteInstanceInfo(
                    "dp2Library",
                    i);
                if (bRet == false)
                    break;
            }

            if (string.IsNullOrEmpty(strError) == false)
                return -1;

            return 0;
        }

        // 删除所有用到的内核数据库
        // 专门开发给安装程序卸载时候使用
        public static int DeleteAllDatabase(
            RmsChannel channel,
            XmlDocument cfg_dom,
            out string strError)
        {
            strError = "";

            string strTempError = "";

            long lRet = 0;

            // 大书目库
            XmlNodeList nodes = cfg_dom.DocumentElement.SelectNodes("itemdbgroup/database");
            for (int i = 0; i < nodes.Count; i++)
            {
                XmlNode node = nodes[i];

                // 实体库
                string strEntityDbName = DomUtil.GetAttr(node, "name");

                if (String.IsNullOrEmpty(strEntityDbName) == false)
                {
                    lRet = channel.DoDeleteDB(strEntityDbName,
                        out strTempError);
                    if (lRet == -1 && channel.ErrorCode != ChannelErrorCode.NotFound)
                    {
                        strError += "删除实体库 '" + strEntityDbName + "' 内数据时候发生错误：" + strTempError + "; ";
                    }
                }

                // 订购库
                string strOrderDbName = DomUtil.GetAttr(node, "orderDbName");

                if (String.IsNullOrEmpty(strOrderDbName) == false)
                {
                    lRet = channel.DoDeleteDB(strOrderDbName,
                        out strTempError);
                    if (lRet == -1 && channel.ErrorCode != ChannelErrorCode.NotFound)
                    {
                        strError += "删除订购库 '" + strOrderDbName + "' 内数据时候发生错误：" + strTempError + "; ";
                    }
                }

                // 期库
                string strIssueDbName = DomUtil.GetAttr(node, "issueDbName");

                if (String.IsNullOrEmpty(strIssueDbName) == false)
                {
                    lRet = channel.DoDeleteDB(strIssueDbName,
                        out strTempError);
                    if (lRet == -1 && channel.ErrorCode != ChannelErrorCode.NotFound)
                    {
                        strError += "删除期库 '" + strIssueDbName + "' 内数据时候发生错误：" + strTempError + "; ";
                    }
                }

                // 2011/2/21
                // 评注库
                string strCommentDbName = DomUtil.GetAttr(node, "commentDbName");

                if (String.IsNullOrEmpty(strCommentDbName) == false)
                {
                    lRet = channel.DoDeleteDB(strCommentDbName,
                        out strTempError);
                    if (lRet == -1 && channel.ErrorCode != ChannelErrorCode.NotFound)
                    {
                        strError += "删除评注库 '" + strCommentDbName + "' 内数据时候发生错误：" + strTempError + "; ";
                    }
                }

                // 小书目库
                string strBiblioDbName = DomUtil.GetAttr(node, "biblioDbName");

                if (String.IsNullOrEmpty(strBiblioDbName) == false)
                {
                    lRet = channel.DoDeleteDB(strBiblioDbName,
                        out strTempError);
                    if (lRet == -1 && channel.ErrorCode != ChannelErrorCode.NotFound)
                    {
                        strError += "删除小书目库 '" + strBiblioDbName + "' 内数据时候发生错误：" + strTempError + "; ";
                    }
                }

            }


            // 读者库
            nodes = cfg_dom.DocumentElement.SelectNodes("readerdbgroup/database");
            for (int i = 0; i < nodes.Count; i++)
            {
                XmlNode node = nodes[i];
                string strDbName = DomUtil.GetAttr(node, "name");

                if (String.IsNullOrEmpty(strDbName) == false)
                {
                    lRet = channel.DoDeleteDB(strDbName,
                        out strTempError);
                    if (lRet == -1 && channel.ErrorCode != ChannelErrorCode.NotFound)
                    {
                        strError += "删除读者库 '" + strDbName + "' 内数据时候发生错误：" + strTempError + "; ";
                    }
                }
            }


            // 预约到书队列库
            XmlNode arrived_node = cfg_dom.DocumentElement.SelectSingleNode("arrived");
            if (arrived_node != null)
            {
                string strArrivedDbName = DomUtil.GetAttr(arrived_node, "dbname");
                if (String.IsNullOrEmpty(strArrivedDbName) == false)
                {
                    lRet = channel.DoDeleteDB(strArrivedDbName,
                        out strTempError);
                    if (lRet == -1 && channel.ErrorCode != ChannelErrorCode.NotFound)
                    {
                        strError += "删除预约到书库 '" + strArrivedDbName + "' 内数据时候发生错误：" + strTempError + "; ";
                    }

                }
            }

            // 违约金库
            XmlNode amerce_node = cfg_dom.DocumentElement.SelectSingleNode("amerce");
            if (amerce_node != null)
            {
                string strAmerceDbName = DomUtil.GetAttr(amerce_node, "dbname");
                if (String.IsNullOrEmpty(strAmerceDbName) == false)
                {
                    lRet = channel.DoDeleteDB(strAmerceDbName,
                        out strTempError);
                    if (lRet == -1 && channel.ErrorCode != ChannelErrorCode.NotFound)
                    {
                        strError += "删除违约金库 '" + strAmerceDbName + "' 内数据时候发生错误：" + strTempError + "; ";
                    }
                }
            }

            // 消息库
            XmlNode message_node = cfg_dom.DocumentElement.SelectSingleNode("message");
            if (message_node != null)
            {
                string strMessageDbName = DomUtil.GetAttr(message_node, "dbname");
                if (String.IsNullOrEmpty(strMessageDbName) == false)
                {
                    lRet = channel.DoDeleteDB(strMessageDbName,
                        out strTempError);
                    if (lRet == -1 && channel.ErrorCode != ChannelErrorCode.NotFound)
                    {
                        strError += "删除消息库 '" + strMessageDbName + "' 内数据时候发生错误：" + strTempError + "; ";
                    }
                }
            }

            // 实用库
            nodes = cfg_dom.DocumentElement.SelectNodes("utilDb/database");
            for (int i = 0; i < nodes.Count; i++)
            {
                XmlNode node = nodes[i];
                string strDbName = DomUtil.GetAttr(node, "name");
                string strType = DomUtil.GetAttr(node, "type");
                if (String.IsNullOrEmpty(strDbName) == false)
                {
                    lRet = channel.DoDeleteDB(strDbName,
                        out strTempError);
                    if (lRet == -1 && channel.ErrorCode != ChannelErrorCode.NotFound)
                    {
                        strError += "删除类型为 " + strType + " 的实用库 '" + strDbName + "' 内数据时发生错误：" + strTempError + "; ";
                    }
                }
            }


            if (String.IsNullOrEmpty(strError) == false)
                return -1;

            return 0;
        }

        // 删除应用服务器在dp2Kernel内核中创建的数据库
        // return:
        //      -1  出错
        //      0   用户放弃删除
        //      1   已经删除
        static int DeleteKernelDatabases(
            string strInstanceName,
            string strXmlFilename,
            out string strError)
        {
            strError = "";
            int nRet = 0;

            DialogResult result = MessageBox.Show(ForegroundWindow.Instance,
                "是否要删除应用服务器实例 '"+strInstanceName+"' 在数据库内核中创建过的全部数据库?\r\n\r\n(注：如果现在不删除，将来也可以用内核管理(dp2manager)工具进行删除)",
                "setup_dp2library",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button1);
            if (result == DialogResult.No)
                return 0;

            XmlDocument dom = new XmlDocument();
            try
            {
                dom.Load(strXmlFilename);
            }
            catch (Exception ex)
            {
                strError = "XML文件 '" + strXmlFilename + "' 装载到DOM时发生错误: " + ex.Message;
                return -1;
            }

            XmlNode rmsserver_node = dom.DocumentElement.SelectSingleNode("rmsserver");
            if (rmsserver_node == null)
            {
                strError = "<rmsserver>元素没有找到";
                return -1;
            }
            string strKernelUrl = DomUtil.GetAttr(rmsserver_node, "url");
            if (String.IsNullOrEmpty(strKernelUrl) == true)
            {
                strError = "<rmsserver>元素的url属性为空";
                return -1;
            }

            RmsChannelCollection channels = new RmsChannelCollection();

            RmsChannel channel = channels.GetChannel(strKernelUrl);
            if (channel == null)
            {
                strError = "channel == null";
                return -1;
            }

            string strUserName = DomUtil.GetAttr(rmsserver_node, "username");
            string strPassword = DomUtil.GetAttr(rmsserver_node, "password");

            string EncryptKey = "dp2circulationpassword";
            try
            {
                strPassword = Cryptography.Decrypt(
                    strPassword,
                    EncryptKey);
            }
            catch
            {
                strError = "<rmsserver>元素password属性中的密码设置不正确";
                return -1;
            }


            nRet = channel.Login(strUserName,
                strPassword,
                out strError);
            if (nRet == -1)
            {
                strError = "以用户名 '" + strUserName + "' 和密码登录内核时失败: " + strError;
                return -1;
            }

            nRet = DeleteAllDatabase(
                channel,
                dom,
                out strError);
            if (nRet == -1)
                return -1;

            return 1;
        }

        private void listView_instance_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.listView_instance.SelectedItems.Count == 0)
            {
                this.button_modifyInstance.Enabled = false;
                this.button_deleteInstance.Enabled = false;
            }
            else
            {
                this.button_modifyInstance.Enabled = true;
                this.button_deleteInstance.Enabled = true;
            }
        }

        private void listView_instance_DoubleClick(object sender, EventArgs e)
        {
            button_modifyInstance_Click(this, null);
        }

#if NO
        // 选择证书
        private void button_certificate_Click(object sender, EventArgs e)
        {
            CertificateDialog dlg = new CertificateDialog();

            dlg.SN = this.strCertificatSN;
            dlg.StartPosition = FormStartPosition.CenterScreen;
            if (dlg.ShowDialog(this) == System.Windows.Forms.DialogResult.Cancel)
                return;

            this.strCertificatSN = dlg.SN;
        }

#endif


    }

    // ListView中每一行的隐藏信息
    public class LineInfo
    {
        public bool UpdateCfgsDir = false;  // 是否要刷新数据目录的cfgs子目录内容

        public string CertificateSN = "";

        public string SerialNumber = "";

        // *** dp2Kernel服务器信息
        // dp2Kernel URL
        public string KernelUrl = "";
        // username
        public string KernelUserName = "";
        // password
        public string KernelPassword = "";

        // *** supervisor 账户信息
        public string SupervisorUserName = null;
        public string SupervisorPassword = null;  // null表示不修改以前的密码
        public string SupervisorRights = null;

        //
        public string LibraryName = "";

        // 内容是否发生过修改
        public bool Changed = false;

        // XML文件没有找到
        public bool XmlFileNotFound = false;
        // XML文件内容格式错误
        public bool XmlFileContentError = false;
        // 是否从V1的数据目录升级上来
        public bool Upgrade = false;

        public void Clear()
        {
            this.KernelUrl = "";
            this.KernelUserName = "";
            this.KernelPassword = "";

            this.SupervisorUserName = "";
            this.SupervisorPassword = "";
            this.SupervisorRights = "";

            this.LibraryName = "";

            this.XmlFileNotFound = false;
            this.XmlFileContentError = false;
            this.Changed = false;
            this.Upgrade = false;
        }

        // return:
        //      -1  error
        //      0   file not found
        //      1   succeed
        public int Build(string strDataDir,
            out string strError)
        {
            strError = "";

            this.Clear();

            string strFilename = PathUtil.MergePath(strDataDir, "library.xml");
            XmlDocument dom = new XmlDocument();
            try
            {
                dom.Load(strFilename);
            }
            catch (FileNotFoundException)
            {
                this.XmlFileNotFound = true;
                return 0;   // file not found
            }
            catch (Exception ex)
            {
                this.XmlFileContentError = true;
                strError = "加载文件 " + strFilename + " 到 XMLDOM 时出错：" + ex.Message;
                return -1;
            }

            XmlNode nodeRmsServer = dom.DocumentElement.SelectSingleNode("rmsserver");
            if (nodeRmsServer == null)
            {
                strError = "文件 " + strFilename + " 内容不合法，根下的<rmsserver>元素不存在。";
                return -1;
            }

            // DomUtil.SetAttr(nodeDatasource, "mode", null);

            this.KernelUrl = DomUtil.GetAttr(nodeRmsServer, "url");
            this.KernelUserName = DomUtil.GetAttr(nodeRmsServer, "username");

            this.KernelPassword = DomUtil.GetAttr(nodeRmsServer, "password");
            try
            {
                this.KernelPassword = Cryptography.Decrypt(this.KernelPassword, "dp2circulationpassword");
            }
            catch
            {
                strError = "<rmsserver password='???' /> 中的密码不正确";
                return -1;
            }

            // supervisor
            XmlNode nodeSupervisor = dom.DocumentElement.SelectSingleNode("accounts/account[@type='']");
            if (nodeSupervisor != null)
            {
                this.SupervisorUserName = DomUtil.GetAttr(nodeSupervisor, "name");
                this.SupervisorPassword = DomUtil.GetAttr(nodeSupervisor, "password");
                try
                {
                    this.SupervisorPassword = Cryptography.Decrypt(this.SupervisorPassword, "dp2circulationpassword");
                }
                catch
                {
                    strError = "<account password='???' /> 中的密码不正确";
                    return -1;
                }
                this.SupervisorRights = DomUtil.GetAttr(nodeSupervisor, "rights");
            }

            this.LibraryName = DomUtil.GetElementText(dom.DocumentElement,
                "libraryInfo/libraryName");

            return 1;
        }

        // return:
        //      -1  error
        //      0   succeed
        public int SaveToXml(string strDataDir,
            out string strError)
        {
            strError = "";

            string strFilename = PathUtil.MergePath(strDataDir, "library.xml");
            XmlDocument dom = new XmlDocument();
            try
            {
                dom.Load(strFilename);
            }
            catch (FileNotFoundException)
            {
                strError = "文件 " + strFilename + " 没有找到";
                return -1;
            }
            catch (Exception ex)
            {
                strError = "加载文件 " + strFilename + " 到 XMLDOM 时出错：" + ex.Message;
                return -1;
            }

            XmlNode nodeRmsServer = dom.DocumentElement.SelectSingleNode("rmsserver");
            if (nodeRmsServer == null)
            {
                // strError = "文件 " + strFilename + " 内容不合法，根下的<rmsserver>元素不存在。";
                // return -1;
                nodeRmsServer = dom.CreateElement("rmsserver");
                dom.DocumentElement.AppendChild(nodeRmsServer);
            }


            DomUtil.SetAttr(nodeRmsServer,
                "url",
                this.KernelUrl);
            DomUtil.SetAttr(nodeRmsServer,
                 "username",
                 this.KernelUserName);

            string strPassword = Cryptography.Encrypt(this.KernelPassword, "dp2circulationpassword");
            DomUtil.SetAttr(nodeRmsServer,
                "password",
                strPassword);

            // 
            XmlNode nodeAccounts = dom.DocumentElement.SelectSingleNode("accounts");
            if (nodeAccounts == null)
            {
                nodeAccounts = dom.CreateElement("accounts");
                dom.DocumentElement.AppendChild(nodeAccounts);
            }
            XmlNode nodeSupervisor = nodeAccounts.SelectSingleNode("account[@type='']");
            if (nodeSupervisor == null)
            {
                nodeSupervisor = dom.CreateElement("account");
                nodeAccounts.AppendChild(nodeSupervisor);
            }

            if (this.SupervisorUserName != null)
                DomUtil.SetAttr(nodeSupervisor, "name", this.SupervisorUserName);
            if (this.SupervisorPassword != null)
            {
                DomUtil.SetAttr(nodeSupervisor, "password",
                    Cryptography.Encrypt(this.SupervisorPassword, "dp2circulationpassword")
                    );
            }
            if (this.SupervisorRights != null)
                DomUtil.SetAttr(nodeSupervisor, "rights", this.SupervisorRights);

            if (this.LibraryName != null)
            {
                DomUtil.SetElementText(dom.DocumentElement,
                        "libraryInfo/libraryName",
                        this.LibraryName);
            }

            dom.Save(strFilename);

            return 0;
        }


    }
}
