using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Wizard
{
    public partial class frmBase : Form
    {
        public frmBase()
        {
            InitializeComponent();
        }
         //向导控制器
    public WizardController controller   = null;

        //检测此窗体作为向导窗体时相应属性是否已具备？
    private void CheckCondition()
    {
        if( controller == null )
            throw new ApplicationException("必须指定一个向导控制器对象:WizardController");
        
        if( controller.info == null )
            throw new ApplicationException("必须指定一个用于收集信息的Information对象");
       
    }


    public void DisableButton()
    {
        if (controller == null )
            return;
       
        if (controller.IsFirstForm)
            btnPrev.Enabled =false;
        else
            btnPrev.Enabled = true;
       
        if (controller.IsLastForm)
            btnNext.Enabled = false;
        else
            btnNext.Enabled = true;
       
    }

    protected virtual void UpdateInfo()
    {

    }

    //单击“下一步”
    protected virtual void GoNext()
    {
        UpdateInfo();
        controller.GoNext();

    }



    //单击“上一步”
    protected virtual void GoPrev()
    {
        UpdateInfo();
        controller.GoPrev();
    }

    //单击“结束”
    protected virtual void Finish()
    {
        UpdateInfo();
        controller.FinishWizard();
        Visible = false;
    }

    //单击“放弃”
    protected virtual void Cancel()
    {
        controller.FinishWizard();
        controller.info = null;
       Visible = false;
    }

    //单击“帮助”
    protected virtual void ShowHelp()
    {
        MessageBox.Show("帮助");
    }

    private void btnPrev_Click(object sender, EventArgs e)
    {
        GoPrev();
    }

    private void btnNext_Click(object sender, EventArgs e)
    {
        GoNext();
    }

    private void btnFinish_Click(object sender, EventArgs e)
    {
        Finish();
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
        Cancel();
    }

    private void btnHelp_Click(object sender, EventArgs e)
    {
        ShowHelp();
    }
    }
}
