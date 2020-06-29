using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace UseColorBrushConverter
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Random ran = new Random();
            Color clr = Color.FromRgb((byte)ran.Next(0, 255), (byte)ran.Next(0, 255), (byte)ran.Next(0, 255));

            MyDataBindObject obj = new MyDataBindObject();
            obj.FillColor = clr;

            obj.Red = clr.R;
            obj.Green = clr.G;
            obj.Blue = clr.B;

            RootStackPanel.DataContext = obj;
        }
    }
}
