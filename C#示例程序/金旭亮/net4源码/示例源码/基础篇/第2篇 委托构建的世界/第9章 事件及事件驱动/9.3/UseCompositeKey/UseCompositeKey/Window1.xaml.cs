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

namespace UseCompositeKey
{
    /// <summary>
    /// Window1.xaml 的交互逻辑
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (((e.KeyboardDevice.Modifiers & ModifierKeys.Control) == ModifierKeys.Control) && (e.Key == Key.P))
            {
                e.Handled = true;
                pntwin = new PrintWindow();
                pntwin.ShowDialog();
            }
            
            if (e.Key == Key.F1)
            {
                e.Handled = true;
            
                hlpwin.Show();
            }      

        }

        private PrintWindow pntwin = null;
        private HelpWindow hlpwin = new HelpWindow();

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (hlpwin != null)
            {
                hlpwin.WantedShutDown = true; //设置关闭标记
                hlpwin.Close();
            }
        } 
    }
}
