using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DynamicInvokeMethodForCS
{
    //声明委托类型
    public delegate double CalculateDelegate(double x,double y);

    public partial class frmDynamicInvokeMethod : Form
    {
        public frmDynamicInvokeMethod()
        {
            InitializeComponent();
            curOpt = Add;
        }
   #region "四个完成操作的方法"

        double Add(double x, double y)
        {
            return x + y;
        }
        double Subtract(double x, double y)
        {
            return x - y;
        }
        double Multiply(double x, double y)
        {
            return x * y;
        }
        double Divid(double x, double y)
        {
            return x / y;
        }

   #endregion

        //当前操作类型，默认为加法
        private CalculateDelegate curOpt;
        //完成计算工作
        void DoCalculate(CalculateDelegate calmethod)
        {
            double x, y;
            try
            {
                x = Convert.ToDouble(txtNumber1.Text);
                y = Convert.ToDouble(txtNumber2.Text);
                lblInfo.Text = String.Format("结果：{0}", calmethod(x, y));
            }
            catch (Exception e)
            {
                lblInfo.Text = String.Format("结果：非法数据");
            }
        }

        private void txtNumber1_TextChanged(object sender, EventArgs e)
        {
            DoCalculate(curOpt);   
        }

        private void txtNumber2_TextChanged(object sender, EventArgs e)
        {
            DoCalculate(curOpt);   
        }

        private void rdoAdd_CheckedChanged(object sender, EventArgs e)
        {
            curOpt = this.Add;
            DoCalculate(curOpt); 
        }

        private void rdoSubtract_CheckedChanged(object sender, EventArgs e)
        {
            curOpt = this.Subtract ;    //选择函数
            DoCalculate(curOpt);        //调用函数
        }

        private void rdoMultiply_CheckedChanged(object sender, EventArgs e)
        {
            curOpt = this.Multiply ;
            DoCalculate(curOpt); 

        }

        private void rdoDivid_CheckedChanged(object sender, EventArgs e)
        {
            curOpt = this.Divid ;
            DoCalculate(curOpt); 
        }

    }
}