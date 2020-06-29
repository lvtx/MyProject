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
        /// ʹ��TreeView��ʾ������
        /// </summary>
        /// <param name="element"></param>
        public void ShowVisualTree(DependencyObject element)
        {
            treeElements.Items.Clear();
            ProcessElement(element, null);            
        }

        /// <summary>
        /// �ݹ����������
        /// </summary>
        /// <param name="element"></param>
        /// <param name="previousItem"></param>
        private void ProcessElement(DependencyObject element, TreeViewItem previousItem)
        {
            
            TreeViewItem item = new TreeViewItem();
            item.Header = element.GetType().Name;
            item.IsExpanded = true;

           
            if (previousItem == null)  //�Ǹ��ڵ�
            {
                treeElements.Items.Add(item);
            }
            else  //���ӽڵ�
            {
                previousItem.Items.Add(item);
            }

            // ��ȡָ��Ԫ�ص�������Ԫ��
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(element); i++)
            {
                // �ݹ鴦��
                ProcessElement(VisualTreeHelper.GetChild(element, i), item);
            }
        }       
    }
}