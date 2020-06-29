using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MathArithmetic;

namespace MathArithmetic
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

         private frmCalculator frm1 =new frmCalculator();
    private frmExpr frm2 =new frmExpr(); 

    private IAlgorithm _obj =null;
    private IAlgorithm AlgorithmObj
    {
        get
        {
            return _obj;
        }
        set
        {
             _obj = value;
            frm1.AlgorithmObj = _obj;
            frm2.AlgorithmObj = _obj;
        }
    }
   
        #region "功能区"

    //初始化系统
    private void Init()
    {
        //创建算法对象
        AlgorithmObj = new InfixAlgorithm();
        //显示标准计算器窗体
        OnShowCalculatorForm();
    }

    //根据另一窗体的面板大小改变本窗体的大小,以嵌入另一个窗体
    private void AdjustSize(Form OtherForm )
    {
        Width = OtherForm.Width;
        Height = OtherForm.Height + MenuStrip1.Height;
    }

    //显示标准的计算器界面
    private void OnShowCalculatorForm()
    {
        mnuUseStandardForm.CheckState = CheckState.Indeterminate;
        mnuUseSimple.CheckState = CheckState.Unchecked;
        frm2.Panel1.Parent = null; //隐藏已显示的子窗体面板
        AdjustSize(frm1); //调整主窗体的大小以刚好容纳子窗体面板
        frm1.Panel1.Parent = Panel1; //显示标准的计算器界面
    }

    //显示精简的表达式界面
    private void OnShowExprForm()
    {
        mnuUseStandardForm.CheckState = CheckState.Unchecked;
        mnuUseSimple.CheckState = CheckState.Indeterminate;
        frm1.Panel1.Parent = null; //隐藏已显示的子窗体面板
        AdjustSize(frm2); //调整主窗体的大小以刚好容纳子窗体
        frm2.Panel1.Parent = Panel1;  //显示精简的表达式界面
    }

    //显示算法参数窗体
    private void OnShowSetupForm()
    {
        frmSetup frm=new frmSetup();
        frm.AlgorithmObj = AlgorithmObj;
        if (frm.ShowDialog() == DialogResult.OK )
            AlgorithmObj = frm.AlgorithmObj;
            //注意：
           //设置了AlgorithmObj属性将会导致两个窗体的AlgorithmObj属性
           //被同时设置
    }

#endregion

        private void mnuExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void mnuSetup_Click(object sender, EventArgs e)
        {
            OnShowSetupForm();
        }

        private void mnuUseSimple_Click(object sender, EventArgs e)
        {
            OnShowExprForm();
        }

        private void mnuUseStandardForm_Click(object sender, EventArgs e)
        {
            OnShowCalculatorForm();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            Init();
        }
    }
}
