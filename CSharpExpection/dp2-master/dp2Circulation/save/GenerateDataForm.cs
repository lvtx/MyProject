﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

using DigitalPlatform;
using DigitalPlatform.Script;
using DigitalPlatform.CommonControl;

namespace dp2Circulation
{
    public partial class GenerateDataForm : Form
    {
        const int WM_SET_FOCUS = API.WM_USER + 209;

        public bool CloseWhenComplete = false;

        public object sender = null;
        public GenerateDataEventArgs e = null;

        public event TriggerActionEventHandler TriggerAction = null;
        public event RefreshMenuEventHandler SetMenu = null;
        public event DoDockEventHandler DoDockEvent = null;

        public bool Docked = false;
        // public MainForm MainForm = null;

        public int SelectedIndex = -1;  // 所选择的事项在Actions数组中的下标。注意，不是表格行的行号，因为重新排序后可能顺序就对不上了
        public ScriptAction SelectedAction = null;

        ScriptActionCollection m_actions = null;

        public ScriptActionCollection Actions
        {
            get
            {
                return this.m_actions;
            }
            set
            {
                this.m_actions = value;

                if (value != null)
                {
                    FillList();
                }
                else
                {
                    this.ActionTable.Rows.Clear();
                }

                ActionTable_SelectionChanged(null, null);
            }
        }

        public TableLayoutPanel Table
        {
            get
            {
                return this.tableLayoutPanel_main;
            }
        }


        public Font GetDefaultFont()
        {
            FontFamily family = null;
            try
            {
                family = new FontFamily("微软雅黑");
            }
            catch
            {
                return null;
            }
            float height = (float)9.0;
            if (this.Font != null)
                height = this.Font.SizeInPoints;
            return new Font(family, height, GraphicsUnit.Point);
        }

        public GenerateDataForm()
        {
            InitializeComponent();
        }

        private void GenerateDataForm_Load(object sender, EventArgs e)
        {
            if (this.Owner != null)
                this.Font = this.Owner.Font;
            else
            {
                Font default_font = GetDefaultFont();
                if (default_font != null)
                    this.Font = default_font;
            }

            // FillList();
            API.PostMessage(this.Handle, WM_SET_FOCUS, 0, 0);

        }

        void FillList()
        {
            this.ActionTable.Rows.Clear();
            if (m_actions == null)
                return;

            DpRow first_item = null;

            for (int i = 0; i < m_actions.Count; i++)
            {
                ScriptAction action = (ScriptAction)m_actions[i];

                DpRow item = new DpRow();
                if (action.Name == "-")
                    item.Style = DpRowStyle.Seperator;
                else
                {
                    DpCell cell = new DpCell(action.Name);
                    cell.Font = new Font(this.ActionTable.Font.FontFamily, 10, FontStyle.Bold, GraphicsUnit.Point);
                    item.Add(cell);

                    // 快捷键
                    cell = new DpCell();
                    if (action.ShortcutKey != (char)0)
                    {
                        cell.Text = new string(action.ShortcutKey, 1);
                        cell.Text = cell.Text.ToUpper();
                    }
                    item.Add(cell);

                    // 说明
                    item.Add(new DpCell(action.Comment));


                    // 入口函数
                    cell = new DpCell(action.ScriptEntry);
                    cell.ForeColor = SystemColors.GrayText;
                    cell.Font = new Font(this.ActionTable.Font.FontFamily, 8, GraphicsUnit.Point);
                    item.Add(cell);
                }

                if (action.Active == true)
                {
                    item.Selected = true;

                    // 2009/2/24 new add
                    if (first_item == null)
                        first_item = item;
                }

                item.Tag = action;
                this.ActionTable.Rows.Add(item);
            }

            if (first_item != null)
            {
                this.ActionTable.FocusedItem = first_item;
                first_item.EnsureVisible();
            }
        }

        private void toolStripButton_dock_Click(object sender, EventArgs e)
        {
            DoDock(true);
        }

