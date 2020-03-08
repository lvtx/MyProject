using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using InterfaceLibrary;

namespace DynamicCreateObject
{
    public partial class frmCreateObject : Form
    {
        public frmCreateObject()
        {
            InitializeComponent();
            LoadComponentLibrary("MathLibrary.dll");
        }
        //程序集对象
        private Assembly myDll;

        //装入组件库
        public void LoadComponentLibrary(string ComponentFileName)
        {
            //装入指定的组件代码库
            try 
	        {	        
		        myDll = Assembly.LoadFrom(ComponentFileName);
                if( myDll==null)
                     MessageBox.Show("Can't Load library");
	        }
	        catch (Exception e)
	        {
        		  MessageBox.Show(e.Message);
	        }
        }


        private IMathOpt mathobj;

        //动态创建对象并计算
        private void DoCalculate()
        {
            if (ComboBox1.SelectedIndex == -1)
                return;
            //获取用户的选择
            switch (ComboBox1.SelectedItem.ToString())
            {
                case "+":
                    mathobj = (IMathOpt)myDll.CreateInstance("MathLibrary.AddClass");
                    break;
                case "-":
                    mathobj = (IMathOpt)myDll.CreateInstance("MathLibrary.SubtractClass");
                    break;
                case "*":
                    mathobj = (IMathOpt)myDll.CreateInstance("MathLibrary.MultiplyClass");
                    break;
                case "/":
                    mathobj = (IMathOpt)myDll.CreateInstance("MathLibrary.DividClass");
                    break;
                default:
                    break;
            }
            //将结果显示在标签上
            lblResult.Text =  mathobj.GetIt(Convert.ToDouble(TextBox1.Text), Convert.ToDouble(TextBox2.Text)).ToString();
        }

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            DoCalculate();
        }

        private void btnEquals_Click(object sender, EventArgs e)
        {
            DoCalculate();
        }

    }
}