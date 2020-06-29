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
    public partial class frmExpr : Form
    {
        public frmExpr()
        {
            InitializeComponent();
        }
        //算法对象
        public IAlgorithm AlgorithmObj = null;

        #region "功能代码"

        //显示结果
        private void ShowResult(string Info)
        {
            lblInfo.Text = Info;
        }

        //显示出错信息
        private void ShowError(string Info)
        {
            ToolStripStatusLabel1.Text = Info;
        }

        //开始计算
        private void Calculate()
    {
        if( AlgorithmObj==null)
            //默认使用中序算法
            AlgorithmObj = new InfixAlgorithm();

        try
        {
            PreProcess.CheckExprValidate(txtExpr.Text);
            lblInfo.Text = "计算结果：" + AlgorithmObj.Calculate(txtExpr.Text);
            lblPrefix.Text = "前序表达式：" + Converter.InfixToPrefix(txtExpr.Text);
            lblPostfix.Text = "后序表达式：" + Converter.InfixToPostfix(txtExpr.Text);
            ShowError("在文本框中输入表达式,分析得到的前序与后序表达式以$符号分隔");
        }
        catch (CalculatorException ex)
        {
            ShowError(ex.Message); //向用户提交有意义的信息
        }
        finally
        {
            //焦点回到文本输入框
            txtExpr.SelectAll();
            txtExpr.Focus();
        }
    }


        #endregion

        private void btnCalculate_Click(object sender, EventArgs e)
        {
            Calculate();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtExpr.Text = "";
            txtExpr.Focus();
        }

        private void txtExpr_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                Calculate();
       
        }
    }
}
