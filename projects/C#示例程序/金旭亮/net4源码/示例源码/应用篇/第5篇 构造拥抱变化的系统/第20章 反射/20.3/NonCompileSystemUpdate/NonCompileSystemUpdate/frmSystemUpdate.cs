using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Reflection;

namespace NonCompileSystemUpdate
{
    public partial class frmSystemUpdate : Form
    {
        public frmSystemUpdate()
        {
            InitializeComponent();
        }
        //组件DLL文件名
        string ComponentFileName = "";
        //组件入口点类名
        string EntryPointClassName = "";
        //从配置文件中读取信息
        private void ReadConfigInfo(string ConfigFileName)
        {
            using (XmlTextReader reader = new XmlTextReader(ConfigFileName))
            {
                while (reader.Read())
                {
                    if (reader.Name == "Component")
                    {
                        reader.MoveToAttribute("FileName");
                        ComponentFileName = reader.Value;
                        reader.MoveToElement();
                        reader.MoveToAttribute("EntryPoint");
                        EntryPointClassName = reader.Value;
                        break;
                    }
                }
            }
        }

        private void btnOpenForm_Click(object sender, EventArgs e)
        {
            //读入配置信息
            ReadConfigInfo("System.Config.xml"); 
            //装入组件库
            Assembly ass=Assembly.LoadFrom(ComponentFileName);
            //创建组件入口点对象
            object obj = ass.CreateInstance(EntryPointClassName);
            //运行组件
            (obj as InterfaceLibrary.IMyInterface).Run();
        }

       
    }
}