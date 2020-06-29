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
using System.Windows.Markup;
using System.IO;

namespace SaveAndReloadXAMLObject
{
    /// <summary>
    /// Window1.xaml 的交互逻辑
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
            CurCanvas = DrawPanel;
        }

        private Canvas CurCanvas = null;
        private void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //获取当前鼠标的位置
            Point curP = e.GetPosition(sender as Canvas);
            Random ran = new Random();
            Color clr = Color.FromRgb((byte)ran.Next(0, 255), (byte)ran.Next(0, 255), (byte)ran.Next(0, 255));
            Brush br = new SolidColorBrush(clr);
            Rectangle rect = new Rectangle();
            rect.Width = ran.Next(1, (int)(CurCanvas.ActualWidth - curP.X - 5));
            rect.Height = ran.Next(1, (int)(CurCanvas.ActualHeight - curP.Y - 5));
            rect.SetValue(Canvas.TopProperty, curP.Y);
            rect.SetValue(Canvas.LeftProperty, curP.X);
            rect.Fill = br;
            CurCanvas.Children.Add(rect);



        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            using (FileStream fs = new FileStream("PicData.dat", FileMode.Create))
            {

                XamlWriter.Save(CurCanvas, fs);

                MessageBox.Show("数据已保存");
            }
        }
        private void btnLoad_Click(object sender, RoutedEventArgs e)
        {
            using (FileStream fs = new FileStream("PicData.dat", FileMode.Open))
            {
                //double oldWidth = DrawPanelBorder.ActualWidth;
                //double oldHeight = DrawPanelBorder.ActualHeight;
                CurCanvas = XamlReader.Load(fs) as Canvas;
                //CurCanvas.Width = oldWidth;
                //CurCanvas.Height = oldHeight;
                CurCanvas.MouseDown += Canvas_MouseDown;
                DrawPanelBorder.Child = CurCanvas;

            }
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            CurCanvas.Children.Clear();
        }

    }
}
