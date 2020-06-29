using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wizard
{
    public class WizardController
    {

    //存放向导窗体的集合
        private List<frmBase> WizardForms = new List<frmBase>();

    //收集用户输入与设置的信息
        public Information info = new Information();

        public  WizardController()
        {

            //按顺序添加窗体到集合中
            WizardForms.Add(new frmStep1());
            WizardForms.Add(new frmStep2());
            //加入更多的向导窗体……


            //为所有的窗体设置相应的属性

            foreach (frmBase frm in WizardForms)
            {
                frm.controller = this;     //将向导控制器传入窗体中
                frm.DisableButton();    //第一个和最后一个窗体的特定按钮灰掉
            }
        }

        private int curIndex = 0;  //当前窗体索引


    public bool IsFirstForm
    {
        get
        {
            return curIndex==0;
        }
    }
       

    public bool IsLastForm
    {
        get
        {
            return curIndex==WizardForms.Count-1;
        }
    }
       

    //单击“下一步”
    public void GoNext()
    {
        //是否已是最后一个窗体？
        if (curIndex + 1 < WizardForms.Count)
        {
            WizardForms[curIndex].Visible = false;
            curIndex += 1;
        }
        else
            return;
       
        //显示窗体
        WizardForms[curIndex].Show();
        WizardForms[curIndex].DisableButton();
    }

    ////单击“上一步”
    public void GoPrev()
    {
        //是否第一个窗体？
        if (curIndex > 0 )
        {
            WizardForms[curIndex].Visible = false;
            curIndex -= 1;
        }
        else
            return ;
       
        //显示窗体
        WizardForms[curIndex].Show();
        WizardForms[curIndex].DisableButton();

    }

    //开始显示向导
    public void BeginWizard()
    {
        WizardForms[0].Show();
        WizardForms[curIndex].DisableButton();
    }

    //结束向导
    public void FinishWizard()
    {
        curIndex = 0;
        CloseAllForm();
    }

    private void CloseAllForm()
    {
        foreach (frmBase frm in WizardForms)
        {
            frm.Close();
        }
    }
   


}
}
