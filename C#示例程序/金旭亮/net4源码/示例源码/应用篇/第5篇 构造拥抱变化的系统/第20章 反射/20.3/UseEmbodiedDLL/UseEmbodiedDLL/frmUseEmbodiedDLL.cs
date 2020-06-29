using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.IO;

namespace UseEmbodiedDLL
{
    public partial class frmUseEmbodiedDLL : Form
    {
        public frmUseEmbodiedDLL()
        {
            InitializeComponent();
        }

        Assembly embodiedAssembly;  //嵌入的程序集
        Module mod;                 //模块对象
        Type typ;                   //类型对象
        MethodInfo mtd;             //方法对象

        //完成方法调用前的准备工作
        private void PrepareMethodInvoke()
        {
            //读取资源
            Byte[] bs = UseEmbodiedDLL.Properties.Resources.MathLibrary;
            embodiedAssembly = Assembly.Load(bs); //获取嵌入的程序集
            mod = embodiedAssembly.GetModule("MathLibrary.dll"); //获取模块对象
            typ = mod.GetType("MathLibrary.Arithmetic");//获取类型对象
        }
       
        private void btnInvokeDynamicMethod_Click(object sender, EventArgs e)
        {
            InvokeInstanceMethod();
        }
         //调用实例方法
        private void InvokeInstanceMethod()
        {
            //完成方法调用前的准备工作
            PrepareMethodInvoke();
            //创建对象
            Object obj = embodiedAssembly.CreateInstance("MathLibrary.Arithmetic");
            //获取方法对象
            mtd = typ.GetMethod("ShowForm");
            //调用方法
            mtd.Invoke(obj, null);
        }
     
        private void btnInvokeStaticMethod_Click(object sender, EventArgs e)
        {
            InvokeStaticMethod();
        }

        //调用静态方法
        private void InvokeStaticMethod()
        {
            //完成方法调用前的准备工作
            PrepareMethodInvoke();
            //获取方法
            mtd = typ.GetMethod("Add");
            //调用方法对象
            object ret = mtd.Invoke(null, new object[] { 100, 200 });
            //显示结果
            MessageBox.Show("100＋200 ＝ " + ret.ToString(), "计算结果");
        }

        
    }
}