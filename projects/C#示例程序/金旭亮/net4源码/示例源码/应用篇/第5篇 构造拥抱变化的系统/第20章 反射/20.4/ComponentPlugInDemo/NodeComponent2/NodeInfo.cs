using System;
using System.Collections.Generic;
using System.Text;
using ComponentInterfaceLibrary;

namespace NodeComponent2
{
    public class NodeInfo : INodeComponentInterface
    {


        IconInfo INodeComponentInterface.SelectedIcon
        {
            get
            {
                IconInfo nodeIcon;
                nodeIcon.icon = Properties.Resources.Component2SelectedIcon;
                nodeIcon.Key = "Component2SelectedIcon";
                return nodeIcon;

            }
        }

        IconInfo INodeComponentInterface.Icon
        {
            get
            {
                IconInfo nodeIcon;
                nodeIcon.icon = Properties.Resources.Component2Icon;
                nodeIcon.Key = "Component2Icon";
                return nodeIcon;

            }
        }

        string INodeComponentInterface.NodeTypeName
        {
            get
            {
                return "组件二";
            }
        }

        private frmNode frm = null;
        System.Windows.Forms.Panel INodeComponentInterface.NodePanel
        {
            get
            {
                if(frm==null)
                    frm = new frmNode();
                return frm.panel1;
            }
        }


    }
}
