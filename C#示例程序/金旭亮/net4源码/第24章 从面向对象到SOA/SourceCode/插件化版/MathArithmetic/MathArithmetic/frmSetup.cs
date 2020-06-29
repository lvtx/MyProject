using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MathArithmetic;
using System.ComponentModel.Composition;
using System.Reflection;
using System.ComponentModel.Composition.Hosting;

namespace MathArithmetic
{
    public partial class frmSetup : Form
    {
        public frmSetup()
        {
            InitializeComponent();
            FindAllAlgorithmObjs();
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
             
            }
        }

      

        #region "功能代码区"
        private void FindAllAlgorithmObjs()
        {
            


            //使用插件
            foreach (IAlgorithm plugin in Program.AlgorithmObjects.AlgorithmObjs)
            {
                lstAlgorithms.Items.Add(plugin.GetAlgorithmName());
            }
            if (lstAlgorithms.Items.Count > 0)
                lstAlgorithms.SelectedIndex = 0;
        }

        
        private void OnOK()
        {
            DialogResult = DialogResult.OK;


            if (lstAlgorithms.SelectedIndex == -1)
                lstAlgorithms.SelectedIndex = 0;
            _obj = Program.AlgorithmObjects.AlgorithmObjs[lstAlgorithms.SelectedIndex];


            

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