        public void DoDock(bool bShowFixedPanel)
        {
            /*
            this.MainForm.CurrentGenerateDataControl = this.Table;
            if (bShowFixedPanel == true
                && this.MainForm.PanelFixedVisible == false)
                this.MainForm.PanelFixedVisible = true;

            this.Docked = true;
            this.Visible = false;
             * */
            if (this.DoDockEvent != null)
            {
                DoDockEventArgs e = new DoDockEventArgs();
                e.ShowFixedPanel = bShowFixedPanel;
                this.DoDockEvent(this, e);
            }
        }

        public int Count
        {
            get
            {
                return this.ActionTable.Rows.Count;
            }
        }

        public void Clear()
        {
            this.ActionTable.Rows.Clear();
        }

        private void ActionTable_DoubleClick(object sender, EventArgs e)
        {
            if (this.ActionTable.SelectedRows.Count == 0)
            {
                MessageBox.Show(this, "尚未选择事项...");
                return;
            }

            this.SelectedAction = (ScriptAction)this.ActionTable.SelectedRows[0].Tag;
            Debug.Assert(this.SelectedAction != null, "");

            this.SelectedIndex = this.Actions.IndexOf(this.SelectedAction);

            if (this.CloseWhenComplete == true)
                this.Close();

            if (this.SelectedAction != null
                && this.TriggerAction != null)
            {
                TriggerActionArgs e1 = new TriggerActionArgs();
                e1.EntryName = this.SelectedAction.ScriptEntry;
                e1.sender = this.sender;
                e1.e = this.e;
                this.TriggerAction(this, e1);
            }
        }

        public bool AutoRun
        {
            get
            {
                return this.checkBox_autoRun.Checked;
            }
            set
            {
                this.checkBox_autoRun.Checked = value;
            }
        }

        public bool TryAutoRun()
        {
            if ((Control.ModifierKeys & Keys.Alt) == Keys.Alt)
            {
                // 旁路
                return false;
            }
            
            // 自动执行
            if (this.checkBox_autoRun.Checked == true
                && this.ActionTable.SelectedRows.Count == 1)
            {
                ActionTable_DoubleClick(this, null);
                return true;
            }

            return false;
        }

        public void RefreshState()
        {
            if (this.SetMenu == null)
                return;

            RefreshMenuEventArgs e = new RefreshMenuEventArgs();
            e.Actions = this.Actions;
            e.sender = this.sender;
            e.e = this.e;

            this.SetMenu(this, e);

            DpRow first_selected_row = null;
            DpRow last_selected_row = null;

            for (int i=0;i<this.ActionTable.Rows.Count; i++)
            {
                DpRow row = this.ActionTable.Rows[i];

                if (row.Style == DpRowStyle.Seperator)
                    continue;

                ScriptAction action = (ScriptAction)row.Tag;
                if (action == null)
                {
                    Debug.Assert(false, "");
                    continue;
                }

                if (this.Actions.IndexOf(action) == -1)
                {
                    row.Selected = false;
                    continue;
                }

                if (row.Count == 0)
                    continue;

                Debug.Assert(row.Count >= 4, "");

                // 刷新一行
                row[0].Text = action.Name;
                string strText = "";
                if (action.ShortcutKey != (char)0)
                {
                    strText = new string(action.ShortcutKey, 1);
                    strText = strText.ToUpper();
                }
                row[1].Text = strText;
                row[2].Text = action.Comment;
                row[3].Text = action.ScriptEntry;

                row.Selected = action.Active;

                if (first_selected_row == null
                    && row.Selected == true)
                    first_selected_row = row;
                if (row.Selected == true)
                    last_selected_row = row;
            }

            if (first_selected_row != null)
                first_selected_row.EnsureVisible();
            if (last_selected_row != null
                && last_selected_row != first_selected_row)
                last_selected_row.EnsureVisible();
        }

