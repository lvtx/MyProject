using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Reflection;


namespace ShowAssembyInfo
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

       
        Assembly a;
        Module[] mods;
        Type[] types;

        //当重新装载程序集时，清空所有的输入框
        private void InitializeAllControl()
        {
            cboModules.Items.Clear();
            lstTypes.Items.Clear();
            rtfConstructor.Text = "";
        }

        private void btnLoadAssembly_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string AssemblyFileName = openFileDialog1.FileName;
              
                a = Assembly.LoadFrom(AssemblyFileName);
                InitializeAllControl();//当重新装载程序集时，清空所有的输入框
                txtAssemblyLocation.Text = a.Location;
                //获取模块清单
                mods = a.GetModules(true);
                cboModules.Items.Clear();
                foreach (Module mod in mods)
                {
                    cboModules.Items.Add(mod.Name);
                }
            }
        }
     

        private void cboModules_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (cboModules.SelectedIndex != -1)
            {
                types = mods[cboModules.SelectedIndex].GetTypes();
                foreach (Type typ in types)
                {
                    lstTypes.Items.Add(typ.Name);
                }
            }
        }
        //获取构造函数
        private string GetConstructor(Type typ)
        {
            ConstructorInfo[] ctors = typ.GetConstructors();
           
            string str = "";
            string temp = "";
            foreach (ConstructorInfo ctor in ctors)
            {
                temp = "";
                if (ctor.IsPublic)
                    temp = "public " + temp;
                else
                    if (ctor.IsPrivate)
                        temp = "private " + temp;
                if (ctor.IsStatic)
                    temp += " static";
                if (ctor.IsVirtual)
                    temp += " virtual ";

                str += temp + ctor.ToString() + "\n";
            }
            return str;
        }

       //获取所有的方法
        private string GetAllMethod(Type typ)
        {
            MethodInfo[] mths = typ.GetMethods();
            string str = "";
            string temp = "";
            foreach (MethodInfo mth in mths)
            {
                temp = "";
                if (mth.IsPublic)
                    temp  = "public " + temp ;
                else
                    if (mth.IsPrivate)
                        temp  = "private " + temp;

                if (mth.IsAbstract)
                    temp  += " abstract";
                if (mth.IsFinal)
                    temp  += " final ";
                if (mth.IsStatic)
                    temp  += " static ";
                if (mth.IsVirtual)
                    temp  += " virtual ";
                

                str += temp+ mth.ToString() + "\n";
            }
            return str;
        }

        
        //获取所有的字段
        private string GetAllField(Type typ)
        {
            FieldInfo[] flds = typ.GetFields();
            string str = "";
            string temp = "";
            foreach (FieldInfo  fld in flds)
            {
                temp = "";
                if (fld.IsPublic)
                    temp = "public " + temp;
                else
                    if (fld.IsPrivate)
                        temp = "private " + temp;

               
                str += temp+ fld.ToString() + "\n";
            }
            return str;
        }

        //获取所有的属性
        private string GetAllProperties(Type typ)
        {
            PropertyInfo[] props = typ.GetProperties();
            string str = "";
            string temp = "";
            foreach (PropertyInfo prop in props)
            {
             
                str += prop.ToString() + "\n";
            }
            return str;
        }
        private void lstTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            //获取构造函数
            if (lstTypes.SelectedIndex == -1)
                return;
            Type typ = types[lstTypes.SelectedIndex];
            rtfConstructor.Text = GetConstructor(typ);
            rtfMethod.Text = GetAllMethod(typ);
            rtfField.Text = GetAllField(typ);
            rtfProperties.Text = GetAllProperties(typ);
        }
    }
}