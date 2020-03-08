using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PrintVisualTree
{
   
    public partial class VisualTreeDisplay : System.Windows.Window
    {

        public VisualTreeDisplay()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 使用TreeView显示可视树
        /// </summary>
        /// <param name="element"></param>
        public void ShowVisualTree(DependencyObject element)
        {
            treeElements.Items.Clear();
            ProcessElement(element, null);            
        }

        /// <summary>
        /// 递归遍历可视树
        /// </summary>
        /// <param name="element"></param>
        /// <param name="previousItem"></param>
        private void ProcessElement(DependencyObject element, TreeViewItem previousItem)
        {
            
            TreeViewItem item = new TreeViewItem();
            item.Header = element.GetType().Name;
            item.IsExpanded = true;

           
            if (previousItem == null)  //是根节点
            {
                treeElements.Items.Add(item);
            }
            else  //是子节点
            {
                previousItem.Items.Add(item);
            }

            // 获取指定元素的所有子元素
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(element); i++)
            {
                // 递归处理
                ProcessElement(VisualTreeHelper.GetChild(element, i), item);
            }
        }       
    }
}