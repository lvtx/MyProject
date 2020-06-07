using System;
using System.Windows;
using System.Windows.Controls;

namespace TwoWayDataBinding
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MyData DataItem = null;
        public MainWindow()
        {
            InitializeComponent();

            DataItem = new MyData { Information = "My Data Object", Value = 100 };

            StackPanelForMyData.DataContext = DataItem;
            //绑定事件响应代码，了解数据更新时机
            txtInformation.TargetUpdated += txtInformation_TargetUpdated;
            txtInformation.SourceUpdated += txtInformation_SourceUpdated;
        }

        void txtInformation_SourceUpdated(object sender, System.Windows.Data.DataTransferEventArgs e)
        {
            Console.WriteLine("SourceUpdated: Information={0},Value={1}", DataItem.Information, DataItem.Value);
        }

        void txtInformation_TargetUpdated(object sender, System.Windows.Data.DataTransferEventArgs e)
        {
            Console.WriteLine("TargetUpdated: Information={0},Value={1}", DataItem.Information, DataItem.Value);
        }

        private void btnChangeDataSource_Click(object sender, RoutedEventArgs e)
        {
            DataItem.Value++;

            String modified = String.Format("被修改于{0}:{1}:{2}", DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
            int index = DataItem.Information.IndexOf("被修改于");
            if (index != -1)
            {
                DataItem.Information = DataItem.Information.Substring(0, index) + modified;
            }
            else
            {
                DataItem.Information += modified;
            }

        }
    }
}
