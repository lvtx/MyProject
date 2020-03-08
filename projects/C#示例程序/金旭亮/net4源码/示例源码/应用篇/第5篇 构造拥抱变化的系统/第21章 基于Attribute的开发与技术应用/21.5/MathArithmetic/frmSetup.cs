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
    public partial class frmSetup : Form
    {
        public frmSetup()
        {
            InitializeComponent();
        }
        //算法对象
        private IAlgorithm _obj = null;
        public IAlgorithm AlgorithmObj
        {
            get
            {
                return _obj;
            }
            set
            {
                _obj = value;
                ShowInfo(value);
            }
        }

        #region "功能代码区"

        private void ShowInfo(IAlgorithm obj)
        {
            switch (obj.GetAlgorithmName())
            {
                case "Infix":
                    rdoInfix.Checked = true;
                    break;
                case "Prefix":
                    rdoPrefix.Checked = true;
                    break;
                case "ExpressTree":
                    rdoExprTree.Checked = true;
                    break;
                default:
                    throw new CalculatorException("不支持的算法");
            }
        }

        private void OnOK()
        {
            DialogResult = DialogResult.OK;

            //创建相关算法对象
            if( rdoInfix.Checked )
                _obj = new InfixAlgorithm();
          
            if(rdoPrefix.Checked)
                _obj = new PrefixAlgorithm();
            

            if(rdoExprTree.Checked)
                _obj = new ExpressTreeAlgorithm();


            Close();
        }

        private void OnCancel()
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }



        #endregion

        private void btnOK_Click(object sender, EventArgs e)
        {
            OnOK();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            OnCancel();
        }
    }
}
