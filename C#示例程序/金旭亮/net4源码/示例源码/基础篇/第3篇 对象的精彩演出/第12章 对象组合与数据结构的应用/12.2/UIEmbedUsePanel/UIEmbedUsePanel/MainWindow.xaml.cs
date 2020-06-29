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

namespace UIEmbedUsePanel
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DynamicEmbedUIElement(root);
        }

        private void DynamicEmbedUIElement(Panel RootPanel)
        {
            Button btn = new Button();
            btn.Content = "Hello";
            btn.HorizontalAlignment = HorizontalAlignment.Center;
            btn.Margin = new Thickness(10);
            btn.Width = 100;
            btn.Height = 50;
            RootPanel.Children.Add(btn);

            Ellipse ep = new Ellipse();
            ep.Width = 200;
            ep.Height = 100;
            ep.Fill = new SolidColorBrush(Colors.Yellow);
            ep.Stroke = Brushes.DeepSkyBlue;
            ep.StrokeThickness = 3;
            RootPanel.Children.Add(ep);
        }
    }
}
