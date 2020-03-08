using System;
using System.Collections.Generic;
using System.Text;
using ComponentInterfaceLibrary;
using System.Windows.Forms;


namespace NodeComponent1
{
    public class NodeInfo : INodeComponentInterface
    {
        IconInfo INodeComponentInterface.SelectedIcon
        {
            get
            {
                IconInfo nodeIcon;
                nodeIcon.icon = Properties.Resources.Component1SelectedIcon;
                nodeIcon.Key = "Component1SelectedIcon";
                return nodeIcon;

            }
        }

        IconInfo INodeComponentInterface.Icon
        {
            get 
            {    
                IconInfo nodeIcon;
                nodeIcon.icon = Properties.Resources.Component1Icon;
                nodeIcon.Key = "Component1Icon";
                return nodeIcon;
 
            }
        }

        string INodeComponentInterface.NodeTypeName
        {
            get 
            {
                return "组件一";
            }
        }

        private frmNode frm = null;
        Panel INodeComponentInterface.NodePanel
        {
            get 
            {
                if (frm == null)
                    frm = new frmNode();
                return frm.panel1;
            }
        }

        
    }
}
