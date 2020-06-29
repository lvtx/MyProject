using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;


namespace ComponentInterfaceLibrary
{
    //用于定义树节点的图标
    public struct IconInfo
    {
        public Icon icon; //图标对象
        public string Key;//图标关键字
    }

    //所有可以加载的组件都必须实现的接口
    public interface  INodeComponentInterface
    {
        IconInfo SelectedIcon //在树中选中时显示的图标
        {
            get;
        }
        IconInfo Icon  //在树中未选中时显示的图标
        {
            get;
        }

        string NodeTypeName //组件名称
        {
            get;
        }
        //节点所对应的可视化控件所放置的面板
         Panel  NodePanel
        {
            get;
        }
    }
}
