using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TwoWayCommunicationInForms
{
    public partial class frmOther : Form
    {
        public frmOther(frmMain frm)
        {
            InitializeComponent();
            frmMainForm = frm;
        }
        //引用主窗体
        private frmMain frmMainForm = null;

        /// <summary>
        /// 允许外界设置进度条的数值
        /// </summary>
        public int WorkDownPercent
        {
            get { return ProgressBar1.Value; }
            set
            {
                ProgressBar1.Value = value;
                //显示信息
                lblInfo.Text = String.Format("{0}%", ProgressBar1.Value);
            }
        }


        /// <summary>
        /// 修改进度条控件的值，IsIncrease属性用于确定是增加还是减少
        /// </summary>
        /// <param name="IsIncrease"></param>
        private void ChangeProgressBarValue(bool IsIncrease)
        {
            if (IsIncrease) //增值
            {
                if (ProgressBar1.Value == 100)
                    ProgressBar1.Value = 0; //重新开始
                else
                    ProgressBar1.Value += 2;

            }
            else //减值
            {
                if (ProgressBar1.Value == 0)
                    ProgressBar1.Value = 0; //不允许再减少了
                else
                    ProgressBar1.Value -= 2;

            }

            //显示信息
            lblInfo.Text = String.Format("{0}%", ProgressBar1.Value);

            //向主窗体的公有方法向其传送信息
            frmMainForm.SetValue(ProgressBar1.Value);

        }

        private void btnIncrease_Click(object sender, EventArgs e)
        {
            ChangeProgressBarValue(true);
        }

        private void btnDecrease_Click(object sender, EventArgs e)
        {
            ChangeProgressBarValue(false);
        }
    }
}