        protected override bool ProcessDialogKey(
Keys keyData)
        {
            if (keyData == Keys.Enter)
            {
                this.ActionTable_DoubleClick(this, null);
                return true;
            }

            return base.ProcessDialogKey(keyData);
        }

        private void ActionTable_KeyPress(object sender, KeyPressEventArgs e)
        {
            /*
            if (e.KeyChar == (char)Keys.Enter)
            {
                this.ActionTable_DoubleClick(this, null);
            }
             * */
        }

        private void ActionTable_KeyDown(object sender, KeyEventArgs e)
        {
            Debug.Assert(this.Actions != null, "");

            char key = (char)e.KeyValue;
            if (key == (char)Keys.Enter)
            {
                this.ActionTable_DoubleClick(this, null);
                return;
            }
            else if (key == (char)Keys.Escape)
            {
                this.Close();
                return;
            }
            else if (char.IsLetter(key) == true)
            {
                foreach (ScriptAction action in this.Actions)
                {
                    if (Char.ToUpper(key) == Char.ToUpper(action.ShortcutKey))
                    {
                        this.SelectedIndex = this.Actions.IndexOf(action);
                        this.SelectedAction = action;

                        if (this.CloseWhenComplete == true)
                            this.Close();

                        if (this.SelectedAction != null
                            && this.TriggerAction != null)
                        {
                            TriggerActionArgs e1 = new TriggerActionArgs();
                            e1.EntryName = this.SelectedAction.ScriptEntry;
                            e1.sender = this.sender;
                            e1.e = this.e;
                            this.TriggerAction(this, e1);
                        }
                        return;
                    }
                }

                // Console.Beep();
            }

        }

        protected override void DefWndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case WM_SET_FOCUS:
            this.ActionTable.Focus();
                    return;
            }
            base.DefWndProc(ref m);
        }

        private void ActionTable_SelectionChanged(object sender, EventArgs e)
        {
            int nCount = this.ActionTable.SelectedRows.Count;
            if (nCount == 0)
            {
                this.button_excute.Text = "执行";
                this.button_excute.Enabled = false;
            }
            else
            {
                if (nCount > 1)
                    this.button_excute.Text = "执行("+nCount.ToString()+"项)";
                else
                    this.button_excute.Text = "执行";

                this.button_excute.Enabled = true;
            }
        }

        private void button_excute_Click(object sender, EventArgs e)
        {
            if (Actions == null || this.TriggerAction == null)
                return;

            if (this.ActionTable.SelectedRows.Count == 0)
            {
                MessageBox.Show(this, "尚未选择要执行的事项...");
                return;
            }

            if (this.CloseWhenComplete == true)
                this.Close();

            List<DpRow> selections = new List<DpRow>();
            selections.AddRange(this.ActionTable.SelectedRows);

            foreach (DpRow row in selections)
            {
                ScriptAction action = (ScriptAction)row.Tag;
                Debug.Assert(action != null, "");

                if (action != null
                    && this.TriggerAction != null)
                {
                    TriggerActionArgs e1 = new TriggerActionArgs();
                    e1.EntryName = action.ScriptEntry;
                    e1.sender = this.sender;
                    e1.e = this.e;
                    this.TriggerAction(this, e1);
                }
            }

        }
    }

    public delegate void TriggerActionEventHandler(object sender,
TriggerActionArgs e);

    public class TriggerActionArgs : EventArgs
    {
        public string EntryName = "";
        public object sender = null;
        public GenerateDataEventArgs e = null;
    }

    //
    public delegate void RefreshMenuEventHandler(object sender,
RefreshMenuEventArgs e);

    public class RefreshMenuEventArgs : EventArgs
    {
        public List<ScriptAction> Actions = null;
        public object sender = null;
        public GenerateDataEventArgs e = null;
    }

    //
    public class SetMenuEventArgs : EventArgs
    {
        public ScriptAction Action = null;
        public object sender = null;
        public GenerateDataEventArgs e = null;
    }


}
