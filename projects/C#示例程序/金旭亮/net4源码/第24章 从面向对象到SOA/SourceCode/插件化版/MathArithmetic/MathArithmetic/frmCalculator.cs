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
    public partial class frmCalculator : Form
    {
        public frmCalculator()
        {
            InitializeComponent();
        }
        //算法对象
        public IAlgorithm AlgorithmObj = null;

        //用户输入的数学表达式
        private string _expr;
        private string Expr
        {
            get
            {
                return _expr;
            }
            set
            {
                _expr = value;
                txtExpr.Text = value;
            }

        }


        #region "功能代码区"

        //显示提示信息
        private void ShowInfo(string info)
        {
            ToolStripStatusLabel1.Text = info;
            toolTip1.SetToolTip( StatusStrip1, info);
        }

        //清空输入框,重新输入
        private void ClearInput()
        {
            _expr = "";
            txtExpr.Text = "0.";
        }

        //计算表达式
        private void Calculate()
        {
            if (AlgorithmObj == null)
            {
                MessageBox.Show("未找到能解析四则运算表达式的插件");
                return;
            }


            double result;
            try
            {
                PreProcess.CheckExprValidate(txtExpr.Text);
                result = AlgorithmObj.Calculate(txtExpr.Text);
                txtExpr.Text = result.ToString();
                _expr = "";
                ShowInfo("单击按键输入表达式");
            }
            catch (CalculatorException ex)
            {
                ShowInfo(ex.Message);
            }
            catch (Exception ex)
            {
                ShowInfo(ex.Message);
            }

        }

        #endregion

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearInput();
        }

        private void btn7_Click(object sender, EventArgs e)
        {
            Expr += "7";
        }

        private void btn8_Click(object sender, EventArgs e)
        {
            Expr += "8";
        }

        private void btn9_Click(object sender, EventArgs e)
        {
            Expr += "9";
        }

        private void btn4_Click(object sender, EventArgs e)
        {
            Expr += "4";
        }

        private void btn5_Click(object sender, EventArgs e)
        {
            Expr += "5";
        }

        private void btn6_Click(object sender, EventArgs e)
        {
            Expr += "6";
        }

        private void btn1_Click(object sender, EventArgs e)
        {
            Expr += "1";
        }

        private void btn2_Click(object sender, EventArgs e)
        {
            Expr += "2";
        }

        private void btn3_Click(object sender, EventArgs e)
        {
            Expr += "3";
        }

        private void btn0_Click(object sender, EventArgs e)
        {
            Expr += "0";
        }

        private void btnDot_Click(object sender, EventArgs e)
        {
            Expr += ".";
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            Expr += "+";
        }

        private void btnSub_Click(object sender, EventArgs e)
        {
            Expr += "-";
        }

        private void btnMulti_Click(object sender, EventArgs e)
        {
            Expr += "*";
        }

        private void btnDiv_Click(object sender, EventArgs e)
        {
            Expr += "/";
        }

        private void btnLeft_Click(object sender, EventArgs e)
        {
            Expr += "(";
        }

        private void btnRight_Click(object sender, EventArgs e)
        {
            Expr += ")";
        }

        private void btnEqual_Click(object sender, EventArgs e)
        {
            Calculate();
        }
    }
}
