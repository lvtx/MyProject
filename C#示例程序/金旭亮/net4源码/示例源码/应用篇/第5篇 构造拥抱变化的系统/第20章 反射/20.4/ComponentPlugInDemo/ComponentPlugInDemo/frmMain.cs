using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ComponentInterfaceLibrary;
using System.Reflection;
using System.IO;
using System.Diagnostics;
using System.Collections;

namespace ComponentPlugInDemo
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
            ScanAllPlugInComponents();
        }
        //插件文件清单
        List<string> ComponentFiles;

        //获取指定文件夹下的所有DLL文件
        private List<string> GetFiles(string FolderName)
        {
            if (Directory.Exists(FolderName) == false)
                throw new ApplicationException("路径必须存在！");
            DirectoryInfo dir = null;

            try
            {

                dir = new DirectoryInfo(FolderName);

            }
            catch (ArgumentException)
            {
                throw new ApplicationException("非法的文件夹路径:" + FolderName);
            }
            //获取所有文件
            FileInfo[] files = dir.GetFiles();
            List<string> result = new List<string>();
            //检查文件是不是DLL
            foreach (FileInfo fi in files)
            {
                if (fi.Name.ToUpper().EndsWith(".DLL"))
                    result.Add(fi.FullName);
            }
            return result;

        }

        //从DLL中获取INodeComponentInterface对象
        private INodeComponentInterface GetNodeInfoObjInOneDLL(string AssemblyFileName)
        {
            //装载程序集
            Assembly ass = Assembly.LoadFrom(AssemblyFileName);
            //获取程序集中的模块
            Module[] mods = ass.GetModules();
            Type[] typs;
            object obj=null;
            //在模块中查找实现了INodeComponentInterface接口的类型
            foreach (Module mod in mods)
            {
                typs = mod.GetTypes(); //获取模块中的所有类型
                foreach (Type typ in typs)
                    //此类型是否实现了INodeComponentInterface接口
                    if(typeof(INodeComponentInterface).IsAssignableFrom(typ))
                    {
                       //创建对象
                       obj= ass.CreateInstance(typ.FullName);
                    }
            }
            return obj as INodeComponentInterface;
        }

        //用于保存所有插件面板的集合
        List<Panel> NodePanels = new List<Panel>();
        
        //扫描所有插件
        private void ScanAllPlugInComponents()
        {
            //获取plugin目录下的所有文件
            ComponentFiles = GetFiles(Application.StartupPath + "\\plugin");
            INodeComponentInterface NodeInfoObj;
            int i = 0;
            //检查所有程序集
            foreach (string ComponentFile in ComponentFiles)
            {
                //获取插件中实现了INodeComponentInterface的对象
                NodeInfoObj = GetNodeInfoObjInOneDLL(ComponentFile);
                //在树中显示一个节点，代表装入的插件
                AddNodeToTreeView(NodeInfoObj);
                //先不显示工作面板
                NodeInfoObj.NodePanel.Visible = false;
                //将工作面板插入到面板集合中，以便在需要时显示。
                NodePanels.Add(NodeInfoObj.NodePanel);
            }
        }

        //向树中加入节点
        private void AddNodeToTreeView(INodeComponentInterface obj)
        {
            //将插件图标加入到ImageList1控件中
            imageList1.Images.Add(obj.Icon.Key, obj.Icon.icon);
            imageList1.Images.Add(obj.SelectedIcon.Key, obj.SelectedIcon.icon);
            //创建一个树节点，并设定其图标
            TreeNode tn = new TreeNode(obj.NodeTypeName);
            tn.SelectedImageKey = obj.SelectedIcon.Key;
            tn.ImageKey = obj.Icon.Key;
            tn.Tag = obj; //将NodeInfo对象“挂”在节点上
            treeView1.Nodes.Add(tn); //将树节点加入TreeView的节点集合显示
        }

        private void HideAllPanels()
        {
            foreach (Panel pnl in NodePanels)
            {
                pnl.Visible = false;
            }
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            //取出挂在节点上的插件NodeInfo对象
            INodeComponentInterface obj = e.Node.Tag as INodeComponentInterface;

            HideAllPanels();//隐藏所有的节点面板

            //显示当前插件所对应节点的面板
            obj.NodePanel.Parent =splitContainer1.Panel2;
            obj.NodePanel.Visible = true;

           
         }
       }
    }
