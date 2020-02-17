using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FolderExplorer
{
    /// <summary>
    /// Interaction logic for VisualTreeDisplay.xaml
    /// </summary>
    public partial class VisualTreeDisplay : Window
    {
        public VisualTreeDisplay()
        {
            InitializeComponent();
        }
        public void ShowVisualTree(DependencyObject element)
        {
            //清空树
            treeElements.Items.Clear();
            //从根节点开始
            ProcessElement(element, null);
        }

        private void ProcessElement(DependencyObject element, TreeViewItem previousItem)
        {
            //从传入的元素创建一个可视化树成员
            TreeViewItem item = new TreeViewItem();
            item.Header = element.GetType().Name;
            item.IsExpanded = true;
            //检查
            if (previousItem == null)
            {
                treeElements.Items.Add(item);
            }
            else
            {
                previousItem.Items.Add(item);
            }
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(element); i++)
            {
                ProcessElement(VisualTreeHelper.GetChild(element, i), item);
            }
        }
    }
}
